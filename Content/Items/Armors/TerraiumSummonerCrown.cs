using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses. Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class TerraiumSummonerCrown : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item. sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.35f;
            player.maxMinions += 3;
            player.whipRangeMultiplier += 0.30f;
            player.GetAttackSpeed(DamageClass. SummonMeleeSpeed) += 0.30f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TerraiumChestplate>() && 
                   legs.type == ModContent.ItemType<TerraiumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summon attacks inflict Terra's Rend\n" +
                             "+3 max minions and +35% summon damage\n" +
                             "A Miniature Worldsoul defends you";
            
            player.maxMinions += 3;
            player.GetDamage(DamageClass.Summon) += 0.35f;
            
            var modPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            modPlayer. summonerSetBonus = true;
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