using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraiumDaggerProjectile : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 5;
        }

        public override void AI()
        {
            
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.1f, 0.5f, 0.1f);

            if (Main.rand.NextBool(4))
            {
                var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade);
                d.velocity *= 0.1f;
                d.scale = 0.8f;
                d.noGravity = true;
                d.color = new Color(160, 210, 255);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
         target.AddBuff(ModContent.BuffType<TerrasRend>(), 120);
        }
    }
}
