using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class PrismaticKnightMask : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrismaticChestplate>()
                && legs.type == ModContent.ItemType<PrismaticGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            string key = ModKeybindManager.ArmorAbility?.GetAssignedKeys().Count > 0 
                ? ModKeybindManager.ArmorAbility.GetAssignedKeys()[0] 
                : "[Unbound]";
            player.setBonus = $"+10% melee damage and melee weapons inflict Prismatic Rend for 3 seconds.\n Press '{key}' to gain 10% melee critical strike chance, 15% melee attack speed, and 25% melee size for 5 seconds.";
            player.GetDamage(DamageClass.Melee) += 0.10f;
            player.GetModPlayer<PrismaticPlayer>().prismaticKnightSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.07f;
            player.GetCritChance(DamageClass.Melee) += 18;
            player.GetAttackSpeed(DamageClass.Melee) += 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}