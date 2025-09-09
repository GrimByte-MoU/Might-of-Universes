using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
    public class SandgunGlobalItem : GlobalItem
    {
        // Here we make sure to only instance this GlobalItem for the Copper Shortsword, by checking item.type
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ItemID.Sandgun;
        }

        public override void SetDefaults(Item item)
        {
            item.StatsModifiedBy.Add(Mod); // Notify the game that we've made a functional change to this item.

            item.damage = 35;
            item.shootSpeed *= 1.25f;
        }
    }
}