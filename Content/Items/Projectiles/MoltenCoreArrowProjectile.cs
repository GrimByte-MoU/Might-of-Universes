using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MoltenCoreArrowProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }

            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);
            Projectile.rotation = Projectile.velocity.ToRotation();

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 180);
            Explode();
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft <= 0)
                return;
                
            Explode();
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20), 40, 40, DustID.Torch);
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int explosionRadius = 80;
                Rectangle explosionBox = new Rectangle(
                    (int)(Projectile.Center.X - explosionRadius),
                    (int)(Projectile.Center.Y - explosionRadius),
                    explosionRadius * 2,
                    explosionRadius * 2
                );

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy() && !npc.dontTakeDamage)
                    {
                        Rectangle npcBox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                        
                        if (explosionBox.Intersects(npcBox))
                        {
                            float distance = Vector2.Distance(Projectile.Center, npc.Center);
                            if (distance < explosionRadius)
                            {
                                int explosionDamage = (int)(Projectile.damage * 0.5f);
                                
                                NPC.HitInfo hitInfo = new NPC.HitInfo
                                {
                                    Damage = explosionDamage,
                                    Knockback = 2f,
                                    HitDirection = npc.Center.X > Projectile.Center.X ? 1 : -1
                                };

                                npc.StrikeNPC(hitInfo);
                                npc.AddBuff(ModContent.BuffType<CoreHeat>(), 60);

                                if (Main.netMode != NetmodeID.SinglePlayer)
                                {
                                    NetMessage.SendStrikeNPC(npc, hitInfo);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}