using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class TempleReaverBoots : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.06f;
            player.GetCritChance(reaper) += 6f;

            player.statLifeMax2 += 15;
            player.endurance += 0.04f;
            
            player.moveSpeed += 0.16f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LihzahrdBrick, 75)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}