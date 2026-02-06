using Terraria;
using Terraria. ID;
using Terraria. ModLoader;
using Microsoft. Xna.Framework;
using MightofUniverses.Content. Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class TerraiumRangerVisor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item. height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.40f;
            player.GetCritChance(DamageClass.Ranged) += 30;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.20f;
            player.ammoCost80 = true;
            
            if (Main.rand.NextBool(10))
            {
                player. ammoCost75 = true;
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TerraiumChestplate>() && 
                   legs.type == ModContent.ItemType<TerraiumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Ranged attacks inflict Terra's Rend\n" +
                             "Attack speed increases the longer you fire\n" +
                             "Up to +50% ranged attack speed after 5 seconds";
            
            var modPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            modPlayer.rangerSetBonus = true;
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