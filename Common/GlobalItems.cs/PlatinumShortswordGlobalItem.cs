using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
    	public class PlatinumShortswordGlobalItem : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstantiation) {
			return item.type == ItemID.PlatinumShortsword;
		}

		public override void SetDefaults(Item item) {
			item.StatsModifiedBy.Add(Mod);

			item.damage = 26;
		}
	}
}