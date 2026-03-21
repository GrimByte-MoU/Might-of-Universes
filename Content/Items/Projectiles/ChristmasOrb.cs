using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using System.Collections.Generic;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ChristmasOrb : MoUProjectile
    {
        private HashSet<int> hitNPCs = new HashSet<int>();
        private int damageTimer = 0;
        private int currentTargetNPC = -1;

        public override void SafeSetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.LimeGreen.ToVector3() * 1.5f);
            
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);
            }

            if (currentTargetNPC != -1 && damageTimer < 60)
            {
                NPC target = Main.npc[currentTargetNPC];
                if (target != null && target.active)
                {
                    damageTimer++;
                    if (damageTimer % 1 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        target.lifeRegen -= 200;
                        
                        target.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = 2,
                            Knockback = 0f,
                            HitDirection = 0,
                            Crit = false
                        });

                        if (damageTimer % 10 == 0)
                        {
                            Dust dust = Dust.NewDustDirect(
                                target.position,
                                target.width,
                                target.height,
                                DustID.GreenTorch,
                                0f,
                                0f,
                                100,
                                default,
                                1.2f
                            );
                            dust.noGravity = true;
                        }
                    }
                }

                if (damageTimer >= 60)
                {
                    Projectile.Kill();
                }
                return;
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.Hitbox.Intersects(Projectile.Hitbox))
                {
                    if (!hitNPCs.Contains(npc.whoAmI))
                    {
                        hitNPCs.Add(npc.whoAmI);
                        currentTargetNPC = npc.whoAmI;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.StrikeNPC(new NPC.HitInfo
                            {
                                Damage = 125,
                                Knockback = 0f,
                                HitDirection = 0,
                                Crit = false
                            });
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.GreenTorch,
                                Main.rand.NextFloat(-3f, 3f),
                                Main.rand.NextFloat(-3f, 3f),
                                100,
                                default,
                                1.5f
                            );
                            dust.noGravity = true;
                        }

                        damageTimer = 0;
                        Projectile.velocity = Vector2.Zero;
                        Projectile.position = npc.Center - new Vector2(Projectile.width / 2, Projectile.height / 2); // Stick to enemy
                        return;
                    }
                }
            }

            Player player = Main.player[Projectile.owner];
            if (player.active && !player.dead && player.Hitbox.Intersects(Projectile.Hitbox))
            {
                for (int i = 0; i < 60; i++)
                {
                    player.lifeRegen += 50;
                    player.GetModPlayer<ReaperPlayer>().soulEnergy += 10f / 60f;
                }

                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        player.position,
                        player.width,
                        player.height,
                        DustID.GreenTorch,
                        Main.rand.NextFloat(-4f, 4f),
                        Main.rand.NextFloat(-4f, 4f),
                        100,
                        default,
                        2f
                    );
                    dust.noGravity = true;
                }

                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GreenTorch,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100,
                    default,
                    2.5f
                );
                dust.noGravity = true;
            }
        }
    }
}