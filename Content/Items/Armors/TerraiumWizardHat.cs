using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System. Collections.Generic;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class TerraiumWizardHat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass. Magic) += 0.40f;
            player.GetCritChance(DamageClass.Magic) += 25;
            player.statManaMax2 += 100;
            player.manaCost -= 0.25f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TerraiumChestplate>() && 
                   legs.type == ModContent.ItemType<TerraiumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Magic attacks inflict Terra's Rend\n" +
                             "Immunity to Mana Sickness\n" +
                             "Magic weapons ignore enemy defense depending on use time\n" +
                             "For every 10 max mana the player gets +1% magic damage";
            
            player.buffImmune[BuffID.ManaSickness] = true;
            
            var modPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            modPlayer. wizardSetBonus = true;
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