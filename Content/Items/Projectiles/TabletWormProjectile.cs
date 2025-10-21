using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TabletWormProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.scale = 1.25f;
        }

        public override void AI()
        {
            NPC target = Main.npc[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            if (target != null && target.CanBeChasedBy())
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 6f, 0.1f);
            }

            Lighting.AddLight(Projectile.Center, Color.Lime.ToVector3() * 0.6f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
                Dust.NewDustPerfect(Projectile.Center, DustID.GreenFairy, velocity, 0, Color.LimeGreen, 1.2f).noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CodeDestabilized>(), 180); // 3 seconds
        }
    }
}
