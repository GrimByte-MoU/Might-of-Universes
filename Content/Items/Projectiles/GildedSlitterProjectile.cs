using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GildedSlitterProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        private bool isStuck = false;
        private int stuckNPCIndex = -1;
        private Vector2 stuckOffset = Vector2.Zero;
        private int damageTickCounter = 0;
        private const int maxDamageTicks = 9;

        public override void AI()
        {
            if (isStuck && stuckNPCIndex >= 0 && stuckNPCIndex < Main.maxNPCs)
            {
                NPC stuckNPC = Main.npc[stuckNPCIndex];

                if (stuckNPC.active && !stuckNPC.dontTakeDamage)
                {
                    Projectile.Center = stuckNPC.Center + stuckOffset;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.rotation = stuckOffset.ToRotation();

                    Projectile.localAI[0]++;
                    if (Projectile.localAI[0] >= 10)
                    {
                        Projectile.localAI[0] = 0;
                        damageTickCounter++;

                        if (damageTickCounter <= maxDamageTicks)
                        {
                            int damage = (int)(Projectile.originalDamage * 0.8f);
                            stuckNPC.StrikeNPC(stuckNPC.CalculateHitInfo(damage, 0, false, 0f));

                            for (int i = 0; i < 3; i++)
                            {
                                Dust.NewDust(
                                    Projectile.position, 
                                    Projectile.width, 
                                    Projectile.height,
                                    DustID.SpectreStaff, 
                                    0, 0, 0, 
                                    Color.Gold, 
                                    1.0f
                                );
                            }
                        }
                        else
                        {
                            Projectile.Kill();
                        }
                    }
                }
                else
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity.Y += 0.15f;

                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position, 
                        Projectile.width, 
                        Projectile.height,
                        DustID.SpectreStaff, 
                        0f, 0f, 100, 
                        Color.Lime, 
                        1.2f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (isStuck)
                return false;

            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!isStuck)
            {
                isStuck = true;
                stuckNPCIndex = target.whoAmI;
                stuckOffset = Projectile.Center - target.Center;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.friendly = false;
                Projectile.netUpdate = true;
                damageTickCounter = 0;
                Projectile.localAI[0] = 0;

                // Hit effect
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDust(
                        Projectile.position, 
                        Projectile.width, 
                        Projectile.height,
                        DustID.SpectreStaff, 
                        hit.HitDirection * 2.5f, 
                        -2.5f, 
                        0, 
                        Color.Gold, 
                        1.2f
                    );
                }

                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.3f;
            }

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.3f;
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height,
                    DustID.SpectreStaff, 
                    Projectile.velocity.X * 0.3f, 
                    Projectile.velocity.Y * 0.3f, 
                    0, 
                    Color.Gold, 
                    1.5f
                );
            }
        }
    }
}