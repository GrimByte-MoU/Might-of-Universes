using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ClockworkPirateVest : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.moveSpeed += 0.10f;
            player.statLifeMax2 += 40;
            player.endurance += 0.02f;
            player.GetModPlayer<ClockworkPlayer>().ammoSaveChance += 0.20f;
            player.GetModPlayer<ClockworkPlayer>().defensePenFlat += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 20)
                .AddIngredient(ItemID.Cog, 50)
                .AddIngredient(ItemID.Wire, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
