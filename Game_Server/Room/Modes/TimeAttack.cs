using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Game_Server.Networking;
using Game_Server.Room_Data;
using Game_Server.Game;

namespace Game_Server.GameModes
{
    class TimeAttack
    {
        ~TimeAttack()
        {
            GC.Collect();
        }
        public int LastTick = 0;
        public bool LastWave = false;
        public bool PreparingStage = false;
        public bool respawnThisWave = false;
        public Room room = null;
        public bool sentStage1, sentStage2, sentStage3 = false;
        public int Stage = 0;
        public int sleepBeforeEverything = 5;
        public int TimeAttackTime = 0;
        public int zombieForStage = 0;
        public bool BreakerKilled = false;
        public int waitForPrepare = -1;
        public int waitBeforeSupplyBoxItemsOut = -1;
        public Stopwatch time;

        public int stage1ZombieCount;

        public void RunTimeAttack()
        {
            stage1ZombieCount = (room.zombiedifficulty > 0 ? 5 : 500);
            if (LastTick != DateTime.Now.Second)
            {
                LastTick = DateTime.Now.Second;
                
                switch(this.Stage)
                {
                    case 0 : zombieForStage = stage1ZombieCount;
                        break;
                }

                if (this.room.SendFirstWave)
                {
                    this.room.FirstWaveSent = true;
                    this.room.send(new SP_ZombieNewStage(this.room, 2));
                    this.room.SendFirstWave = false;
                }
                
                if (this.room.zombieRunning)
                {
                    if (this.waitForPrepare > 0)
                    {
                        waitForPrepare = -1;
                        room.timeattack.PrepareNewStage(4);
                        room.timeattack.Stage++;
                        room.timeattack.sleepBeforeEverything = 5;
                    }
                    
                    if (this.sleepBeforeEverything > 0)
                    {
                        this.sleepBeforeEverything--;
                        return;
                    }
                    
                    switch (this.Stage)
                    {
                        case 0:
                            {
                                this.room.SpawnZombie(0);
                                this.room.SpawnZombie(1);
                                break;
                            }
                    }

                    if (this.room.SleepTime > 0)
                    {
                        this.room.SleepTime--;
                        if (this.room.SleepTime == 0 && (!sentStage1 || !sentStage2 || !sentStage3))
                        {
                            if (!sentStage1) { sentStage1 = true; }
                            else if (!sentStage2) { sentStage2 = true; }
                            else if (!sentStage3) { sentStage3 = true; }
                            this.room.send(new SP_ZombieNewStage(this.room, 2));
                        }
                        return;
                    }

                    if (this.BreakerKilled)
                    {
                        room.EndGame();
                        return;
                    }

                    if (this.Stage == 0 && !this.sentStage1 && this.room.KilledZombies >= this.zombieForStage)
                    {
                        this.sentStage1 = true;
                        this.PrepareNewStage(3);
                        this.room.SleepTime = 125;
                    }

                    if (this.room.Zombies.Count >= 20) return;
                }
            }
        }

        public int[] chooses = new int[4] { -1, -1, -1, -1 };
        
        public void Update()
        {
            if (this.waitBeforeSupplyBoxItemsOut > 0)
            {
                this.waitBeforeSupplyBoxItemsOut--;
                if (this.waitBeforeSupplyBoxItemsOut <= 0)
                {
                    int index = Array.IndexOf(chooses, "-1");
                    if (index > 0)
                    {
                        this.waitBeforeSupplyBoxItemsOut = -1;

                        foreach (User u in room.users.Values)
                        {
                            if (u.timeattackBoxChoose <= -1)
                            {
                                u.timeattackBoxChoose = chooses.Where(r => r == -1).FirstOrDefault();
                                chooses[u.timeattackBoxChoose] = u.userId;
                            }
                        }

                        room.send(new SP_Unknown(30053, 6, 4, 0, 0, "DA09", 1));
                    }
                }
            }
            RunTimeAttack();
            if (this.room.timeleft <= 0) { this.room.EndGame(); return; }
            var alive = (room.users.Values.Where(r => r.Health > 0 || r.timeAttackSpawns >= 0)).Count();
            if(alive == 0) room.EndGame();  
        }

        public void SendNewStage()
        {
            room.spawnedMadmans = room.spawnedManiacs = room.spawnedGrinders = room.spawnedGrounders = room.spawnedHeavys = room.spawnedGrowlers = room.spawnedLovers = room.spawnedHandgemans = room.spawnedChariots = room.spawnedCrushers = 0;
            this.room.KilledZombies = this.room.ZombieSpawnPlace = this.room.KillsBeforeDrop = 0;
            this.room.RespawnAllVehicles();
            this.waitForPrepare = 120;
            this.Stage++;

            chooses = new int[4] { -1, -1, -1, -1 };

            int p = 2;
            switch (this.Stage)
            {
                case 1: p = 2; this.room.timeleft += 1080000; break;
                case 2: p = 1; this.room.timeleft += 480000; break;
                case 3: p = 4; this.room.timeleft += 360000; break;
            }

            this.room.send(new SP_ZombieNewStage(this.room, p));
        }

        public void PrepareNewStage(int id)
        {
            this.room.send(new SP_ZombieNewStage(this.room, id));
            if (id != 3)
            {
                room.send(new SP_ScoreboardInformations(room));
                time.Reset();
                this.room.SleepTime = 5;
            }
        }

        public TimeAttack(Room room)
        {
            time = new Stopwatch();
            time.Start();
            this.room = room;
            this.Stage = 0;
            this.sleepBeforeEverything = 5;
            this.PreparingStage = this.respawnThisWave = room.zombieRunning = room.SendFirstWave = room.FirstWaveSent = BreakerKilled = false;
            room.SleepTime = 0;
            room.ZombiePoints = room.SpawnedZombieplayers = room.KilledZombies = room.KillsBeforeDrop = room.ZombieSpawnPlace = room.spawnedMadmans = room.spawnedManiacs = room.spawnedGrinders = room.spawnedGrounders = room.spawnedHeavys = room.spawnedGrowlers = room.spawnedLovers = room.spawnedHandgemans = room.spawnedChariots = room.spawnedCrushers = 0;
        }
    }
}
