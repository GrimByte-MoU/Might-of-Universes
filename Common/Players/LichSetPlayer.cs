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
        private bool wearingLichSet;
        private int spawnTimer;

        public override void ResetEffects()
        {
            wearingLichSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullLichSet())
            {
                wearingLichSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += 125;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingLichSet)
            {
                Player.setBonus = "+125 max souls\nTwo accursed souls spawn near you every 2 seconds. They home in on nearby enemies and steal 2 souls on hit.";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingLichSet)
            {
                spawnTimer = 0;
                return;
            }

            if (spawnTimer > 0) spawnTimer--;
            if (spawnTimer > 0) return;

            spawnTimer = SpawnIntervalTicks;

            for (int i = 0; i < SpawnCount; i++)
            {
                float r = (float)(Main.rand.NextDouble() * SpawnRadiusTiles * 16.0);
                float ang = MathHelper.ToRadians(Main.rand.NextFloat() * 360f);
                Vector2 spawnPos = Player.Center + new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)) * r;
                Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<AccursedSoul>(), 45, 0f, Player.whoAmI);
            }

            SoundEngine.PlaySound(SoundID.Item8, Player.position);
        }

        private bool IsWearingFullLichSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.LichMask>()
                && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.LichChestplate>()
                && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.LichGreaves>();
        }
    }
}