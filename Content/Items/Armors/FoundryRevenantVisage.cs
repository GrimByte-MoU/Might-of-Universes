using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class FoundryRevenantVisage : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;

            player.AddBuff(BuffID.Dangersense, 2);
            player.AddBuff(BuffID.Hunter, 2);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FoundryRevenantBoilerplate>()
                && legs.type == ModContent.ItemType<FoundryRevenantTreads>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "+250 max souls, +10% Reaper damage\n" +
                "Consuming souls grants Foundry stacks (up to 50) that persist until death or unequipping.\n" +
                "Stacks: +1 armor penetration each; 5+: +1% Reaper damage each; 10+: +1% movement speed each;\n" +
                "25+: +0.1% dodge chance each;\n" +
                "50: +0.2% damage reduction each";

            var foundry = player.GetModPlayer<FoundryRevenantPlayer>();
            player.GetModPlayer<ReaperPlayer>().hasReaperArmor = true;
            foundry.FullSetEquipped = true;
            player.GetModPlayer<ReaperPlayer>().maxSoulEnergy += 250;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.Cog, 20)
                .AddIngredient(ItemID.Wire, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}