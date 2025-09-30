using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class ChillingPresenceGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0) return;
            Player owner = Main.player[projectile.owner];
            var reaper = owner.GetModPlayer<ReaperPlayer>();
            if (reaper == null || !reaper.chillingPresence) return;
            if (projectile.DamageType != ModContent.GetInstance<ReaperDamageClass>()) return;

            int duration = 180;
            target.AddBuff(BuffID.Frostburn, duration);
            target.AddBuff(ModContent.BuffType<Corrupted>(), duration);
            target.AddBuff(ModContent.BuffType<Spineless>(), duration);
        }
    }
}