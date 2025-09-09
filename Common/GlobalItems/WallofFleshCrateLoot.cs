using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Common.DropConditions;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Common.GlobalItems
{
    public sealed class WallofFleshCrateLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void ModifyItemLoot(Terraria.Item item, ItemLoot itemLoot)
        {
            var Hardmode = new DownedWallofFleshCondition();
            if (CrateIds.Is(item.type, CrateIds.Hellstone))
            {
                itemLoot.Add(ItemDropRule.ByCondition(Hardmode, ModContent.ItemType<DevilsBlood>(), 10, 3, 5));
            }
        }
    }
}