using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class UnderworldSorcererPlayer : ModPlayer
    {
        public bool setActive;

        public override void ResetEffects()
        {
            setActive = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (setActive && proj.DamageType == DamageClass.Magic)
            {
                target.AddBuff(ModContent.BuffType<Demonfire>(), 120);
                target.AddBuff(BuffID.OnFire3, 180); 
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (setActive && item.DamageType == DamageClass.Magic)
            {
                target.AddBuff(ModContent.BuffType<Demonfire>(), 120);
                target.AddBuff(BuffID.OnFire3, 180);
            }
        }
    }
}