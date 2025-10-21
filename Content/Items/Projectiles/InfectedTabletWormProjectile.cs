using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class InfectedTabletWormProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            NPC target = Main.npc[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            if (target != null && target.CanBeChasedBy())
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 9f, 0.1f);
            }

            Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.7f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(4f, 4f);
                Dust.NewDustPerfect(Projectile.Center, DustID.RedTorch, velocity, 0, Color.Red, 1.5f).noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CodeDestabilized>(), 300); // 5 seconds
        }
    }
}
