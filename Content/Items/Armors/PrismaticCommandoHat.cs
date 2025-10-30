using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class PrismaticCommandoHat : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 15;
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

            player.setBonus = $"+10% ranged damage and ranged weapons inflict Prismatic Rend for 3 seconds.\n Press '{key}' to gain tripled firing speed and doubled velocity for 3 seconds.\n This ability has a 10 second cooldown.";
            player.GetDamage(DamageClass.Ranged) += 0.10f;
            player.GetModPlayer<PrismaticPlayer>().prismaticCommandoSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.07f;
            player.GetCritChance(DamageClass.Ranged) += 18;
            player.GetModPlayer<PrismaticPlayer>().ammoConserveChance += 0.25f;
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