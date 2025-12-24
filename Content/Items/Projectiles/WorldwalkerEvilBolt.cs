using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WorldwalkerEvilBolt : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 12;
            Projectile.timeLeft = 300; // 5 seconds
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
            Main.rand.NextBool() ? DustID.CursedTorch : DustID.IchorTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            Projectile.gfxOffY = -16f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
            target.AddBuff(BuffID.Ichor, 300);
            target.AddBuff(ModContent.BuffType<DeadlyCorrupt>(), 180);
            Player owner = Main.player[Projectile.owner];
    int heal = damageDone / 25;
    owner.statLife = Math.Min(owner. statLife + heal, owner.statLifeMax2);
    owner.HealEffect(heal);

            for (int i = 0; i < 3; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                    target.Center, velocity,
                    ProjectileID.NightsEdge,
                    Projectile.damage / 2,
                    2f, Projectile.owner);
            }
        }
    }
}
