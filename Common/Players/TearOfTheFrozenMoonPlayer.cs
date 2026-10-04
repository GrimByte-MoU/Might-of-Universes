using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class TearOfTheFrozenMoonPlayer : ModPlayer
    {
        private const int FreezeDuration = 3 * 60;
        private const int FreezeCooldownLength = 10 * 60;
        private const int ShardCount = 8;
        private const int ShardDamage = 100;
        private const float ShardSpeed = 9f;

        public bool hasTearOfTheFrozenMoon;
        private int freezeCooldown;

        public override void ResetEffects()
        {
            hasTearOfTheFrozenMoon = false;
        }

        public override void PostUpdate()
        {
            if (freezeCooldown > 0)
                freezeCooldown--;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyDefenseIgnore(target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyDefenseIgnore(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            HandleHit(target, hit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.type == ModContent.ProjectileType<FrozenMoonShard>())
                return;

            HandleHit(target, hit);
        }

        private void ApplyDefenseIgnore(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasTearOfTheFrozenMoon && target.HasBuff(BuffID.Frozen))
                modifiers.ArmorPenetration += 100;
        }

        private void HandleHit(NPC target, NPC.HitInfo hit)
        {
            if (!hasTearOfTheFrozenMoon || Player.whoAmI != Main.myPlayer)
                return;

            if (target.HasBuff(BuffID.Frozen))
            {
                if (hit.Crit)
                {
                    target.DelBuff(target.FindBuffIndex(BuffID.Frozen));
                    SpawnShards(target);
                }
                return;
            }

            if (freezeCooldown <= 0 && target.HasBuff(ModContent.BuffType<SheerCold>()))
            {
                target.AddBuff(BuffID.Frozen, FreezeDuration);
                freezeCooldown = FreezeCooldownLength;
            }
        }

        private void SpawnShards(NPC target)
        {
            var source = Player.GetSource_OnHit(target);
            float offset = Main.rand.NextFloat(MathHelper.TwoPi);

            for (int i = 0; i < ShardCount; i++)
            {
                float angle = offset + MathHelper.TwoPi * i / ShardCount;
                Vector2 velocity = angle.ToRotationVector2() * ShardSpeed;

                Projectile.NewProjectile(
                    source,
                    target.Center,
                    velocity,
                    ModContent.ProjectileType<FrozenMoonShard>(),
                    ShardDamage,
                    0f,
                    Player.whoAmI);
            }
        }
    }
}
