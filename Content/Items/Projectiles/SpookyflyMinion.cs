using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SpookyflyMinion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.damage = 50;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3());

            NPC target = null;
            float distance = 300f;

            foreach (var npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this) && Vector2.Distance(npc.Center, Projectile.Center) < distance)
                {
                    distance = Vector2.Distance(npc.Center, Projectile.Center);
                    target = npc;
                }
            }

            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 12f;
                Projectile.velocity = direction;
                target.AddBuff(ModContent.BuffType<Terrified>(), 180);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
