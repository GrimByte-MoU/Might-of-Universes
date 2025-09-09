using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
	public class StakeLauncherGlobalItem : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstantiation) {
			return item.type == ItemID.StakeLauncher;
		}

		public override void SetDefaults(Item item) {
			item.StatsModifiedBy.Add(Mod);

			item.damage = 85;
		}
	}
}