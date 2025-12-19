using Microsoft.Xna.Framework;
using Terraria;
using Terraria. ID;
using Terraria. ModLoader;
using MightofUniverses.Content. Items.Buffs;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ClockworkBomb : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile. DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile. timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation += 0.1f;

            Lighting.AddLight(Projectile.Center, 0.5f, 0.3f, 0.1f);

            if (Main.rand. NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 0.8f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID. Smoke, speed.X * 3, speed.Y * 3);
            }

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust. NewDust(Projectile. position, Projectile.width, Projectile.height, DustID.Torch, speed.X * 4, speed.Y * 4);
            }

            SoundEngine.PlaySound(SoundID. Item14, Projectile.position);

            float explosionRadius = 96f;
            
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < explosionRadius)
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    float damageMult = 1f - (distance / explosionRadius);
                    int finalDamage = (int)(Projectile.damage * damageMult);

                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = finalDamage,
                        Knockback = Projectile.knockBack,
                        HitDirection = (npc.Center.X > Projectile.Center.X) ? 1 : -1
                    });

                    npc.AddBuff(ModContent.BuffType<Shred>(), 120);
                    npc.AddBuff(BuffID.Oiled, 120);
                    npc.AddBuff(BuffID.OnFire3, 120);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Shred>(), 120);
            target.AddBuff(BuffID. Oiled, 120);
            target.AddBuff(BuffID.OnFire3, 120);
        }
    }
}