using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using MightofUniverses.Content.Items.Projectiles;
// If DemonicEssence is in a different namespace, update this using:
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Whipsword : ModItem
    {
        // 0 = Sword stance, 1 = Whip stance
        public int Mode;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.damage = 55; // early/mid-hardmode baseline
            Item.knockBack = 5.5f;
            Item.crit = 4;
            Item.value = Item.buyPrice(gold: 6);
            Item.rare = ItemRarityID.Pink;
            Item.maxStack = 1;

            // Default to sword stance on spawn; CanUseItem will swap settings dynamically
            ConfigureAsSword();
        }

        public override bool AltFunctionUse(Player player) => true; // enable right-click

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Toggle mode on right-click without using the item
                Mode = 1 - Mode;

                if (Main.myPlayer == player.whoAmI)
                {
                    CombatText.NewText(player.getRect(), Mode == 0 ? Color.LightSkyBlue : Color.Gold,
                        Mode == 0 ? "Connected" : "Disconnected");
                }
                return false;
            }

            // Apply mode-specific properties for the attack
            if (Mode == 0)
                ConfigureAsSword();
            else
                ConfigureAsWhip();

            return base.CanUseItem(player);
        }

        // Make the sword stance 150% damage while keeping it a summoner weapon
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (Mode == 0)
            {
                damage *= 1.5f;
            }
        }

        private void ConfigureAsSword()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = false;          // item hitbox deals damage
            Item.noUseGraphic = false;     // show the sword
            Item.channel = false;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 0f;
            Item.autoReuse = true;
            Item.scale = 1.25f;
        }

        private void ConfigureAsWhip()
        {
            // True whip (summon melee speed scaling)
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;            
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<WhipswordWhipProj>();
            Item.shootSpeed = 4f;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 50)
                .AddIngredient(ModContent.ItemType<DemonicEssence>(), 15)
                .AddIngredient(ItemID.Rope, 50)
                .AddIngredient(ItemID.SoulofFright, 9)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Mode"] = Mode;
        }

        public override void LoadData(TagCompound tag)
        {
            Mode = tag.GetInt("Mode");
        }
    }
}