using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WorldsBalanceGauntlet : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Cyan;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.2f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
            player.GetKnockback(DamageClass.Melee) += 0.2f;
            player.autoReuseGlove = true;
            player.GetModPlayer<WorldsBalanceGauntletPlayer>().hasWorldsBalanceGauntlet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticGauntlet>())
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
