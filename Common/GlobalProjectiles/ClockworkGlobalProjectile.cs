using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using Terraria.ID;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class ClockworkGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool boostedByGear = false;

        public override void AI(Projectile projectile)
        {
            if (projectile.owner != Main.myPlayer || boostedByGear)
                return;

            Player player = Main.player[projectile.owner];
            if (!player.GetModPlayer<ClockworkPlayer>().hasClockworkSet)
                return;

            if (projectile.friendly && projectile.DamageType == DamageClass.Ranged && projectile.damage > 0 && projectile.type != ModContent.ProjectileType<ClockworkHook>())
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile gear = Main.projectile[i];

                    if (gear.active && gear.type == ModContent.ProjectileType<ClockworkGear>() && gear.owner == projectile.owner)
                    {
                        if (projectile.Hitbox.Intersects(gear.Hitbox))
                        {
                            projectile.damage = (int)(projectile.damage * 1.3f);
                            projectile.velocity *= 1.5f;
                            player.GetModPlayer<ClockworkPlayer>().defensePenFlat += 20;

                            if (gear.localAI[0] <= 0f)
                            {
                                NPC target = FindNearestTarget(gear.Center, 800f);
                                if (target != null)
                                {
                                    Vector2 toTarget = (target.Center - gear.Center).SafeNormalize(Vector2.UnitX) * 12f;
                                    Projectile.NewProjectile(
                                        gear.GetSource_FromThis(),
                                        gear.Center,
                                        toTarget,
                                        ModContent.ProjectileType<ClockworkHook>(),
                                        100,
                                        1f,
                                        player.whoAmI
                                    );

                                    gear.localAI[0] = 30f;
                                }
                            }

                            boostedByGear = true;
                            break;
                        }
                    }
                }
            }
        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<ClockworkGear>())
            {
                if (projectile.localAI[0] > 0f)
                    projectile.localAI[0]--;
            }
        }

        private NPC FindNearestTarget(Vector2 center, float range)
        {
            NPC nearest = null;
            float minDist = range;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float dist = Vector2.Distance(center, npc.Center);
                    if (dist < minDist)
                    {
                        nearest = npc;
                        minDist = dist;
                    }
                }
            }

            return nearest;
        }
    }
}
