using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.Audio;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class PalladiumPlayer : ModPlayer
    {
        public bool hasPalladiumRangedSet;
        public bool hasPalladiumMagicSet;

        private int crystalSpawnTimer;

        public override void ResetEffects()
        {
            hasPalladiumRangedSet = false;
            hasPalladiumMagicSet = false;
        }

        public override void PostUpdate()
        {
            crystalSpawnTimer++;

            if (crystalSpawnTimer >= 120)
            {
                TrySpawnRangedCrystal();
                TryMagicHealPulse();

                crystalSpawnTimer = 0;
            }
        }

        private void TrySpawnRangedCrystal()
        {
            if (!hasPalladiumRangedSet)
                return;

            // Check if one already exists
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<FloatingHeartCrystal>())
                {
                    return;
                }
            }

            // Spawn at a distance from player
            Vector2 offset = new Vector2(Main.rand.NextFloat(200, 300), 0).RotatedByRandom(MathHelper.TwoPi);
            Vector2 spawnPos = Player.Center + offset;

            Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, Vector2.Zero,
                ModContent.ProjectileType<FloatingHeartCrystal>(), 0, 0f, Player.whoAmI);
        }

        private void TryMagicHealPulse()
        {
            if (!hasPalladiumMagicSet || !Main.rand.NextBool(2)) // 50% chance
                return;

            bool healed = false;

            if (Player.statLife < Player.statLifeMax2)
            {
                Player.statLife += 15;
                if (Player.statLife > Player.statLifeMax2)
                    Player.statLife = Player.statLifeMax2;

                Player.HealEffect(15, true);
                healed = true;
            }

            if (Player.statMana < Player.statManaMax2)
            {
                Player.statMana += 40;
                if (Player.statMana > Player.statManaMax2)
                    Player.statMana = Player.statManaMax2;

                CombatText.NewText(Player.Hitbox, new Color(100, 100, 255), "Mana Restored", true);
                CombatText.NewText(Player.Hitbox, new Color(255, 100, 100), "Health Restored", true);
                healed = true;
            }

            if (healed)
            {
                SoundEngine.PlaySound(SoundID.Item4, Player.Center); // Fairy-like healing sound
            }
        }
    }
}

