using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WorldwalkersCharm : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 8;
            player.lifeRegen += 4; // +1 health per second (2 = 1 hp/sec)
            player.statLifeMax2 += 35;
            player.statManaMax2 += 35;
            
            // All damage types +10%
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            
            player.endurance += 0.08f; // Damage reduction
            player.moveSpeed += 0.25f;
            player.maxMinions += 1;
            
            // Movement improvements
            player.jumpSpeedBoost += 0.3f;
            player.jumpBoost = true;
            player.extraFall += 30;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<LandwalkersCharm>())
                .AddIngredient(ItemID.SpectreBar, 10)
                .AddIngredient(ItemID.ShroomiteBar, 10)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ModContent.ItemType<ViralCode>(), 5)
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 5)
                .AddIngredient(ModContent.ItemType<GreedySpirit>(), 5)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 5)
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 5)
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 5)
                .AddIngredient(ModContent.ItemType<PureTerror>(), 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}