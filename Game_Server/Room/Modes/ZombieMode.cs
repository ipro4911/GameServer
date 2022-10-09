// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.ZombieMode
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.GameModes
{
  internal class ZombieMode
  {
    public int Wave = 1;
    private Room room;
    public bool PreparingWave;
    public bool respawnThisWave;
    public bool WeaponsGot;
    public int ZombiePoints;
    public int ZombieToWave;
    public int LastTick;
        internal int id;

        ~ZombieMode()
    {
      GC.Collect();
    }

         /*public void PrepareNewWave()
         {
           this.room.send((Packet) new SP_ZombieNewWave(0));
           this.room.spawnedMadmans = this.room.spawnedManiacs = this.room.spawnedGrinders = this.room.spawnedGrounders = this.room.spawnedHeavys = this.room.spawnedGrowlers = this.room.spawnedLovers = this.room.spawnedHandgemans = this.room.spawnedChariots = this.room.spawnedCrushers = this.room.spawnedBusters = this.room.spawnedCrashers = this.room.spawnedEnvys = this.room.spawnedClaws = this.room.spawnedBombers = this.room.spawnedDefeders = this.room.spawnedBreakers = this.room.spawnedMadSoldiers = this.room.spawnedMadPrisoners = this.room.spawnedSuperHeavYs = this.room.spawnedLadYs = this.room.spawnedMidgets = 0;
           this.room.spawnedZombieToMap1 = this.room.spawnedZombieToMap2 = this.room.spawnedZombieToMap3 = this.room.spawnedZombieToMap4 = 0;
           this.PreparingWave = true;
           this.room.KilledZombies = this.room.ZombieSpawnPlace = this.room.KillsBeforeDrop = this.room.SpawnedZombies = 0;
           this.room.SleepTime = 15;
           this.room.RespawnAllVehicles();
           this.respawnThisWave = false;
           this.room.send((Packet) new SP_ZombieNewWave(this.Wave));
           ++this.Wave;
         }*/
        public void PrepareNewWave()
        {
            this.room.send((Packet)new SP_ZombieNewWave(0));
            this.room.spawnedMadmans = this.room.spawnedManiacs = this.room.spawnedGrinders = this.room.spawnedGrounders = this.room.spawnedHeavys = this.room.spawnedGrowlers = this.room.spawnedLovers = this.room.spawnedHandgemans = this.room.spawnedChariots = this.room.spawnedCrushers = this.room.spawnedBusters = this.room.spawnedCrashers = this.room.spawnedEnvys = this.room.spawnedClaws = this.room.spawnedBombers = this.room.spawnedDefeders = this.room.spawnedBreakers = this.room.spawnedMadSoldiers = this.room.spawnedMadPrisoners = this.room.spawnedSuperHeavYs = this.room.spawnedLadYs = this.room.spawnedMidgets = 0;
            this.room.spawnedZombieToMap1 = this.room.spawnedZombieToMap2 = this.room.spawnedZombieToMap3 = this.room.spawnedZombieToMap4 = 0;
            this.PreparingWave = true;
            this.room.KilledZombies = this.room.ZombieSpawnPlace = this.room.KillsBeforeDrop = this.room.SpawnedZombies = 0;
            this.room.SleepTime = 15;
            this.room.RespawnAllVehicles();
            this.respawnThisWave = false;
            this.room.send((Packet)new SP_ZombieNewWave(this.Wave));
            ++this.Wave;
        }

        public void Zombie()
    {
      try
      {
        if (this.LastTick == DateTime.Now.Second)
          return;
        this.LastTick = DateTime.Now.Second;
        if (this.room.SendFirstWave)
        {
          this.room.FirstWaveSent = true;
          this.room.send((Packet) new SP_ZombieNewWave(0));
          this.room.SendFirstWave = false;
        }
        switch (this.room.mapid)
        {
          case 48:
            this.room.ZombieToMap1 = 0;
            this.room.ZombieToMap2 = 1;
            this.room.ZombieToMap3 = 6;
            this.room.ZombieToMap4 = 13;
            break;
          case 49:
            this.room.ZombieToMap1 = 18;
            this.room.ZombieToMap2 = 17;
            this.room.ZombieToMap3 = 6;
            this.room.ZombieToMap4 = 7;
            break;
          case 51:
            this.room.ZombieToMap1 = 0;
            this.room.ZombieToMap2 = 1;
            this.room.ZombieToMap3 = 12;
            this.room.ZombieToMap4 = 7;
            break;
          default:
            this.room.ZombieToMap1 = 0;
            this.room.ZombieToMap2 = 1;
            this.room.ZombieToMap3 = 6;
            this.room.ZombieToMap4 = 7;
            break;
        }
        if (!this.room.zombieRunning)
          return;
        switch (this.Wave)
        {
          case 1:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 25 : 20;
            break;
          case 2:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 32 : 27;
            break;
          case 3:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 40 : 35;
            break;
          case 4:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 48 : 43;
            break;
          case 5:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 57 : 52;
            break;
          case 6:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 70 : 65;
            break;
          case 7:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 77 : 72;
            break;
          case 8:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 88 : 83;
            break;
          case 9:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 97 : 92;
            break;
          case 10:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 105 : 100;
            break;
          case 11:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 106 : 101;
            break;
          case 12:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 112 : 107;
            break;
          case 13:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 120 : 115;
            break;
          case 14:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 122 : 117;
            break;
          case 15:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 130 : 125;
            break;
          case 16:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 133 : 128;
            break;
          case 17:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 144 : 139;
            break;
          case 18:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 149 : 144;
            break;
          case 19:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 158 : 153;
            break;
          case 20:
            this.ZombieToWave = this.room.zombiedifficulty > 0 ? 174 : 169;
            break;
        }
        if (this.room.AliveDerb == 0)
          this.room.EndGame();
        if (this.room.KilledZombies >= this.ZombieToWave && this.room.Zombies.Where<KeyValuePair<int, Zombie>>((Func<KeyValuePair<int, Zombie>, bool>) (r => r.Value.Health > 0)).Count<KeyValuePair<int, Zombie>>() == 0)
        {
          if (this.Wave >= (this.room.zombiedifficulty > 0 ? 18 : 20))
            this.room.EndGame();
          else
            this.PrepareNewWave();
        }
        else if (this.room.SleepTime >= 0)
        {
          --this.room.SleepTime;
        }
        else
        {
          if (this.PreparingWave)
            this.PreparingWave = false;
          if (this.room.Zombies.Where<KeyValuePair<int, Zombie>>((Func<KeyValuePair<int, Zombie>, bool>) (r => r.Value.Health > 0)).Count<KeyValuePair<int, Zombie>>() >= 20 || this.room.SpawnedZombies >= this.ZombieToWave || this.PreparingWave)
            return;
          switch (this.Wave)
          {
            case 1:
              if (this.room.spawnedZombieToMap1 < 15)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 5)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 2:
              if (this.room.spawnedZombieToMap1 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 7 && this.room.KilledZombies >= 10)
              {
                this.room.SpawnZombie(2);
                break;
              }
              break;
            case 3:
              if (this.room.spawnedZombieToMap1 < 15)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 5 && this.room.KilledZombies >= 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap3 < 3 && this.room.KilledZombies >= 15)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 2 && this.room.KilledZombies >= 20)
              {
                this.room.SpawnZombie(this.room.ZombieToMap4);
                break;
              }
              break;
            case 4:
              if (this.room.spawnedZombieToMap1 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 5 && this.room.KilledZombies >= 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap3 < 3 && this.room.KilledZombies >= 15)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 2 && this.room.KilledZombies >= 20)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 3 && this.room.KilledZombies >= 25)
              {
                this.room.SpawnZombie(5);
                break;
              }
              break;
            case 5:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 10 && this.room.KilledZombies >= 5)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 5 && this.room.KilledZombies >= 10)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 3 && this.room.KilledZombies >= 15)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 2 && this.room.KilledZombies >= 20)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 2 && this.room.KilledZombies >= 30)
              {
                this.room.SpawnZombie(5);
                break;
              }
              break;
            case 6:
              if (this.room.spawnedZombieToMap1 < 15)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrinders < 10 && this.room.KilledZombies >= 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 5 && this.room.KilledZombies >= 15)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 25)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 2 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 3 && this.room.KilledZombies >= 30)
              {
                this.room.SpawnZombie(5);
                break;
              }
              break;
            case 7:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrounders < 6)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrinders < 15 && this.room.KilledZombies >= 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 6 && this.room.KilledZombies >= 15)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 20)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 3 && this.room.KilledZombies >= 25)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 6 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 1 && this.room.KilledZombies >= 35)
              {
                this.room.SpawnZombie(8);
                break;
              }
              break;
            case 8:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 15)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 20)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrowlers < 8 && this.room.KilledZombies >= 20)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 5 && this.room.KilledZombies >= 45)
              {
                this.room.SpawnZombie(5);
                break;
              }
              break;
            case 9:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 15)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 25)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrowlers < 7 && this.room.KilledZombies >= 20)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 40)
              {
                this.room.SpawnZombie(5);
                break;
              }
              break;
            case 10:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 5)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 5 && this.room.KilledZombies >= 25)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 9 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 1 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(8);
              if (this.room.spawnedGrinders < 15 && this.room.KilledZombies > 40)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 10 && this.room.KilledZombies > 40)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap2 < 25 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedZombieToMap1 < 25 && this.room.KilledZombies >= 35)
              {
                this.room.SpawnZombie(this.room.ZombieToMap1);
                break;
              }
              break;
            case 11:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 5)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 25)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5 && this.room.KilledZombies >= 25)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 11 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrinders < 15 && this.room.KilledZombies > 30)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 10 && this.room.KilledZombies > 35)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap1 < 25 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 25 && this.room.KilledZombies >= 35)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 12:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrowlers < 10)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 10 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 10 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 1 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(8);
              if (this.room.spawnedCrushers < 1 && this.room.KilledZombies >= 60)
                this.room.SpawnZombie(9);
              if (this.room.spawnedGrinders < 10 && this.room.KilledZombies > 45)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap1 < 25 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 25 && this.room.KilledZombies >= 40)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 13:
              if (this.room.spawnedZombieToMap1 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 20)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedChariots < 1 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(8);
              if (this.room.spawnedGrowlers < 7)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 6 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 2 && this.room.KilledZombies >= 75)
                this.room.SpawnZombie(8);
              if (this.room.spawnedGrinders < 15 && this.room.KilledZombies > 50)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap2 < 30 && this.room.KilledZombies >= 75)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedZombieToMap1 < 30 && this.room.KilledZombies >= 50)
              {
                this.room.SpawnZombie(this.room.ZombieToMap1);
                break;
              }
              break;
            case 14:
              if (this.room.spawnedZombieToMap1 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 15)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 10)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 5 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 6 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(5);
              if (this.room.spawnedCrushers < 1 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(9);
              if (this.room.spawnedGrinders < 20 && this.room.KilledZombies > 65)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap1 < 30 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 30 && this.room.KilledZombies >= 45)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 15:
              if (this.room.spawnedZombieToMap1 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedZombieToMap3 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedGrowlers < 5)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 15)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 10 && this.room.KilledZombies >= 30)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 45)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrinders < 10 && this.room.KilledZombies > 55)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap2 < 35 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedZombieToMap1 < 30 && this.room.KilledZombies >= 50)
              {
                this.room.SpawnZombie(this.room.ZombieToMap1);
                break;
              }
              break;
            case 16:
              if (this.room.spawnedZombieToMap1 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedZombieToMap3 < 10)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedGrinders < 5)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 10)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 5)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 6 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 15 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 2 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(8);
              if (this.room.spawnedGrinders < 10 && this.room.KilledZombies > 45)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap1 < 35 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 35 && this.room.KilledZombies >= 30)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 17:
              if (this.room.spawnedZombieToMap1 < 30)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 25)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedZombieToMap3 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedGrinders < 10)
                this.room.SpawnZombie(2);
              if (this.room.spawnedChariots < 1)
                this.room.SpawnZombie(8);
              if (this.room.spawnedGrowlers < 6)
                this.room.SpawnZombie(4);
              if (this.room.spawnedGrounders < 10)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 10 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 45)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 2 && this.room.KilledZombies >= 60)
                this.room.SpawnZombie(8);
              if (this.room.spawnedCrushers < 1 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(9);
              if (this.room.spawnedGrinders < 20 && this.room.KilledZombies > 45)
                this.room.SpawnZombie(2);
              if (this.room.spawnedZombieToMap1 < 35 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 35 && this.room.KilledZombies >= 50)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 18:
              foreach (Game_Server.User usr in (IEnumerable<Game_Server.User>) this.room.users.Values)
              {
                if (!this.WeaponsGot)
                {
                  if (this.room.mapid == 51)
                  {
                    Inventory.AddItem(usr, "DA73", 3);
                    usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " EVENT >> [HUN] Túlélted a 18. szintet nyereményed Kris (3 NAP).", 999L, Game_Server.Configs.Server.SystemName));
                    usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " EVENT >> [ENG] You got Kris (3 DAYS) because u survived wave 18!!", 999L, Game_Server.Configs.Server.SystemName));
                  }
                  else if (this.room.mapid == 50)
                  {
                    Inventory.AddItem(usr, "DA19", 3);
                    usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " EVENT >> [HUN] Túlélted a 18. szintet nyereményed Kitchenknife (3 NAP).", 999L, Game_Server.Configs.Server.SystemName));
                    usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " EVENT >> [ENG] You got Kitchenknife (3 DAYS) because u survived wave 18!!", 999L, Game_Server.Configs.Server.SystemName));
                  }
                  this.WeaponsGot = true;
                }
              }
              if (this.room.spawnedZombieToMap1 < 30)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 30)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 15)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 5)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 5)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrounders < 7)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrounders < 10 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 10 && this.room.KilledZombies >= 45)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 45)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 14 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrinders < 20 && this.room.KilledZombies > 45)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 10 && this.room.KilledZombies > 40)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap1 < 35 && this.room.KilledZombies >= 65)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 35 && this.room.KilledZombies >= 60)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 19:
              this.WeaponsGot = false;
              if (this.room.spawnedZombieToMap1 < 30)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 35)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 15)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 8)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 5)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 4)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 6)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrounders < 7)
                this.room.SpawnZombie(3);
              if (this.room.spawnedChariots < 2)
                this.room.SpawnZombie(8);
              if (this.room.spawnedCrushers < 1)
                this.room.SpawnZombie(9);
              if (this.room.spawnedGrounders < 10 && this.room.KilledZombies >= 45)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 10 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 60)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 10 && this.room.KilledZombies >= 55)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrinders < 20 && this.room.KilledZombies > 45)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 10 && this.room.KilledZombies > 60)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap1 < 40 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 40 && this.room.KilledZombies >= 70)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
            case 20:
              if (this.room.spawnedZombieToMap1 < 35)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedChariots < 2)
                this.room.SpawnZombie(8);
              if (this.room.spawnedZombieToMap2 < 35)
                this.room.SpawnZombie(this.room.ZombieToMap2);
              if (this.room.spawnedGrinders < 16)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 8)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap3 < 6)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 4)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 6)
                this.room.SpawnZombie(5);
              if (this.room.spawnedGrounders < 7)
                this.room.SpawnZombie(3);
              if (this.room.spawnedGrounders < 10 && this.room.KilledZombies >= 35)
                this.room.SpawnZombie(3);
              if (this.room.spawnedZombieToMap3 < 13 && this.room.KilledZombies >= 45)
                this.room.SpawnZombie(this.room.ZombieToMap3);
              if (this.room.spawnedZombieToMap4 < 10 && this.room.KilledZombies >= 40)
                this.room.SpawnZombie(this.room.ZombieToMap4);
              if (this.room.spawnedHeavys < 15 && this.room.KilledZombies >= 50)
                this.room.SpawnZombie(5);
              if (this.room.spawnedChariots < 4 && this.room.KilledZombies >= 65)
                this.room.SpawnZombie(8);
              if (this.room.spawnedCrushers < 2 && this.room.KilledZombies >= 60)
                this.room.SpawnZombie(9);
              if (this.room.spawnedGrinders < 20 && this.room.KilledZombies > 70)
                this.room.SpawnZombie(2);
              if (this.room.spawnedGrowlers < 15 && this.room.KilledZombies > 75)
                this.room.SpawnZombie(4);
              if (this.room.spawnedZombieToMap1 < 40 && this.room.KilledZombies >= 75)
                this.room.SpawnZombie(this.room.ZombieToMap1);
              if (this.room.spawnedZombieToMap2 < 40 && this.room.KilledZombies >= 70)
              {
                this.room.SpawnZombie(this.room.ZombieToMap2);
                break;
              }
              break;
          }
          if (this.room.zombiedifficulty != 1)
            return;
          if (new Random().Next(0, 2) == 0)
          {
            if (this.room.spawnedLadYs + this.room.spawnedMidgets >= 5 || this.room.KilledZombies < 10 + this.Wave)
              return;
            this.room.SpawnZombie(21);
          }
          else
          {
            if (this.room.spawnedLadYs + this.room.spawnedMidgets >= 5 || this.room.KilledZombies < 10 + this.Wave)
              return;
            this.room.SpawnZombie(22);
          }
        }
      }
      catch
      {
      }
    }

    public void Update()
    {
      this.Zombie();
    }

    public ZombieMode(Room room)
    {
      this.room = room;
      this.PreparingWave = this.respawnThisWave = room.zombieRunning = room.SendFirstWave = room.FirstWaveSent = false;
      room.SleepTime = 15;
      room.ZombiePoints = room.SpawnedZombieplayers = room.KilledZombies = room.KillsBeforeDrop = room.ZombieSpawnPlace = 0;
      room.spawnedMadmans = room.spawnedManiacs = room.spawnedGrinders = room.spawnedGrounders = room.spawnedHeavys = room.spawnedGrowlers = room.spawnedLovers = room.spawnedHandgemans = room.spawnedChariots = room.spawnedCrushers = room.spawnedBusters = room.spawnedCrashers = room.spawnedEnvys = room.spawnedClaws = room.spawnedBombers = room.spawnedDefeders = room.spawnedBreakers = room.spawnedMadSoldiers = room.spawnedMadPrisoners = room.spawnedSuperHeavYs = room.spawnedLadYs = room.spawnedMidgets = 0;
      room.spawnedZombieToMap1 = room.spawnedZombieToMap2 = room.spawnedZombieToMap3 = room.spawnedZombieToMap4 = 0;
      room.SpawnedZombies = 0;
      room.Zombies.Clear();
      for (int index = 0; index < 28; ++index)
        room.Zombies.Add(index + 4, new Zombie(index + 4, 0, 0, 0));
    }
  }
}
