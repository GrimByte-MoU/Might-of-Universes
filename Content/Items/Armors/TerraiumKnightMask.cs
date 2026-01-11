using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType. Head)]
    public class TerraiumKnightMask :  ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.defense = 35;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass. Melee) += 0.40f;
            player.GetCritChance(DamageClass.Melee) += 30;
            player.GetAttackSpeed(DamageClass. Melee) += 0.20f;
            player.endurance += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TerraiumChestplate>() && 
                   legs.type == ModContent.ItemType<TerraiumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Melee attacks inflict Terra's Rend\n" +
                             "Three Terraium Glaives orbit around you\n" +
                             "For every 5 defense:  +1% melee damage, +1% melee speed, +0.5% damage reduction";
            
            var modPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            modPlayer. knightSetBonus = true;
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