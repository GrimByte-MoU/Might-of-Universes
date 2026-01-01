using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BiomeCleanserSpearProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.timeLeft = 120; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 90)
            {
                // Spin in place
                Projectile.rotation += 0.6f;
                Projectile.velocity *= 0.91f;
                if (Projectile.velocity.Length() < 1f)
                    Projectile.velocity = Vector2.Zero;
            }
            else
            {
                if (Projectile.localAI[0] == 0f)
                {
                    NPC nearest = null;
                    float dist = 1000f;
                    for (int k = 0; k < Main.maxNPCs; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this))
                        {
                            float d = Vector2.Distance(Projectile.Center, npc.Center);
                            if (d < dist)
                            {
                                dist = d;
                                nearest = npc;
                            }
                        }
                    }
                    if (nearest != null)
                        Projectile.velocity = (nearest.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 26f;
                    else
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 26f;
                    if (Projectile.velocity.LengthSquared() > 0.01f)
                        Projectile.rotation = Projectile.velocity.ToRotation();

                    Projectile.localAI[0] = 1f;
                }
                if (Projectile.velocity.LengthSquared() > 0.01f)
                    Projectile.rotation = Projectile.velocity.ToRotation();
            }
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.TerraBlade, 0, 0, 120, Color.LimeGreen, 1.2f);
                Main.dust[d].noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 999999;
            modifiers.FinalDamage *= 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 300);
            target.AddBuff(ModContent.BuffType<EnemyBleeding>(), 300);
        }
    }
}