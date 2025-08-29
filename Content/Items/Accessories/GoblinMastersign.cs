using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoblinMastersign : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Goblin Core effects
            player.moveSpeed += 0.15f;
            player.accFlipper = true;
            player.accDepthMeter = 3;
            player.lavaImmune = true;
            player.waterWalk = true;
            player.noFallDmg = true;
            player.pickSpeed -= 0.25f;
            player.accWatch = 3;

            // Goblin Plating effects
            player.statLifeMax2 += (int)(player.statLifeMax * 0.15f);
            player.endurance += 0.08f;
            player.lifeRegen += 3;
            player.accDreamCatcher = true;
            player.thorns += 1f;

            // Goblin Sigil effects
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.GetCritChance(DamageClass.Generic) += 10f;
            player.manaRegenBonus += 25;
            player.accCritterGuide = true;

            // Goblin Tech effects
            player.findTreasure = true;
            player.detectCreature = true;
            player.dangerSense = true;
            player.nightVision = true;
            player.sonarPotion = true;
            Lighting.AddLight(player.Center, 1.2f, 1.2f, 1.2f);
        }

        public override void AddRecipes()
{
    CreateRecipe()
        .AddIngredient(ModContent.ItemType<GoblinCore>())
        .AddIngredient(ModContent.ItemType<GoblinPlating>())
        .AddIngredient(ModContent.ItemType<GoblinSigil>())
        .AddIngredient(ModContent.ItemType<GoblinRadar>())
        .AddIngredient(ModContent.ItemType<BrassBar>(), 25)
        .AddIngredient(ItemID.SteampunkPlatform, 10)
        .AddIngredient(ItemID.Wire, 100)
        .AddTile(TileID.MythrilAnvil)
        .Register();
}

    }
}