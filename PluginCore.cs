// Decompiled with JetBrains decompiler
// Type: ShieldPlugin.PluginCore
// Assembly: Discoli.Wander, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B339FF5-D552-43F0-9881-095000766A95
// Assembly location: C:\Users\brade\Downloads\Discoli.Wander.dll

using OQ.MineBot.GUI.Protocol.Movement.Maps;
using OQ.MineBot.PluginBase;
using OQ.MineBot.PluginBase.Base;
using OQ.MineBot.PluginBase.Base.Plugin;
using OQ.MineBot.PluginBase.Classes;
using OQ.MineBot.PluginBase.Classes.Base;
using OQ.MineBot.PluginBase.Classes.Entity;
using OQ.MineBot.PluginBase.Classes.Physics;
using OQ.MineBot.PluginBase.Movement.Events;
using OQ.MineBot.PluginBase.Movement.Maps;
using OQ.MineBot.Protocols.Classes.Base;
using System;

namespace ShieldPlugin
{
    [Plugin(1, "Discoli Wanderer", "Wanders around randomly to make people think bots are real players")]
    public class PluginCore : IStartPlugin
  {
    private static Random rnd = new Random();
    public MapOptions LowDetailOption;
    private int ticks;
    private bool moving;
    private CancelToken stopToken;

    public string GetName()
    {
      return "Wander";
    }

    public string GetDescription()
    {
      return "Aimlessly moves around tricking people into thinking these are real alts!";
    }

    public string GetVersion()
    {
      return "4.20.68";
    }

    public override void OnLoad(int version, int subversion, int buildversion)
    {
    }

    public void OnEnabled()
    {
      this.stopToken.Reset();
    }

    public void OnDisabled()
    {
    }

    public void Stop()
    {
      this.stopToken.Stop();
    }

    public new IPluginSetting[] Setting
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public IPlugin Copy()
    {
      return (IPlugin) this.MemberwiseClone();
    }

    public PluginResponse OnStart(IPlayer player)
    {
      if (!player.settings.loadWorld)
      {
        Console.WriteLine("[GankAura] 'Load world' must be enabled.");
        return new PluginResponse(false, "'Load world' must be enabled.");
      }
            // ISSUE: method pointer
            player.physicsEngine.onPhysicsPreTick += PhysicsEngine_onPhysicsPreTick;
      return new PluginResponse(true, "");
    }

    private void PhysicsEngine_onPhysicsPreTick(IPlayer player)
    {
      if (this.stopToken.stopped)
      {
                // ISSUE: method pointer
                player.physicsEngine.onPhysicsPreTick += PhysicsEngine_onPhysicsPreTick;
      }
      else
      {
        if (player.status.entity.isDead)
          return;
        ++this.ticks;
        this.ticks = 0;
        if (player.physicsEngine.path != null && !player.physicsEngine.path.Complete && (player.physicsEngine.path.Valid || this.moving))
          return;
        int num1 = Convert.ToInt32(((IEntity) player.status.entity).location.X) + PluginCore.rnd.Next(-10, 20);
        int int32 = Convert.ToInt32(((IEntity) player.status.entity).location.Y);
        int num2 = Convert.ToInt32(((IEntity) player.status.entity).location.Z) + PluginCore.rnd.Next(-10, 20);
        IAsyncMap location = player.functions.AsyncMoveToLocation((ILocation) new Location(num1, (float) int32, num2), (IStopToken) this.stopToken, this.LowDetailOption);
                // ISSUE: method pointer
                ((IAreaMap)location).Completed += PluginCore_Completed;
                // ISSUE: method pointer
                ((IAreaMap)location).Cancelled += PluginCore_Cancelled;
        location.Start();
        if (((IAreaMap) location).Valid)
          player.functions.LookAtBlock((ILocation) new Location(num1, (float) int32, num2), false);
        this.moving = true;
        if (((IAreaMap) location).Searched && ((IAreaMap) location).Complete && ((IAreaMap) location).Valid)
          this.OnPathReached();
      }
    }

        private void PluginCore_Cancelled(IAreaMap map, OQ.MineBot.PluginBase.Movement.Geometry.IAreaCuboid cuboid)
        {
            throw new NotImplementedException();
        }

        private void PluginCore_Completed(IAreaMap map)
        {
            throw new NotImplementedException();
        }

        private void OnPathReached()
    {
      this.ticks = 0;
      this.moving = false;
    }

    private void OnPathFailed()
    {
      this.ticks = -10;
      this.moving = false;
    }

    public PluginCore() : base()
    {
      MapOptions mapOptions = new MapOptions();
      mapOptions.Look = true;
      mapOptions.Quality = (SearchQuality) 152;
      this.LowDetailOption = mapOptions;
      this.stopToken = new CancelToken();
            // ISSUE: explicit constructor call
            return;
    }
  }
}
