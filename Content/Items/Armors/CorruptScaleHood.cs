using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class CorruptScaleHood : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.25f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CorruptScaleChestplate>() &&
                   legs.type == ModContent.ItemType<CorruptScaleBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+55% nonweapon damage, -25% damage to all classes\nCreates a dark aura that deals 45 damage per second and inflicts Corrupted";
            player.GetModPlayer<CorruptScalePlayer>().hasCorruptScaleSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}