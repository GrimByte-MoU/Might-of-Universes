using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses. Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses. Content.Items.Accessories
{
    public class EclipseCrystalPendant : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item. width = 22;
            Item.height = 26;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<ReaperPlayer>();
            int soulEnergy = (int)modPlayer.soulEnergy;

            player.GetDamage<ReaperDamageClass>() += 0.00075f * soulEnergy;

            if (Main.eclipse)
            {
                player.lifeRegen += 4;
                modPlayer.soulEnergy += 4f / 60f;
            }
            else if (Main. dayTime)
            {
                player.lifeRegen += 4;
            }
            else
            {
                modPlayer. soulEnergy += 4f / 60f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID. CrystalShard, 1)
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}