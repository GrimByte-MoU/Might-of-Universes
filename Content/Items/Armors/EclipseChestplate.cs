using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class EclipseChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.reaperDamageMultiplier += 0.08f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.08f;
            player.GetCritChance(ModContent.GetInstance<ReaperDamageClass>()) += 6f;
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(2f / 60f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SolunarBreastplate>())
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 20)
                .AddIngredient(ItemID.Ectoplasm, 25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}