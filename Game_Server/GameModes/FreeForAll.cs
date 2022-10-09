// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.FreeForAll
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;

namespace Game_Server.GameModes
{
    internal class FreeForAll
    {
        private Room room;

        ~FreeForAll()
        {
            GC.Collect();
        }

        public void Update()
        {
            if (this.room == null)
                return;
           
            if (this.room.SpawnLocation < 0 || this.room.SpawnLocation >= 10)
                this.room.SpawnLocation = 0;
            foreach (User user in room.users.Values)
            {
               // user.isSpawned = true;
                user.throwNades = user.throwRockets = 0;

                if (user.rKills > this.room.highestkills)
                    this.room.highestkills = user.rKills;
                
            }
            if (this.room.timeleft > 0 && this.room.highestkills < this.room.ffakillpoints)
                return;
            this.room.EndGame();
        }

        public FreeForAll(Room room)
        {
            this.room = room;
        }
    }
}
