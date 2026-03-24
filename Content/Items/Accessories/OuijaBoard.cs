using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class OuijaBoard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerReaperDamage += 0.12f;
            acc.BonusEmpowerAttackSpeed += 0.10f;
            acc.BonusEmpowerCritChance += 10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CrypticMessage>()
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.Lens, 10)
                .AddIngredient(ItemID.AshWood, 25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}