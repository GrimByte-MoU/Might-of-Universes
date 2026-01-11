using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content. Rarities;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using System. Collections.Generic;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class TerraiumReaperHood : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.38f;
            player.GetCritChance(reaperClass) += 35;
            
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.hasReaperArmor = true;
            reaperPlayer.soulGatherMultiplier += 8f / 60f;
            
            var accPlayer = player.GetModPlayer<ReaperAccessoryPlayer>();
            accPlayer. SoulCostMultiplier *= 0.80f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TerraiumChestplate>() && 
                   legs.type == ModContent.ItemType<TerraiumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+500 max souls\n" +
                             "Reaper attacks inflict Terra's Rend\n" +
                             "Consuming souls fires 3 Terra Missiles and grants 1 Death Mark";
            
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.maxSoulEnergy += 500;
            
            var modPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            modPlayer. reaperSetBonus = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}