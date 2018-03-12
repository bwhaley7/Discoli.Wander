using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using OQ.MineBot.GUI.Protocol.Movement.Maps;
using OQ.MineBot.PluginBase.Base.Plugin.Tasks;
using OQ.MineBot.PluginBase.Classes;
using OQ.MineBot.PluginBase;
using OQ.MineBot.PluginBase.Base;
using OQ.MineBot.PluginBase.Base.Plugin;
using OQ.MineBot.PluginBase.Bot;
using OQ.MineBot.PluginBase.Classes.Base;
using OQ.MineBot.PluginBase.Classes.Entity;
using OQ.MineBot.PluginBase.Classes.Physics;
using OQ.MineBot.PluginBase.Movement.Events;
using OQ.MineBot.PluginBase.Movement.Maps;
using OQ.MineBot.Protocols.Classes.Base;

namespace ShieldPlugin
{
    class Wander : ITask
    {
        private static Random rnd = new Random();
        public MapOptions LowDetailOption;
        private int ticks;
        private bool moving;
        private CancelToken stopToken;

        public Wander()
        {
            MapOptions mapOptions = new MapOptions();
            mapOptions.Look = true;
            mapOptions.Quality = (SearchQuality)152;
            this.LowDetailOption = mapOptions;
            this.stopToken = new CancelToken();
            Console.WriteLine("Issuing Constructor Call");
            return;
        }

        public void OnStart()
        {
            player.physicsEngine.onPhysicsPreTick += PhysicsEngine_onPhysicsPreTick;
        }

        public override bool Exec()
        {
            return !status.entity.isDead && !status.eating;
        }

        private void PhysicsEngine_onPhysicsPreTick(IPlayer player)
        {
            Console.WriteLine("PreTick started");
            if (this.stopToken.stopped)
            {
                Console.WriteLine("Issue Method Pointer");
                player.physicsEngine.onPhysicsPreTick += PhysicsEngine_onPhysicsPreTick;
            }
            else
            {
                if (player.status.entity.isDead)
                    return;
                ++this.ticks;
                this.ticks = 0;
                if (player.physicsEngine.path != null && !player.physicsEngine.path.Complete && (player.physicsEngine.path.Valid || this.moving))
                {
                    Console.WriteLine("Returned");
                    return;
                }
                int num1 = Convert.ToInt32(((IEntity)player.status.entity).location.X) + Wander.rnd.Next(-10, 20);
                int int32 = Convert.ToInt32(((IEntity)player.status.entity).location.Y);
                int num2 = Convert.ToInt32(((IEntity)player.status.entity).location.Z) + Wander.rnd.Next(-10, 20);
                Console.WriteLine("Pre Async");
                IAsyncMap location = player.functions.AsyncMoveToLocation((ILocation)new Location(num1, (float)int32, num2), (IStopToken)this.stopToken, this.LowDetailOption);
                Console.WriteLine("Post Async");
                // ISSUE: method pointer
                ((IAreaMap)location).Completed += OnPathReached;
                // ISSUE: method pointer
                ((IAreaMap)location).Cancelled += OnPathFailed;
                Console.WriteLine("Pre Loc Start");
                location.Start();
                Console.WriteLine("Post Loc Start");
                if (((IAreaMap)location).Valid)
                    player.functions.LookAtBlock((ILocation)new Location(num1, (float)int32, num2), false);
                this.moving = true;
                if (((IAreaMap)location).Searched && ((IAreaMap)location).Complete && ((IAreaMap)location).Valid)
                    this.OnPathReached((IAreaMap)location);
            }
        }

        private void OnPathReached(IAreaMap map)
        {
            this.ticks = 0;
            this.moving = false;
        }

        private void OnPathFailed(IAreaMap map, OQ.MineBot.PluginBase.Movement.Geometry.IAreaCuboid cuboid)
        {
            this.ticks = -10;
            this.moving = false;
        }
    }
}
