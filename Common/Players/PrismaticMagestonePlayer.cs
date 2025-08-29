using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class PrismaticMagestonePlayer : ModPlayer
    {
        public bool hasPrismaticMagestone;
        

        public override void ResetEffects()
        {
            hasPrismaticMagestone = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticMagestone && hit.DamageType == DamageClass.Magic)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            }
        }
        }
    }