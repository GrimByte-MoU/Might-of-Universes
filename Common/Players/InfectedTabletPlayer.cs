using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class InfectedTabletPlayer : ModPlayer
    {
        public bool holdingTablet = false;
        private int tabletTimer = 0;
        private bool spawnRed = false;
        private const float radius = 30f * 16f;

        public override void ResetEffects()
        {
            holdingTablet = false;
        }

        public override void PostUpdate()
        {
            if (!holdingTablet)
                return;

            tabletTimer++;
            int interval = Main.rand.Next(6, 31);

            if (tabletTimer >= interval)
            {
                tabletTimer = 0;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.Distance(Player.Center) <= radius)
                    {
                        Vector2 spawnPos = npc.Bottom + new Vector2(Main.rand.NextFloat(-30, 30), Main.rand.NextFloat(40, 80));
                        Vector2 velocity = (npc.Center - spawnPos).SafeNormalize(Vector2.UnitY) * 10f;

                        int projType;
                        int damage;
                        if (Main.rand.NextFloat() < 0.25f)
                        {
                            projType = ModContent.ProjectileType<InfectedTabletWormProjectile>();
                            damage = (int)(Player.GetDamage(DamageClass.Summon).ApplyTo(18) * 1.5f);
                            spawnRed = true;
                        }
                        else
                        {
                            projType = ModContent.ProjectileType<TabletWormProjectile>();
                            damage = (int)Player.GetDamage(DamageClass.Summon).ApplyTo(18);
                            spawnRed = false;
                        }

                        Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, velocity, projType, damage, 0f, Player.whoAmI);
                        break;
                    }
                }
            }

            const int POINTS = 60;
            for (int i = 0; i < POINTS; i++)
            {
                float angle = MathHelper.TwoPi * i / POINTS;
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                Vector2 pos = Player.Center + offset;
                int dustID = Dust.NewDustPerfect(pos, DustID.GreenTorch, Vector2.Zero, 0,
                    spawnRed ? Color.Red : Color.LimeGreen, 0.8f).dustIndex;

                Main.dust[dustID].noGravity = true;
            }
        }
    }
}
