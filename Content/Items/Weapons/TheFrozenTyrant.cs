using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TheFrozenTyrant : ModItem
    {
        private int currentMode = 0;

        public override void SetDefaults()
        {
            Item.damage = 175;
            Item.DamageType = DamageClass.Magic;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item28;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FrozenTyrantSphere>();
            Item.shootSpeed = 18f;
            Item.noMelee = true;
            Item.mana = 20;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                currentMode = (currentMode + 1) % 2;
                SoundEngine.PlaySound(SoundID.Item37, player.position);
                
                string modeName = currentMode == 0 ? "Subjugate" : "Punish";
                CombatText.NewText(player.getRect(), Color.Cyan, modeName, true);
                
                return false;
            }

            if (currentMode == 0)
            {
                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.mana = 12;
                Item.shoot = ModContent.ProjectileType<FrozenTyrantSphere>();
                Item.shootSpeed = 18f;
                Item.UseSound = SoundID.Item28;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = false;
            }
            else
            {
                Item.useTime = 3;
                Item.useAnimation = 3;
                Item.mana = 3;
                Item.shoot = ModContent.ProjectileType<FrozenTyrantShard>();
                Item.shootSpeed = 16f;
                Item.UseSound = SoundID.Item30;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = false;
            }

            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
        {
            if (currentMode == 0)
            {
                damage = (int)(damage * 1.5f);
            }
            else if (currentMode == 1)
            {
                damage = (int)(damage * 0.75f);
                
                Vector2 targetPosition = Main.MouseWorld;
                Vector2 spawnPosition = targetPosition + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);
                position = spawnPosition;
                
                Vector2 directionToTarget = (targetPosition - spawnPosition).SafeNormalize(Vector2.UnitY);
                velocity = directionToTarget * Item.shootSpeed;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FrostAdmiralsPoint>()
                .AddIngredient(ItemID.LunarBar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}