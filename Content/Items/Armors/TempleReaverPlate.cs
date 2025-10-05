using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class TempleReaverPlate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 12);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.07f;
            player.GetCritChance(reaper) += 7f;

            player.statLifeMax2 += 30;
            player.endurance += 0.04f;
            

            // +3 souls/sec while equipped
            player.GetModPlayer<TempleReaverPlayer>().ChestSoulGenActive = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LihzahrdBrick, 125)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}