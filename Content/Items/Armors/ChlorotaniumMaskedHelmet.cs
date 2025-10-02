using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorotaniumMaskedHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.defense = 13;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 9);
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;
            player.statLifeMax2 += 5;


            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.SoulCostMultiplier *= 0.90f;

            if (player.HeldItem?.type == ModContent.ItemType<ChlorotaniumScythe>())
            {
                acc.SoulCostMultiplier *= 0.9444444f;
            }
        }

        public override void AddRecipes()
        {
            // Titanium variant
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddIngredient(ItemID.TitaniumBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            // Adamantite variant
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddIngredient(ItemID.AdamantiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}