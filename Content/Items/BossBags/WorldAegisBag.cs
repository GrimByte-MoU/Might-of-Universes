using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Content.Items.BossBags
{
    public class WorldAegisBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.expert = true;
            Item.width = 36;
            Item.height = 36;
            Item.rare = ItemRarityID.Expert;
            Item.maxStack = 99;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int[] weapons = new int[]
            {
                ModContent.ItemType<MouldoftheMother>(),
                ModContent.ItemType<TomeoftheWorldsSoul>(),
                ModContent.ItemType<GaiasLongbow>(),
                ModContent.ItemType<WorldwalkerSword>(),
                ModContent.ItemType<BiomeCleanser>(),
            };
            
            itemLoot.Add(new FewFromOptionsDropRule(2, 2, 2, weapons));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AegisRemains>(), 1, 300, 401));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AegisReplica>()));
            itemLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<EarthenCrown>()));
        }
    }
}