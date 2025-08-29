using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Content.Items.Consumables.BossBags
{
    public class ObeSadeeBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Item.type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartAttackGlob>(), 1));

            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SugarClot>(), 1, 15, 21));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GummyMembrane>(), 1, 25, 41));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SweetTooth>(), 1, 25, 41));

            var possibleWeapons = new List<int>
            {
                ModContent.ItemType<Conecussion>(),
                ModContent.ItemType<SweetCandylabra>(),
                ModContent.ItemType<SprinkleShaker>(),
                ModContent.ItemType<SweetlandsStaff>(),
                ModContent.ItemType<TreatSpiker>(),
                ModContent.ItemType<WaferSword>()
            };

            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1,
                possibleWeapons.ToArray()));

            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1,
                possibleWeapons.ToArray()));

            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1,
                possibleWeapons.ToArray()));
        }
    }
}
