using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class UnderworldSorcererMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.GetCritChance(DamageClass.Magic) += 5f;
            player.manaCost -= 0.06f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<UnderworldSorcererRobes>() 
                && legs.type == ModContent.ItemType<UnderworldSorcererBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.statManaMax2 += 100;
            player.setBonus = "+100 max mana\n Magic attacks inflict Hellfire for 3 seconds and Demonfire for 2 seconds";
            player.GetModPlayer<UnderworldSorcererPlayer>().setActive = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 8)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddIngredient(ItemID.Bone, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}