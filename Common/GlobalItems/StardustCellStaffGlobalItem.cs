using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
	// This file shows a very simple example of a GlobalItem class. GlobalItem hooks are called on all items in the game and are suitable for sweeping changes like
	// adding additional data to all items in the game. Here we simply adjust the damage of the Copper Shortsword item, as it is simple to understand.
	// See other GlobalItem classes in ExampleMod to see other ways that GlobalItem can be used.
	public class StardustCellStaffGlobalItem : GlobalItem
	{
		// Here we make sure to only instance this GlobalItem for the Copper Shortsword, by checking item.type
		public override bool AppliesToEntity(Item item, bool lateInstantiation) {
			return item.type == ItemID.StardustCellStaff;
		}

		public override void SetDefaults(Item item) {
			item.StatsModifiedBy.Add(Mod); // Notify the game that we've made a functional change to this item.

			item.damage = 65;
		}
	}
}