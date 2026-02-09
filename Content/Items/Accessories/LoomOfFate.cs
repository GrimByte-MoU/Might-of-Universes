using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LoomOfFate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 20);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();

            p.accLoomOfFate = true;
            p.ApplyPassiveSoulToHPScalar(0.65f);
            if (!p.loomOfFateOnCooldown)
            {
                float healthPercent = (float)player.statLife / (float)player.statLifeMax2;
                if (healthPercent <= 0.20f && !p.loomOfFateActive)
                {
                    p.loomOfFateActive = true;
                    p.loomOfFateDuration = 300;
                }
            }
            if (p.loomOfFateActive)
            {
                p.loomOfFateDuration--;
                reaperPlayer.AddSoulEnergy(0.5f);
                if (p.loomOfFateDuration % 10 == 0)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, 
                        DustID.Ghost, 0f, 0f, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                }

                if (p.loomOfFateDuration <= 0)
                {
                    p.loomOfFateActive = false;
                    p.loomOfFateOnCooldown = true;
                    p.loomOfFateCooldown = 1200;
                }
            }
            if (p.loomOfFateOnCooldown)
            {
                p.loomOfFateCooldown--;
                if (p.loomOfFateCooldown <= 0)
                {
                    p.loomOfFateOnCooldown = false;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ThreadsOfTheSoul>(), 1)
                .AddIngredient(ItemID.Loom, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}