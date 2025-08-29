using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class PrismaticAmmoBagPlayer : ModPlayer
    {
        public bool hasPrismaticAmmoBag;

        public override void ResetEffects()
        {
            hasPrismaticAmmoBag = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticAmmoBag && hit.DamageType == DamageClass.Ranged)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            }
        }
        }
    }