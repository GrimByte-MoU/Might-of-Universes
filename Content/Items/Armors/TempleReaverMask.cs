using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class TempleReaverMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 9);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.06f;
            player.GetCritChance(reaper) += 6f;

            player.statLifeMax2 += 15;
            player.endurance += 0.04f;
            
            player.GetModPlayer<ReaperAccessoryPlayer>().SoulCostMultiplier *= 0.90f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TempleReaverPlate>()
                && legs.type == ModContent.ItemType<TempleReaverBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "+350 max souls, +10% damage reduction\n" +
                "Soul Empowerment from soul consumption lasts twice as long \n" +
                "Being hit while Empowered ends the buff and halts soul gain for 5 seconds ";

            player.GetModPlayer<TempleReaverPlayer>().FullSetEquipped = true;
            player.GetModPlayer<ReaperPlayer>().hasReaperArmor = true;
            player.GetModPlayer<ReaperPlayer>().maxSoulEnergy += 350;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LihzahrdBrick, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}