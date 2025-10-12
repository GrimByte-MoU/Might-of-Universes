using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent; // ContentSamples.ItemsByType
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.ModPlayers
{
    public class HorrorTonguePlayer : ModPlayer
    {
        private static int BladetongueProjType => ContentSamples.ItemsByType[ItemID.Bladetongue].shoot;

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.type == BladetongueProjType && Player.HeldItem?.type == ModContent.ItemType<HorrorTongue>())
            {
                target.AddBuff(BuffID.Ichor, 180);
                target.AddBuff(ModContent.BuffType<EnemyBleeding>(), 180);
            }
        }
    }
}