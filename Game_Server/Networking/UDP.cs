using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;

using Game_Server.Managers;

namespace Game_Server.Networking
{
    /// <summary>
    /// This class handles UDP requests
    /// </summary>
    class UDP
    {
        private int port;
        private UdpClient socket;
        private byte[] buffer = new byte[4096];
        private Thread receiveConnectionsThread;
        private EndPoint endPoint;

        public bool InitializeSocket()
        {
            try
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
            catch(Exception e) {Console.WriteLine(e); }

            this.socket = null;
            this.endPoint = new IPEndPoint(IPAddress.Any, this.port);

            try
            {
                this.endPoint = new IPEndPoint(IPAddress.Any, this.port);
                this.socket = new UdpClient(this.port);
                receiveConnectionsThread = new Thread(ReceiveUDPConnections);
                receiveConnectionsThread.Priority = ThreadPriority.Highest;
                receiveConnectionsThread.SetApartmentState(ApartmentState.STA);
                receiveConnectionsThread.Start();
               
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError("Error re-initializing socket...");
            }
            return false;
        }

        public void RestartUDP()
        {
            try
            {
                socket.Close();
            }
            catch
            {
            }

            bool status = InitializeSocket();
            if (status)
            {
                Log.WriteError("UDP restarted successfully");
            }
        }

        public bool Start(int port)
        {
            this.port = port;
            bool status = InitializeSocket();
            if (status)
            {
                Log.WriteLine("Binded the UDP socket to port " + port);
            }
            return status;
        }

        private void HandleUDP(byte[] buffer, IPEndPoint IPeo)
        {
            try
            {
                byte[] response = AnalyzePacket(buffer, IPeo);
                if (response != null)
                {
                    if (response.Length > 0)
                    {
                        socket.BeginSend(response, response.Length, IPeo, new AsyncCallback(sendCallBack), socket);
                    }
                }
            }
            catch
            {
            }
        }

