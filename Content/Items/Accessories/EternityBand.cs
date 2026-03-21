using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class EternityBand : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 5;
            player.statLifeMax2 += 50;
            player.lifeRegen += 6;
            player.pStone = true;
            player.GetModPlayer<EternityBandPlayer>().hasEternityBand = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CharmofMyths)
                .AddIngredient(ItemID.Shackle)
                .AddIngredient(ItemID.LifeCrystal, 5)
                .AddIngredient(ItemID.LunarBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class EternityBandPlayer : ModPlayer
    {
        public bool hasEternityBand = false;

        public override void ResetEffects()
        {
            hasEternityBand = false;
        }
        public override void PostUpdateMiscEffects()
        {
            if (!hasEternityBand) return;

            if (Player.potionDelay > 0 && Main.GameUpdateCount % 2 == 0)
            {
                Player.potionDelay--;
            }

            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                int buffType = Player.buffType[i];
                if (buffType > 0 && Main.debuff[buffType] && Player.buffTime[i] > 1)
                {
                    if (Main.GameUpdateCount % 2 == 0)
                    {
                        Player.buffTime[i]--;
                    }
                }
            }
        }
    }
}