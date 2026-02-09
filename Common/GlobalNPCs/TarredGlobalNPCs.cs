using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class TarredGlobalNPC : GlobalNPC
    {
        private const float ExplosionRadiusPx = 5f * 16f;
        private const int ExplosionDamage = 200;
        private const int TarredDurationTicks = 60 * 3;

        public override bool InstancePerEntity => true;

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<Tarred>()))
                modifiers.SourceDamage *= 1.20f;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<Tarred>()) && projectile.friendly && projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
                modifiers.SourceDamage *= 1.20f;
        }

        public override void OnKill(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<Tarred>()))
                return;

            for (int i = 0; i < 30; i++)
            {
                int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Smoke, Scale: Main.rand.NextFloat(1.1f, 1.6f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 2.5f;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            int owner = (npc.lastInteraction >= 0 && npc.lastInteraction < Main.maxPlayers) ? npc.lastInteraction : Main.myPlayer;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target == null || !target.active || target.friendly || target.dontTakeDamage || target.life <= 0 || target.whoAmI == npc.whoAmI)
                    continue;

                if (Vector2.Distance(target.Center, npc.Center) <= ExplosionRadiusPx)
                {
                    int finalDamage = ExplosionDamage;
                    NPC.HitInfo info = new NPC.HitInfo
                    {
                        Damage = finalDamage,
                        Knockback = 0f,
                        HitDirection = (target.Center.X > npc.Center.X) ? 1 : -1,
                        DamageType = DamageClass.Generic
                    };
                    target.StrikeNPC(info);

                    target.AddBuff(ModContent.BuffType<Tarred>(), TarredDurationTicks);
                }
            }
        }
    }
}