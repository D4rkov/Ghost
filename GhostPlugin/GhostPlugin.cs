﻿using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;

namespace Ghost
{
    [ApiVersion(2, 1)]
    public class Ghost : TerrariaPlugin
    {
        public override string Name => "Ghost";

        public override string Author => "Darkov";

        public override string Description => "A plugin that allows admins to become completely invisible to players.";

        public override Version Version => new Version(1, 2, 1, 0);

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("ghost.ghost", OnGhost, "ghost", "vanish"));
        }
        void OnGhost(CommandArgs args)
        {
            int i = Projectile.NewProjectile(Projectile.GetNoneSource(), args.Player.LastNetPosition.X, args.Player.LastNetPosition.Y, Vector2.Zero.X, Vector2.Zero.Y, 0, 0, 0, 16, 0, 0);
            Main.projectile[i].timeLeft = 0;
            NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, i);
            args.TPlayer.active = !args.TPlayer.active;
            NetMessage.SendData((int)PacketTypes.PlayerActive, -1, args.Player.Index, null, args.Player.Index, args.TPlayer.active.GetHashCode());
            if (args.TPlayer.active)
            {
                NetMessage.SendData((int)PacketTypes.PlayerInfo, -1, args.Player.Index, null, args.Player.Index);
                NetMessage.SendData((int)PacketTypes.PlayerUpdate, -1, args.Player.Index, null, args.Player.Index);
            }
            args.Player.SendSuccessMessage("{0}abled Ghost.", args.TPlayer.active ? "Dis" : "En");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

        public Ghost(Main game)
            : base(game)
        {
            Order = 10;
        }
    }
}