        public void ReceiveUDPConnections()
        {
            IPEndPoint IPeo = new IPEndPoint(IPAddress.Any, this.port);
            while (true)
            {
                try
                {
                    byte[] buffer = socket.Receive(ref IPeo);
                    if (buffer.Length > 0)
                    {
                        HandleUDP(buffer, IPeo);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError("UDP Error: " + ex.Message);
                }
            }
        }

        


        private void sendCallBack(IAsyncResult iAr)
        {
            try
            {
                Socket s = (Socket)iAr.AsyncState;
                s.EndSendTo(iAr);
            }
            catch
            {
            }
        }

        private byte[] AnalyzePacket(byte[] packet, IPEndPoint IPeo)
        {
            byte[] response = packet;
            uint type = packet.ToUShort(0);
            ushort sessionId = packet.ToUShort(4);
            int userId = (int)packet.ToUInt(10);
            User usr = UserManager.getTarGetUser(sessionId);

            UDPPacket cPacket = new UDPPacket(packet);

            switch (cPacket.identity)
            {
                case UDPPacket.Identity.Authentication:
                    {
                        if (usr != null)
                        {
                            if (usr.userId != userId) { return null; }
                            packet.WriteUShort((ushort)(this.port + 1), 4);
                            usr.setRemoteEndPoint(IPeo);
                        }
                        break;
                    }
                case UDPPacket.Identity.IP:
                    {
                        if (usr != null)
                        {
                            IPEndPoint EndPoint = packet.ToIPEndPoint(32);

                            if (usr.remoteEndPoint == EndPoint)
                            {
                                usr.setLocalEndPoint(EndPoint);
                                usr.setRemoteEndPoint(IPeo);

                                Log.WriteDebug(usr.nickname + " -> UDP Connected & Set Local End Point " + usr.localEndPoint.Address.ToString());

                                response = packet.Extend(65);

                                response[17] = 0x41;
                                response[response.Length + 1] = Game.Cryption.clientXor;
                                response.WriteUShort((ushort)usr.sessionId, 4);
                                if (usr.remoteEndPoint != null)
                                {
                                    response.WriteIPEndPoint(usr.remoteEndPoint, 32);
                                }
                                if (usr.localEndPoint != null)
                                {
                                    response.WriteIPEndPoint(usr.localEndPoint, 50);
                                }
                            }
                        }
                        break;
                    }
                case UDPPacket.Identity.Tunneling:
                    {
                        if (usr != null)
                        {
                            int roomId = packet.ToUShort(6);
                            if (usr.room != null)
                            {
                                if (usr.room.id == roomId)
                                {
                                    ushort targetID = packet.ToUShort(22);
                                    User player = Managers.UserManager.getTarGetUser(targetID);
                                    if (player != null && player.room.id == usr.room.id)
                                    {
                                        IPEndPoint tunnelEndPoint = (usr.remoteEndPoint != player.remoteEndPoint ? player.remoteEndPoint : player.localEndPoint);

                                        socket.BeginSend(response, response.Length, tunnelEndPoint, new AsyncCallback(sendCallBack), null);

                                        foreach (User u in player.room.spectators.Values)
                                        {
                                            socket.BeginSend(response, response.Length, tunnelEndPoint, new AsyncCallback(sendCallBack), null);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
            }

            return response;
        }
    }

    public class UDPPacket
    {
        public enum Identity
        {
            Unknown,
            Authentication,
            IP,
            Tunneling
        }

        public byte[] data;
        public Identity identity;

        public UDPPacket(byte[] data)
        {
            this.data = data;
            if (data.Length == 14 && data[0] == 0x10 && data[1] == 0x01 && data[2] == 0x01)
                this.identity = Identity.Authentication;
            else if (data.Length == 46 && data[0] == 0x10 && data[1] == 0x10 && data[2] == 0x00 && data[14] == 0x21)
                this.identity = Identity.IP;
            else if (data.Length > 20 && data[0] == 0x10 && data[1] == 0x10 && data[2] == 0x00 && (data[14] == 0x2E || data[14] == 0x31 || data[14] == 0x34 || data[14] == 0x30)) //todo: make this better
                this.identity = Identity.Tunneling;
            else
                this.identity = Identity.Unknown;
        }
    }

    public static class UDPReader
    {
        private const byte xOrSendKey = Game.Cryption.serverXor;
        private const byte xOrReceiveKey = Game.Cryption.clientXor;

        public static ushort ToUShort(this byte[] packet, int offset)
        {
            byte[] value = new byte[2];
            Array.Copy(packet, offset, value, 0, 2);
            Array.Reverse(value);
            return BitConverter.ToUInt16(value, 0);
        }

        public static uint ToUInt(this byte[] packet, int offset)
        {
            byte[] value = new byte[4];
            Array.Copy(packet, offset, value, 0, 4);
            Array.Reverse(value);
            return BitConverter.ToUInt32(value, 0);
        }

        public static IPEndPoint ToIPEndPoint(this byte[] packet, int offset)
        {
            for (int i = offset; i < offset + 6; i++)
            {
                packet[i] ^= xOrSendKey;
            }
            ushort port = BitConverter.ToUInt16(packet, offset);
            uint ip = BitConverter.ToUInt32(packet, offset + 2);
            return new IPEndPoint(ip, port);
        }

        public static void WriteUShort(this byte[] packet, ushort value, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Array.Copy(bytes, 0, packet, offset, 2);
        }

        public static void WriteUInt(this byte[] packet, uint value, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            Array.Copy(bytes, 0, packet, offset, 4);
        }

        public static void WriteIPEndPoint(this byte[] packet, IPEndPoint endpoint, int offset)
        {
            byte[] value = new byte[6];
            Array.Copy(BitConverter.GetBytes(endpoint.Port), 0, value, 0, 2);
            Array.Copy(endpoint.Address.GetAddressBytes(), 0, value, 2, 4);
            Array.Reverse(value);
            for (int i = offset; i < offset + 6; i++)
            {
                packet[i] = (byte)(value[i - offset] ^ xOrReceiveKey);
            }
        }

        public static byte[] Extend(this byte[] packet, int length)
        {
            byte[] newPacket = new byte[length];
            Array.Copy(packet, newPacket, packet.Length);
            return newPacket;
        }
    }
}
