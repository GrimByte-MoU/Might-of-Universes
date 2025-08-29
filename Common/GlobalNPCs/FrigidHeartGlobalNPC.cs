using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class FrigidHeartGlobalNPC : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (player.GetModPlayer<FrigidHeartPlayer>().hasFrigidHeart && hit.Crit)
            {
                FireShardAt(npc, player, damageDone);
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            // Get the owning player
            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
            {
                Player player = Main.player[projectile.owner];
                if (player.GetModPlayer<FrigidHeartPlayer>().hasFrigidHeart && hit.Crit)
                {
                    FireShardAt(npc, player, damageDone);
                }
            }
        }

        private void FireShardAt(NPC npc, Player player, int damage)
        {
            int count = player.statLife <= player.statLifeMax2 / 2 ? 2 : 1;
            int shardType = ModContent.ProjectileType<FrigidHeartShard>();

            for (int i = 0; i < count; i++)
            {
                Vector2 velocity = (npc.Center - player.Center).SafeNormalize(Vector2.UnitY) * 12f;
                Projectile.NewProjectile(
                    player.GetSource_FromThis(),
                    player.Center,
                    velocity,
                    shardType,
                    damage,
                    0f,
                    player.whoAmI
                );
            }
        }
    }
}


