using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class FrigidHeartPlayer : ModPlayer
    {
        public bool hasFrigidHeart = false;
        private int shardSpawnTimer = 0;
        private const int maxShards = 10;

        public override void ResetEffects()
        {
            hasFrigidHeart = false;
        }

        public override void PostUpdate()
        {
            if (!hasFrigidHeart)
                return;

            int shardType = ModContent.ProjectileType<FrigidHeartShard>();
            int currentShards = 0;

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == shardType && proj.owner == Player.whoAmI)
                    currentShards++;
            }

            if (currentShards < maxShards)
            {
                shardSpawnTimer++;
                if (shardSpawnTimer >= 90)
                {
                    Vector2 spawnPos = Player.Center + new Vector2(32, 0).RotatedByRandom(MathHelper.TwoPi);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, Vector2.Zero, shardType, 0, 0f, Player.whoAmI);
                    shardSpawnTimer = 0;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryFireShardOnCrit(target, hit, damageDone);
        }

        private void TryFireShardOnCrit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasFrigidHeart || !hit.Crit)
                return;

            int count = Player.statLife <= Player.statLifeMax2 / 2 ? 2 : 1;
            int projType = ModContent.ProjectileType<FrigidHeartShard>();

            for (int i = 0; i < count; i++)
            {
                Vector2 velocity = (target.Center - Player.Center).SafeNormalize(Vector2.UnitX).RotatedByRandom(MathHelper.ToRadians(10f)) * 10f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, velocity, projType, damageDone, 0f, Player.whoAmI);
            }
        }
    }
}

