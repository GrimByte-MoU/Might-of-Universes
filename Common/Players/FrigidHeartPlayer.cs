using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses. Content.Items.Projectiles;
using Terraria.Audio;
using System.Collections.Generic;

namespace MightofUniverses. Common.Players
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
            if (! hasFrigidHeart)
                return;

            int shardType = ModContent.ProjectileType<FrigidHeartShard>();
            int currentShards = CountOrbitingShards();

            if (currentShards < maxShards)
            {
                shardSpawnTimer++;
                if (shardSpawnTimer >= 90)
                {
                    int index = Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        shardType,
                        0,
                        0f,
                        Player. whoAmI
                    );

                    if (index >= 0 && index < Main.maxProjectiles)
                    {
                        Main.projectile[index]. ai[1] = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                    }

                    shardSpawnTimer = 0;
                }
            }
        }

        private int CountOrbitingShards()
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active &&
                    proj.type == ModContent.ProjectileType<FrigidHeartShard>() &&
                    proj.owner == Player.whoAmI &&
                    proj.ai[0] == 0f)
                {
                    count++;
                }
            }
            return count;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.owner == Player.whoAmI && hit.Crit && hasFrigidHeart)
            {
                TryFireShardOnCrit(target, damageDone);
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && hasFrigidHeart)
            {
                TryFireShardOnCrit(target, damageDone);
            }
        }

        private void TryFireShardOnCrit(NPC target, int damageDone)
        {
            int shardsToFire = Player.statLife <= (Player.statLifeMax2 / 2) ? 2 : 1;
            int shardType = ModContent.ProjectileType<FrigidHeartShard>();

            Vector2 baseDirection = (target.Center - Player.Center).SafeNormalize(Vector2.UnitX);

            List<int> shardsToFireList = new List<int>();

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (shardsToFireList.Count >= shardsToFire)
                    break;

                Projectile proj = Main.projectile[i];

                if (!proj. active || proj.type != shardType || proj.owner != Player. whoAmI || proj.ai[0] != 0f)
                    continue;

                shardsToFireList.Add(i);
            }

            for (int j = 0; j < shardsToFireList.Count; j++)
            {
                int projIndex = shardsToFireList[j];
                Projectile proj = Main.projectile[projIndex];

                Vector2 direction = baseDirection;

                if (shardsToFireList.Count == 2)
                {
                    float spreadAngle = MathHelper.ToRadians(j == 0 ? -15f : 15f);
                    direction = direction.RotatedBy(spreadAngle);
                }

                proj.ai[0] = 1f;
                proj.velocity = direction * 14f;
                proj.damage = damageDone;
                proj.penetrate = 3;
                proj. tileCollide = true;
                proj.timeLeft = 300;
                proj.netUpdate = true;
            }

            if (shardsToFireList.Count > 0)
            {
                SoundEngine.PlaySound(SoundID.Item28, Player.Center);

                if (Main.myPlayer == Player.whoAmI)
                {
                    string message = shardsToFireList.Count == 2 ? "Double Shard!" : "Shard Fired!";
                    CombatText.NewText(Player.getRect(), Color.Cyan, message, false);
                }
            }
        }
    }
}