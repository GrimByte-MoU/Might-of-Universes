using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.Players
{
    public class SnackPouchPlayer : ModPlayer
    {
        public bool hasSnackPouch;
        public bool hasGummyClusterPouch;

        public override void ResetEffects()
        {
            hasSnackPouch = false;
            hasGummyClusterPouch = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();

            if (hasGummyClusterPouch)
            {
                if (hit.Crit)
                {
                    Player.Heal(3);
                    reaperPlayer.soulGatherMultiplier += 1f;
                }
                
                if (Player.HasBuff(ModContent.BuffType<Hyper>()))
                {
                    Player.Heal(5);
                    reaperPlayer.soulGatherMultiplier += 2f;
                }
            }
            else if (hasSnackPouch)
            {
                if (reaperPlayer.hasReaperArmor) // Changed to check for Reaper armor instead
                {
                    Player.Heal(2);
                }
                
                if (Player.HasBuff(ModContent.BuffType<Hyper>()))
                {
                    Player.Heal(1);
                    reaperPlayer.soulGatherMultiplier += 1f;
                }
            }
        }
    }
}

