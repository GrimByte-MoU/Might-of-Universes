using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class LichSetPlayer : ModPlayer
    {
        private const int SpawnIntervalTicks = 120;
        private const int SpawnCount = 2;
        private const float SpawnRadiusTiles = 15f;

        private bool wearing;
        private int spawnTimer;

        public override void ResetEffects()
        {
            wearing = false;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.LichMask>()
             && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.LichChestplate>()
             && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.LichGreaves>())
            {
                wearing = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += 125f;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearing)
                Player.setBonus = "+125 max souls\nTwo accursed souls spawn near you every 2 seconds. They home in on nearby enemies and steal 2 souls on hit.";
        }

        public override void PostUpdate()
        {
            if (!wearing)
            {
                spawnTimer = 0;
                return;
            }

            if (spawnTimer > 0) spawnTimer--;
            if (spawnTimer > 0) return;
            spawnTimer = SpawnIntervalTicks;

            for (int i = 0; i < SpawnCount; i++)
            {
                float r = (float)(Main.rand.NextDouble() * SpawnRadiusTiles * 16f);
                float ang = MathHelper.TwoPi * Main.rand.NextFloat();
                Vector2 spawnPos = Player.Center + new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)) * r;
                Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, Vector2.Zero,
                    ModContent.ProjectileType<AccursedSoul>(), 45, 0f, Player.whoAmI);
            }

            SoundEngine.PlaySound(SoundID.Item8, Player.position);

            Lighting.AddLight(Player.Center, 0f, 0.3f, 0.8f);

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Smoke, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.CursedTorch, 0f, -1f, 100, default, 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.3f;
                dust.velocity.Y = -0.5f;
            }
        }
    }
}