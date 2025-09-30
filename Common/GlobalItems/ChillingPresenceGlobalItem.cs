using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalItems
{
    public class ChillingPresenceGlobalItem : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            if (reaper == null || !reaper.chillingPresence) return;
            if (item.DamageType != ModContent.GetInstance<ReaperDamageClass>()) return;

            int duration = 60 * 3;
            target.AddBuff(BuffID.Frostburn, duration);
            target.AddBuff(ModContent.BuffType<Corrupted>(), duration);
            target.AddBuff(ModContent.BuffType<Spineless>(), duration);
        }
    }
}