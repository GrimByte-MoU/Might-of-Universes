using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CodeBulletProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 1f, 0f);
            
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.GreenTorch, 0f, 0f, 100, default, 1f);
            }
            
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 randomPosition = Main.LocalPlayer.Center + new Vector2(
                Main.rand.Next(-200, 201),
                Main.rand.Next(-200, 201)
            );

            int proj = Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                randomPosition,
                Vector2.Zero,
                ModContent.ProjectileType<CodeBlastProjectile>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );

            if (Main.projectile[proj].ModProjectile is CodeBlastProjectile blast)
            {
                blast.targetNPC = target;
            }
        {
            target.AddBuff(ModContent.BuffType<CodeDestabilized>(), 180);
        }
        }
    }
}
