using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ReinforcedStakeProjectile : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 9;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            
            AIType = ProjectileID.WoodenArrowFriendly;
        }

         public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.Vampire || target.type == NPCID.VampireBat)
            {
                hit.Damage = (int)(hit.Damage * 2f);
            }
            
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.WoodFurniture, Projectile.velocity.X * 0.2f, 
                    Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
            }
        }
    }
}
