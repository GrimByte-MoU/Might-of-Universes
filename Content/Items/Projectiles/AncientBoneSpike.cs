using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AncientBoneSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.ArmorPenetration = 25;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
            Vector2 target = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 to = target - Projectile.Center;
            float dist = to.Length();

            if (dist < 8f)
            {
                Projectile.Kill();
                return;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 desiredVel = to.SafeNormalize(Vector2.Zero) * 20f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVel, 0.2f);
            if (Main.rand.NextBool(4))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale = 1.1f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Tarred>(), 120);
        }
    }
}