using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalItems
{
    public class PrismaticGlobalItem : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player == null || !player.active) return;

            var pp = player.GetModPlayer<PrismaticPlayer>();
            int rendBuff = ModContent.BuffType<PrismaticRend>();

            if (pp.prismaticWizardSet && item.DamageType == DamageClass.Magic)
            {
                target.AddBuff(rendBuff, 180);
            }

            if (pp.prismaticKnightSet && item.DamageType == DamageClass.Melee)
            {
                target.AddBuff(rendBuff, 180);
            }

            if (pp.prismaticCommandoSet && item.DamageType == DamageClass.Ranged)
            {
                target.AddBuff(rendBuff, 180);
            }

            if (pp.prismaticConjurerSet && item.DamageType == DamageClass.Summon)
            {
                target.AddBuff(rendBuff, 180);
            }
        }
    }
}