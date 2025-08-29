using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoblinSigil : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.06f;
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetCritChance(DamageClass.Generic) += 7;
            player.manaRegenBonus += 25;
            player.accCritterGuide = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinTool>())
                .AddIngredient(ItemID.MagicPowerPotion, 5)
                .AddIngredient(ItemID.ManaRegenerationPotion, 5)
                .AddIngredient(ItemID.RagePotion, 5)
                .AddIngredient(ItemID.WrathPotion, 5)
                .AddIngredient(ItemID.TallyCounter)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}