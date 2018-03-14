
using OQ.MineBot.GUI.Protocol.Movement.Maps;
using OQ.MineBot.PluginBase;
using OQ.MineBot.PluginBase.Base;
using OQ.MineBot.PluginBase.Base.Plugin;
using OQ.MineBot.PluginBase.Bot;
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
    [Plugin(1, "SpiffyAF Wanderer", "A recode of Discolis Wanderer Plugin")]
    public class PluginCore : IStartPlugin
  {

    public string GetVersion()
    {
      return "2.0";
    }

    public override void OnLoad(int version, int subversion, int buildversion)
    {
            //Nuffin
    }

        public override void OnStart()
        {
            RegisterTask(new Wander());
        }

        public override void OnDisable()
        {

        }

    public IPlugin Copy()
    {
      return (IPlugin) this.MemberwiseClone();
    }

        public override PluginResponse OnEnable(IBotSettings botSettings)
        {
            if (!botSettings.loadWorld)
            {
                Console.WriteLine("[GankAura] 'Load world' must be enabled.");
                return new PluginResponse(false, "'Load world' must be enabled.");
            }
            
            return new PluginResponse(true, "");
        }
    }

  }
