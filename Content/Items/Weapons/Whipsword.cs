using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Whipsword : ModItem
    {
        public int Mode;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 55;
            Item.knockBack = 5.5f;
            Item.crit = 4;
            Item.value = Item.buyPrice(gold: 6);
            Item.rare = ItemRarityID.Pink;
            Item.maxStack = 1;
            ConfigureAsSword();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Mode = 1 - Mode;

                if (Main.myPlayer == player.whoAmI)
                {
                    CombatText.NewText(player.getRect(), Mode == 0 ? Color.LightSkyBlue : Color.Gold,
                        Mode == 0 ? "Connected" : "Disconnected");
                }
                return false;
            }

            if (Mode == 0)
                ConfigureAsSword();
            else
                ConfigureAsWhip();

            return base.CanUseItem(player);
        }

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
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.channel = false;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 0f;
            Item.autoReuse = true;
            Item.scale = 1.25f;
        }

        private void ConfigureAsWhip()
        {
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