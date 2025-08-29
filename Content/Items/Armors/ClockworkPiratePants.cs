using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class ClockworkPiratePants : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.13f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.moveSpeed += 0.15f;
            player.endurance += 0.02f;
            player.GetModPlayer<ClockworkPlayer>().ammoSaveChance += 0.15f;
            player.GetModPlayer<ClockworkPlayer>().defensePenFlat += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.Cog, 15)
                .AddIngredient(ItemID.Wire, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
