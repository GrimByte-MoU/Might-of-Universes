using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class PrismaticConjurerHood : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrismaticChestplate>()
                && legs.type == ModContent.ItemType<PrismaticGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = $"+10% summon damage. +3 max minion and sentry slots.\n Whips and Minions inflict Prismatic Rend for 3 seconds.\n A Prismatic sentinel will defend you.";
            player.GetDamage(DamageClass.Summon) += 0.10f;
            player.maxMinions += 3;
            player.maxTurrets += 3;
            player.GetModPlayer<PrismaticPlayer>().prismaticConjurerSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.07f;
            player.maxMinions += 2;
            player.whipRangeMultiplier += 0.15f;
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