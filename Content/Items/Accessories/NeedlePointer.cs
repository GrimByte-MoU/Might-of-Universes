using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Accessories
{
    public class NeedlePointer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NeedlePointerPlayer>().hasNeedlePointer = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Stinger, 5)
                .AddIngredient(ItemID.Vine, 3)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class NeedlePointerPlayer : ModPlayer
    {
        public bool hasNeedlePointer;
        private int needleTimer;

        public override void ResetEffects()
        {
            hasNeedlePointer = false;
        }

        public override void PostUpdate()
        {
            if (!hasNeedlePointer)
                return;

            needleTimer++;
            if (needleTimer >= 90)
            {
                needleTimer = 0;
                if (Main.myPlayer == Player.whoAmI)
                {
                    Vector2 direction = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);
                    // Replace Player.EquippedItem with Player.inventory[Player.selectedItem]
                Projectile.NewProjectile(
                Player.GetSource_Accessory(Player.inventory[Player.selectedItem]),
                Player.Center,
                direction * 8f,
                ModContent.ProjectileType<NeedlePointerProjectile>(),
                10,
                0f,
             Player.whoAmI
);
                }
            }
        }
    }
}
