using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Audio;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Content.Items.Weapons
{
    public class WorldwalkerSword : ModItem
    {
        // Modes: 0 Evil, 1 Snow, 2 Jungle (multi stinger), 3 Underworld, 4 Purity
        private int mode;

        private static readonly string[] ModeNames =
        {
            "Evil Spear",
            "Chilly Blade",
            "Jungle Swarm",
            "Infernal Wave",
            "Forest's Fist"
        };

        // Visual theme colors (match your description):
        private static readonly Color[] ModeColors =
        {
            new Color(160, 40, 170),  // Evil: purple/red blend
            new Color(185, 235, 255), // Chilly: icy blue/white
            new Color(40, 105, 40),   // Jungle: dull deep green
            new Color(255, 150, 45),  // Infernal: orange/yellow
            new Color(95, 75, 45)     // Forest: greenish brown
        };

        public override void SetDefaults()
        {
            Item.damage = 185;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.25f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            // Let right-click still play swing anim; you can block if undesired.
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                CycleMode(player);
                return true;
            }
            return base.UseItem(player);
        }

        private void CycleMode(Player player)
        {
            mode = (mode + 1) % ModeNames.Length;
            if (Main.myPlayer == player.whoAmI)
            {
                var c = ModeColors[mode];
                Main.NewText($"{ModeNames[mode]}", c.R, c.G, c.B);
                SoundEngine.PlaySound(SoundID.MenuTick with { Pitch = 0.3f }, player.Center);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Suppress projectile on right-click (mode switch)
            if (player.altFunctionUse == 2)
                return false;

            // Adjust spawn to front of player (avoid spawning inside player)
            position = player.MountedCenter;
            velocity = velocity.SafeNormalize(Vector2.UnitX) * Item.shootSpeed;

            int projType = 0;
            int projDamage = damage;
            float projKB = knockback;

            switch (mode)
            {
                case 0: // Evil Spear
                    projType = ModContent.ProjectileType<WorldwalkerEvilBolt>();
                    break;
                case 1: // Chilly Blade
                    projType = ModContent.ProjectileType<WorldwalkerChilly>();
                    break;
                case 2: // Jungle Swarm (multi stinger, half damage each)
                    projType = ModContent.ProjectileType<WorldwalkerStinger>();
                    projDamage = (int)(damage * 0.5f);
                    break;
                case 3: // Infernal Wave
                    projType = ModContent.ProjectileType<WorldwalkerFireWave>();
                    break;
                case 4: // Forest's Fist (Purity Boulder previously)
                    projType = ModContent.ProjectileType<WorldwalkerPurityBoulder>();
                    break;
                default:
                    return false;
            }

            if (projType == 0)
                return false;

            if (projType == ModContent.ProjectileType<WorldwalkerStinger>())
            {
                int count = Main.rand.Next(5, 8);
                for (int i = 0; i < count; i++)
                {
                    Vector2 perturbed = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                    perturbed *= Main.rand.NextFloat(0.4f, 1f);
                    Projectile.NewProjectile(source, position, perturbed, projType, projDamage, projKB, player.whoAmI);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, projType, projDamage, projKB, player.whoAmI);
            }

            // We manually spawned projectiles, so return false.
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (Main.rand.NextBool(6))
            {
                var c = ModeColors[mode];
                int d = Dust.NewDust(player.Center - new Vector2(4, 4), 8, 8, DustID.Dirt,
                    player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, c, 0.9f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Show current mode & recolor item name
            var nameLine = tooltips.FirstOrDefault(t => t.Mod == "Terraria" && t.Name == "ItemName");
            if (nameLine != null)
            {
                nameLine.OverrideColor = ModeColors[mode];
            }
            tooltips.Add(new TooltipLine(Mod, "WorldwalkerMode", $"Mode: {ModeNames[mode]} (Right-click to cycle)"));
        }

        // Persist the mode
        public override void SaveData(TagCompound tag)
        {
            tag["WWMode"] = mode;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("WWMode"))
                mode = tag.GetInt("WWMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(mode);
        }

        public override void NetReceive(BinaryReader reader)
        {
            mode = reader.ReadInt32();
        }
    }
}