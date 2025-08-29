using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class PrismaticBeetlePlayer : ModPlayer
    {
        public bool hasPrismaticBeetle;

        public override void ResetEffects()
        {
            hasPrismaticBeetle = false;
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