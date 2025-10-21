using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TitanoboaProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            // Clone Scourge of the Corruptor projectile
            Projectile.CloneDefaults(ProjectileID.EatersBite);
            AIType = ProjectileID.EatersBite; // Use same AI
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Inflict Tarred for 3 seconds
            target.AddBuff(ModContent.BuffType<Tarred>(), 180);

            // Split into 4-7 mini projectiles
            int miniCount = Main.rand.Next(4, 8);
            
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < miniCount; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                    
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        ModContent.ProjectileType<TitanoboaMini>(),
                        Projectile.damage / 2,
                        Projectile.knockBack * 0.5f,
                        Projectile.owner
                    );
                }
            }
        }
    }
}