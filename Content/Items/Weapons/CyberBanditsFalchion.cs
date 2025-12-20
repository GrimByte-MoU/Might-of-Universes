using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CyberBanditsFalchion : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Cutlass);

            Item.damage = 200;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
            Item.scale = 2f;
            Item.maxStack = 1;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CodeDestabilized>(), 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cutlass)
                .AddIngredient(ModContent.ItemType<SyntheticumBar>(), 10)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}