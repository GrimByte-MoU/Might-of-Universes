using Microsoft.Xna.Framework;
using Terraria;
using Terraria. ID;
using Terraria. ModLoader;
using Terraria.Audio;
using MightofUniverses. Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class DemonicSickle : MoUProjectile
    {
        private const int SpinDuration = 30;
        private const float TravelSpeed = 16f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile. timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.6f;
            Projectile. usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            var reaperPlayer = owner.GetModPlayer<ReaperPlayer>();

            Projectile.ai[0]++;

            if (Projectile.ai[0] < SpinDuration)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation += 0.3f;

                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(Projectile. position, Projectile.width, Projectile.height,
                        DustID. Shadowflame, 0f, 0f, 100, Color.Purple, 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                }
            }
            else
            {
                if (Projectile.ai[0] == SpinDuration)
                {
                    Vector2 direction = Projectile.ai[1]. ToRotationVector2();
                    Projectile.velocity = direction * TravelSpeed;
                    SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                }

                Projectile.rotation += 0.5f;

                if (Main.rand.NextBool())
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Shadowflame, 0f, 0f, 100, Color.Purple, 1.2f);
                    dust.noGravity = true;
                }
            }

            if (Projectile.ai[0] % 2 == 0)
            {
                if (! reaperPlayer.ConsumeSoulEnergy(1))
                {
                    var cfPlayer = owner.GetModPlayer<CoreOfFleshPlayer>();
                    cfPlayer.ForceSwitchToLaser = true;
                    Projectile.Kill();
                    return;
                }
            }

            Lighting.AddLight(Projectile. Center, 0.6f, 0.2f, 0.8f);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FlatBonusDamage += 50;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                    DustID.Shadowflame, 0f, 0f, 100, Color. Purple, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }

            SoundEngine.PlaySound(SoundID.NPCHit54, target.Center);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Shadowflame, 0f, 0f, 100, Color. Purple, 2f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
    }
}