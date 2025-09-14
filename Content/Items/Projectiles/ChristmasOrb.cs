using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using System.Collections.Generic;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ChristmasOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.LimeGreen.ToVector3() * 1.5f);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);

            foreach (NPC npc in Main.npc)
{
    if (npc.active && !npc.friendly && npc.Hitbox.Intersects(Projectile.Hitbox))
    {
        npc.StrikeNPC(new NPC.HitInfo
        {
            Damage = 125,
            Knockback = 0f,
            HitDirection = 0,
            Crit = false
        });

        Projectile.Kill();
    }
}
            Player player = Main.player[Projectile.owner];
            if (player.Hitbox.Intersects(Projectile.Hitbox))
            {
                player.lifeRegen += 50;
                player.GetModPlayer<ReaperPlayer>().soulEnergy += 50f / 60f;
                Projectile.Kill();
            }
        }
    }
}
