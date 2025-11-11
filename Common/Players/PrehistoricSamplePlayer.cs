using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class PrehistoricSamplePlayer : ModPlayer
    {
        public bool hasPrehistoricSample = false;

        public override void ResetEffects()
        {
            hasPrehistoricSample = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrehistoricSample && damageDone > 0)
            {
                ApplyTarred(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrehistoricSample && damageDone > 0)
            {
                ApplyTarred(target);
            }
        }

        private void ApplyTarred(NPC target)
        {
            if (target != null && target.active)
            {
                int tarredType = GetTarredBuffType();
                if (tarredType != -1)
                {
                    target.AddBuff(tarredType, 300);
                }
            }
        }

        private int GetTarredBuffType()
        {
            if (ModContent.TryFind<ModBuff>("MightofUniverses/Tarred", out var tarred))
            {
                return tarred.Type;
            }
            if (ModContent.TryFind("Tarred", out tarred))
            {
                return tarred.Type;
            }
            return -1;
        }
    }
}