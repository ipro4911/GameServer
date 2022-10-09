/*
 _____   ___ __  __  _____ _____   ___  _  __              ___   ___   __    __ 
/__   \ /___\\ \/ /  \_   \\_   \ / __\( )/ _\            / __\ /___\ /__\  /__\
  / /\///  // \  /    / /\/ / /\// /   |/ \ \            / /   //  /// \// /_\  
 / /  / \_//  /  \ /\/ /_/\/ /_ / /___    _\ \          / /___/ \_/// _  \//__  
 \/   \___/  /_/\_\\____/\____/ \____/    \__/          \____/\___/ \/ \_/\__/  
__________________________________________________________________________________

Created by: ToXiiC
Thanks to: CodeDragon, Kill1212, CodeDragon

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Room_Data
{
    class RoomHandler_VoteKick : RoomDataHandler
    {
        public override void Handle(User usr, Room room)
        {
            // [6] = Type vote casted.
            // [7] = Kick Target
            byte TargetSlot = byte.Parse(getBlock(7));
            bool startVote = int.Parse(getBlock(6)) == 0;
            if (TargetSlot == usr.roomslot || room.mode == 1 || room.channel == 3) return;
            User Target = room.GetUser(TargetSlot);
            int UserSide = room.GetSide(usr);
            int TeamCount = room.GetSideCount(UserSide);
            int VoteType = int.Parse(getBlock(6));
        //    Log.WriteDebug(TargetSlot + " - " + TeamCount + " - " + VoteType);

            if (Target == null)// && TargetSlot == usr.roomslot) TODO: add a check if player has anti votekick hack
            {
                usr.send(new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.InvalidCandidate));
            }
            else if (room.GetSideCount(UserSide) < 4)
            {
                usr.send(new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.Need4Players));
            }
            else if (room.GetSide(Target) != UserSide && room.voteKick.running && startVote)
            {
                usr.send(new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.VoteKickInProgress));
            }
            else if (Target == null || TargetSlot == usr.roomslot || Target.rank > 2)
            {
                usr.send(new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.InvalidCandidate));
            }
            else if (room.voteKick.lastKickTimestamp > Generic.timestamp)
            {
                usr.send(new Game.SP_Chat("GM", Game.SP_Chat.ChatType.Room_ToAll, "GM >> You have to wait 5 minutes!", 999, "GM"));
            }
            else
            {
                int targetSide = room.GetSide(Target);
                if (targetSide == UserSide)
                {
                    if (!room.voteKick.running)
                    {
                        sendPacket = true;
                        sendBlocks[6] = "1";
                        room.voteKick.StartVote(Target.roomslot, targetSide);
                    }
                    else
                    {
                        if (room.voteKick.votes.Where(r => r.usr.userId == usr.userId).Count() == 0)
                        {
                            room.voteKick.AddUserVotekick(usr, true);
                        }
                    }
                }
                else
                {
                    usr.send(new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.CannotKickOpposingTeam));
                }
            }
        }
    }
    class SP_RoomData_VoteKick : Packet
    {
        internal enum ErrCodes
        {
            NotEnoughPlayers = 96140,   // Not enough players. Cannot start game.
            VoteKickInProgress = 96150, // Vote kick is currently in progress.
            CannotKickError,            // Currently, you cannot vote kick.
            InvalidCandidate,            // Invalid vote kick candidate.
            CannotKickOpposingTeam,     // Cannot vote kick opposing team members.
            Need4Players,               // You need 4 or more players.
            CannotKickError2            // Currently, you cannot vote kick.
        }
        public SP_RoomData_VoteKick(int Target, bool KickedOut, int roomId)
        {
            newPacket(30000);
            addBlock(1);
            addBlock(-1);
            addBlock(roomId);
            addBlock(2);
            addBlock((int)Subtype.VoteKick);
            addBlock(KickedOut ? 1 : 0);
            addBlock(KickedOut ? 0 : 1);
            addBlock(2);
            addBlock(Target);
            addBlock(KickedOut ? 1 : 0);
            addBlock(KickedOut ? 75 : 0);
            Fill(0, 4);
        }

        public SP_RoomData_VoteKick(ErrCodes code)
        {
            newPacket(30000);
            addBlock((int)code);
        }
    }
}