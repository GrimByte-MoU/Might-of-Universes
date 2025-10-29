using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class PrismaticGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers) return;
            Player player = Main.player[projectile.owner];
            if (player == null || !player.active) return;

            var pp = player.GetModPlayer<PrismaticPlayer>();
            int rendBuff = ModContent.BuffType<PrismaticRend>();

            if (pp.prismaticWizardSet && projectile.DamageType == DamageClass.Magic)
            {
                target.AddBuff(rendBuff, 180);
            }
            else if (pp.prismaticKnightSet && projectile.DamageType == DamageClass.Melee)
            {
                target.AddBuff(rendBuff, 180);
            }
            else if (pp.prismaticCommandoSet && projectile.DamageType == DamageClass.Ranged)
            {
                target.AddBuff(rendBuff, 180);
            }
            else if (pp.prismaticConjurerSet && projectile.DamageType == DamageClass.Summon)
            {
                target.AddBuff(rendBuff, 180);
            }
        }
    }
}