using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Armors;

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

            // Simulated wing flight
            Player.wingTimeMax = 240;
            Player.wingTime = 240;
            Player.equippedWings = new Item();
            Player.equippedWings.SetDefaults(ModContent.ItemType<ClockworkPirateWings>());

            // Spawn orbiting gears
            float gearDistance = 240f; // 15 tiles
            for (int i = 0; i < 3; i++)
            {
                bool gearExists = false;
                for (int j = 0; j < Main.maxProjectiles; j++)
                {
                    Projectile proj = Main.projectile[j];
                    if (proj.active && proj.type == ModContent.ProjectileType<ClockworkGear>() && proj.owner == Player.whoAmI && proj.ai[0] == i)
                    {
                        gearExists = true;
                        break;
                    }
                }

                if (!gearExists)
                {
                    Vector2 spawnPos = Player.Center + new Vector2(0, -gearDistance).RotatedBy(MathHelper.ToRadians(120 * i));
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<ClockworkGear>(), 0, 0f, Player.whoAmI, i);
                }
            }

            // Flame effect on feet when flying
            bool isFlying = !Player.mount.Active && Player.velocity.Y != 0f && Player.wingTime > 0;

            if (isFlying)
            {
                Vector2 footPos = Player.Bottom + new Vector2(0f, -4f);
                int dustType = DustID.Torch;

                Dust dust = Dust.NewDustDirect(
                    footPos,
                    Width: 0,
                    Height: 0,
                    Type: dustType,
                    SpeedX: 0f,
                    SpeedY: -1.5f,
                    Alpha: 100,
                    newColor: default,
                    Scale: 1f
                );

                dust.noGravity = true;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return Main.rand.NextFloat() >= ammoSaveChance;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasClockworkSet)
            {
                modifiers.DefenseEffectiveness *= 1f + (defensePenFlat / 100f);
                if (target.boss)
                    modifiers.SourceDamage *= 1.15f;
            }
        }
    }
}

