using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
    	public class ChlorophyteClaymoreGlobalItem : GlobalItem
	{
		// Here we make sure to only instance this GlobalItem for the Copper Shortsword, by checking item.type
		public override bool AppliesToEntity(Item item, bool lateInstantiation) {
			return item.type == ItemID.ChlorophyteClaymore;
		}

		public override void SetDefaults(Item item) {
			item.StatsModifiedBy.Add(Mod);

            item.useTime = 20;
            item.useAnimation = 20;
            item.autoReuse = true;
		}
	}
}