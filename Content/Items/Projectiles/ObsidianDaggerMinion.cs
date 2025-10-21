using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ObsidianDaggerMinion : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0.5f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.netImportant = true;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.HasBuff(ModContent.BuffType<ObsidianDaggerBuff>()))
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            // Unique index across this player's daggers
            int totalDaggers = 0;
            int myIndex = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == Projectile.type)
                {
                    if (proj.whoAmI == Projectile.whoAmI)
                        myIndex = totalDaggers;
                    totalDaggers++;
                }
            }

            // Semicircle above player (-PI..0 maps to upper half; single dagger sits straight above)
            float formationRadius = 56f + 10f * Math.Max(0, totalDaggers - 1);
            float angle = (totalDaggers <= 1)
                ? -MathHelper.PiOver2
                : (-MathHelper.Pi + (MathHelper.Pi / (totalDaggers + 1)) * (myIndex + 1));

            Vector2 targetPos = player.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * formationRadius;

            NPC target = FindTarget(player, 800f);
            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;

                float swipeDist = 32f;
                float swipeSpeed = 0.4f;
                float swipeAngle = (float)Math.Sin(Main.GameUpdateCount * swipeSpeed + Projectile.identity) * swipeDist;

                Vector2 swipeOffset = new Vector2(-toTarget.Y, toTarget.X);
                if (swipeOffset != Vector2.Zero)
                    swipeOffset.Normalize();
                swipeOffset *= swipeAngle;

                Vector2 attackPos = target.Center + swipeOffset;
                Projectile.velocity = (attackPos - Projectile.Center) * 0.20f;
                //Projectile.rotation = toTarget.ToRotation();
            }
            else
            {
                Projectile.velocity = (targetPos - Projectile.Center) * 0.20f;
                //Projectile.rotation = (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2;
            }

            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Obsidian, Projectile.velocity.X, Projectile.velocity.Y);
            }
        }

        private NPC FindTarget(Player player, float range)
        {
            NPC chosen = null;
            float best = range * range;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this))
                {
                    float d2 = Vector2.DistanceSquared(player.Center, npc.Center);
                    if (d2 < best)
                    {
                        best = d2;
                        chosen = npc;
                    }
                }
            }
            return chosen;
        }
    }
}