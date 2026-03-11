using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CyberLeaf : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.alpha = 0;
            Projectile.scale = 1.0f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            int targetIndex = (int)Projectile.ai[0];
            
            if (targetIndex >= 0 && targetIndex < Main.maxNPCs)
            {
                NPC target = Main.npc[targetIndex];
                
                if (target.active && !target.dontTakeDamage)
                {
                    Vector2 direction = target.Center - Projectile.Center;
                    float distance = direction.Length();
                    
                    if (distance > 20f)
                    {
                        direction.Normalize();
                        float homingSpeed = 16f;
                        Projectile.velocity = direction * homingSpeed;
                    }
                }
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass);
                dust.noGravity = true;
                dust.scale = 0.8f;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 60);
            target.AddBuff(ModContent.BuffType<EnemyBleeding>(), 180);


            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(target.Center, 10, 10, DustID.JunglePlants);
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass);
                dust.velocity = Main.rand.NextVector2Circular(2f, 2f);
                dust.scale = 1f;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}