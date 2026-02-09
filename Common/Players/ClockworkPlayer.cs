using System;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class ClockworkPlayer : ModPlayer
    {
        public bool hasClockworkSet;
        public float ammoSaveChance;
        public int defensePenFlat;

        public override void ResetEffects()
        {
            hasClockworkSet = false;
            ammoSaveChance = 0f;
            defensePenFlat = 0;
        }

        public override void PostUpdate()
        {
            if (!hasClockworkSet)
                return;

            Player.noFallDmg = true;

            Player.wingTimeMax = 240;

            float gearDistance = 60f;
            for (int i = 0; i < 3; i++)
            {
                bool gearExists = false;
                for (int j = 0; j < Main.maxProjectiles; j++)
                {
                    Projectile proj = Main.projectile[j];
                    if (proj.active && proj.type == ModContent.ProjectileType<ClockworkGear>() && proj.owner == Player.whoAmI)
                    {
                        if ((int)proj.ai[0] == i)
                        {
                            gearExists = true;
                            break;
                        }
                    }
                }

                if (!gearExists)
                {
                    Vector2 spawnPos = Player.Center + new Vector2(0, -gearDistance).RotatedBy(MathHelper.ToRadians(120 * i));
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<ClockworkGear>(), 0, 0f, Player.whoAmI, ai0: i);
                }
            }

            bool isAirborneUsingWings = !Player.mount.Active && Player.velocity.Y != 0f && Player.wingTime > 0 && Player.wingTime < Player.wingTimeMax;
            if (isAirborneUsingWings)
            {
                Vector2 footPos = Player.Bottom + new Vector2(0f, -4f);
                int dustType = DustID.Torch;

                Dust dust = Dust.NewDustDirect(
                    footPos,
                    0,
                    0,
                    dustType,
                    0f,
                    -1.5f,
                    100,
                    default,
                    1f
                );

                dust.noGravity = true;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            float clampedChance = MathHelper.Clamp(ammoSaveChance, 0f, 1f);
            return Main.rand.NextFloat() >= clampedChance;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasClockworkSet)
            {
                float penFrac = MathHelper.Clamp(defensePenFlat / 100f, 0f, 1f);
                modifiers.DefenseEffectiveness *= Math.Max(0f, 1f - penFrac);

                if (target.boss)
                {
                    modifiers.SourceDamage *= 1.15f;
                }
            }
        }
    }
}
