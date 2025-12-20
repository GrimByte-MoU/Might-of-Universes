using Microsoft.Xna. Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CoreOfFlesh : ModItem
    {
        private int currentMode = 1;
        private int laserBurstCount = 0;
        private int laserBurstCooldown = 0;
        private int sickleCooldown = 0;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 35;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 4f;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2);
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<FleshGlob>();
            Item.shootSpeed = 14f;
            Item.maxStack = 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                CycleMode(player);
                return false;
            }

            var cfPlayer = player.GetModPlayer<CoreOfFleshPlayer>();
            if (cfPlayer.ForceSwitchToLaser)
            {
                currentMode = 2;
                cfPlayer.ForceSwitchToLaser = false;
                Main.NewText("Out of souls! Switched to Laser Beam.", 255, 200, 100);
            }

            if (currentMode == 3)
            {
                var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
                if (reaperPlayer.soulEnergy < 30)
                {
                    Main. NewText("Not enough souls for Sickle mode!", 255, 100, 100);
                    return false;
                }

                if (sickleCooldown > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void CycleMode(Player player)
        {
            currentMode++;
            if (currentMode > 3) currentMode = 1;

            string modeName = currentMode switch
            {
                1 => "Flesh Glob",
                2 => "Laser Beam",
                3 => "Sickle of Demons",
                _ => "Unknown"
            };

            Main.NewText($"Switched to: {modeName}", 200, 255, 200);
            SoundEngine.PlaySound(SoundID.MenuTick, player. Center);

            laserBurstCount = 0;
            laserBurstCooldown = 0;
            sickleCooldown = 0;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override void HoldItem(Player player)
        {
            var auraPlayer = player.GetModPlayer<LanternAuraPlayer>();
            auraPlayer.HasGuidingLightSource = true;
            auraPlayer.BaseAuraRadiusTiles = 12f;

            Lighting.AddLight(player.Center, 0.9f, 0.8f, 0.4f);

            if (laserBurstCooldown > 0)
            {
                laserBurstCooldown--;
            }

            if (sickleCooldown > 0)
            {
                sickleCooldown--;
            }

            if (currentMode != 2)
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 targetDirection = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

            switch (currentMode)
            {
                case 1:
                    ShootFleshGlobs(player, source, position, targetDirection, damage, knockback);
                    break;

                case 2:
                    ShootLaserBurst(player, source, position, targetDirection, damage, knockback);
                    break;

                case 3:
                    ShootSickle(player, source, position, targetDirection, damage, knockback);
                    break;
            }

            return false;
        }

        private void ShootFleshGlobs(Player player, IEntitySource source, Vector2 position, Vector2 direction, int damage, float knockback)
        {
            float spreadAngle = MathHelper.ToRadians(10);
            int fleshDamage = (int)(damage * 0.75f);

            for (int i = 0; i < 2; i++)
            {
                float angle = (i - 0.5f) * spreadAngle;
                Vector2 velocity = direction.RotatedBy(angle) * 14f;

                Projectile.NewProjectile(source, position, velocity,
                    ModContent.ProjectileType<FleshGlob>(), fleshDamage, knockback, player.whoAmI);
            }

            SoundEngine.PlaySound(SoundID.Item17, position);
        }

        private void ShootLaserBurst(Player player, IEntitySource source, Vector2 position, Vector2 direction, int damage, float knockback)
        {
            if (laserBurstCooldown > 0)
                return;

            if (laserBurstCount < 3)
            {
                float spreadAngle = MathHelper. ToRadians(5);
                float angle = (laserBurstCount - 1) * spreadAngle;
                Vector2 velocity = direction.RotatedBy(angle) * 20f;
                int laserDamage = (int)(damage * 0.8f);

                Projectile.NewProjectile(source, position, velocity,
                    ModContent.ProjectileType<YellowLaser>(), laserDamage, knockback * 2f, player.whoAmI);

                laserBurstCount++;
                Item.useTime = 3;

                SoundEngine.PlaySound(SoundID.Item12, position);
            }

            if (laserBurstCount >= 3)
            {
                laserBurstCount = 0;
                laserBurstCooldown = 12;
                Item.useTime = 20;
            }
        }

        private void ShootSickle(Player player, IEntitySource source, Vector2 position, Vector2 direction, int damage, float knockback)
        {
            int sickleDamage = (int)(damage * 2.5f);
            float storedAngle = direction.ToRotation();

            Projectile.NewProjectile(source, position, Vector2.Zero,
                ModContent.ProjectileType<DemonicSickle>(), sickleDamage, knockback, player.whoAmI, 0f, storedAngle);

            SoundEngine.PlaySound(SoundID.Item71, position);

            sickleCooldown = 15;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Mode", $"[c/FFD700:Current Mode:  {GetModeName()}]"));
            tooltips.Add(new TooltipLine(Mod, "ModeSwitch", "[c/808080:Right-click to cycle modes]"));
            tooltips.Add(new TooltipLine(Mod, "FleshGlob", "[c/FF6B6B:Flesh Glob:] 2 globs, +2 souls per hit, 75% damage"));
            tooltips.Add(new TooltipLine(Mod, "Laser", "[c/FFFF00:Laser Beam:] 3-shot burst, 80% damage"));
            tooltips.Add(new TooltipLine(Mod, "Sickle", "[c/9B59B6:Sickle of Demons:] 250% damage, -30 souls/sickle, ignores 50 defense"));
        }

        private string GetModeName()
        {
            return currentMode switch
            {
                1 => "Flesh Glob",
                2 => "Laser Beam",
                3 => "Sickle of Demons",
                _ => "Unknown"
            };
        }
    }
}