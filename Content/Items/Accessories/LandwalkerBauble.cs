using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LandwalkerBauble : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 4;
            player.lifeRegen += 2; // +1 health per second (2 = 1 hp/sec)
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            
            // All damage types +5%
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.2f;
            player.endurance += 0.05f; // Damage reduction
            player.moveSpeed += 0.10f;
            player.maxMinions += 1;
            
            // Movement improvements
            player.jumpSpeedBoost += 0.15f;
            player.jumpBoost = true;
            player.extraFall += 15;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddIngredient(ItemID.IceBlock, 20)
                .AddIngredient(ItemID.DesertFossil, 20)
                .AddIngredient(ItemID.Coral, 20)
                .AddIngredient(ItemID.Hive, 20)
                .AddIngredient(ItemID.Cloud, 20)
                .AddIngredient(ItemID.StoneBlock, 20)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}