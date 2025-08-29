using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class BackbonePlayer : ModPlayer
    {
        public bool hasBackbone;

        public override void ResetEffects()
        {
            hasBackbone = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasBackbone)
            {
                target.AddBuff(ModContent.BuffType<Spineless>(), 180);
            }
        }
    }
}
