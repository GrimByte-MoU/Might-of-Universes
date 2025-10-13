using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;

namespace MightofUniverses.Common.Players
{
    /// <summary>
    /// Handles Shield of Cthulhu dash damage conversion to Pacifist
    /// </summary>
    public class VanillaPacifistPlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            for (int i = 3; i < 10; i++)
            {
                if (Player.armor[i].type == ItemID.EoCShield)
                {
                    break;
                }
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.dashDelay < 0)
            {
                for (int i = 3; i < 10; i++)
                {
                    if (Player.armor[i].type == ItemID.EoCShield)
                    {
                        var pacifistPlayer = Player.GetModPlayer<PacifistPlayer>();
                        modifiers.FinalDamage *= pacifistPlayer.pacifistDamageMultiplier;
                        break;
                    }
                }
            }
        }
    }
}