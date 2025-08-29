using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class PrismaticGauntletPlayer : ModPlayer
    {
        public bool hasPrismaticGauntlet;

        public override void ResetEffects()
        {
            hasPrismaticGauntlet = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticGauntlet && hit.DamageType == DamageClass.Melee)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            }
        }
        }
    }

