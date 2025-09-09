using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
    	public class IronShortswordGlobalItem : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstantiation) {
			return item.type == ItemID.IronShortsword;
		}

		public override void SetDefaults(Item item) {
			item.StatsModifiedBy.Add(Mod);

			item.damage = 16;
		}
	}
}