using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VisceraBoltProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.15f));
            Projectile.rotation = Projectile.velocity.ToRotation();

            NPC target = FindClosestNPC(700f);
            if (target != null)
            {
                Vector2 desired = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 12f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.12f);
            }

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 120, default, 1.1f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int t = 60 * 5;
            target.AddBuff(BuffID.Ichor, t);
            target.AddBuff(ModContent.BuffType<Spineless>(), t);
            target.AddBuff(ModContent.BuffType<EnemyBleeding>(), t);
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closest = null;
            float sq = maxDetectDistance * maxDetectDistance;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.life <= 0) continue;
                float dist = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                if (dist < sq) { sq = dist; closest = npc; }
            }
            return closest;
        }
    }
}