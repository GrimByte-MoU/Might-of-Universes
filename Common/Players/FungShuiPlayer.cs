using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class FungShuiPlayer : ModPlayer
    {
        public bool fungShuiEquipped;
        private int trapTimer;

        public override void ResetEffects()
        {
            fungShuiEquipped = false;
        }

        public override void PostUpdate()
        {
            if (fungShuiEquipped)
            {
                trapTimer++;
                if (trapTimer >= 60)
                {
                    trapTimer = 0;
                    Vector2 spawnPos1 = Player.Center + new Vector2(Main.rand.NextFloat(-240, 240), Main.rand.NextFloat(-240, 240));
                    Vector2 spawnPos2 = Player.Center + new Vector2(Main.rand.NextFloat(-240, 240), Main.rand.NextFloat(-240, 240));

                    if (Vector2.Distance(spawnPos1, spawnPos2) < 32f)
                        spawnPos2 += new Vector2(32, 0);

                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), spawnPos1, Vector2.Zero, ModContent.ProjectileType<FungShuiTrap1>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), spawnPos2, Vector2.Zero, ModContent.ProjectileType<FungShuiTrap2>(), 0, 0, Player.whoAmI);
                }
            }
        }
    }
}
