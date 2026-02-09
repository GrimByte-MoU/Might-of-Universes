using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses. Common.Players
{
    public class PrismaticBeetlePlayer : ModPlayer
    {
        public bool hasPrismaticBeetle;

        public override void ResetEffects()
        {
            hasPrismaticBeetle = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticBeetle && proj.DamageType == DamageClass.Summon)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);

                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                        DustID.RainbowMk2, 0f, 0f, 100, default, 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                }
            }
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticBeetle && hit.DamageType == DamageClass.Summon)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            }
        }
    }
}