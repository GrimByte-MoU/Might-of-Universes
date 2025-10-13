using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalItems
{
    /// <summary>
    /// Reclasses Shield of Cthulhu dash damage to PacifistDamageClass
    /// </summary>
    public class VanillaPacifistGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.EoCShield)
            {
            
            }
        }
    }
}