using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PrismaticSentinel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            Main.projPet[Projectile.type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 18000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5f;
        }

        private int shootTimer = 0;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }
            float orbitDistance = 48f;
            float orbitSpeed = 0.02f;
            float t = (Main.GameUpdateCount * orbitSpeed) + Projectile.identity;
            Vector2 orbitOffset = new Vector2((float)Math.Cos(t), (float)Math.Sin(t)) * orbitDistance;
            Vector2 targetPos = owner.Center + new Vector2(-20 * owner.direction, -36f) + orbitOffset;
            Vector2 move = (targetPos - Projectile.Center) * 0.18f;
            Projectile.velocity = Projectile.velocity * 0.86f + move * 0.14f;

            Projectile.rotation = Projectile.velocity.ToRotation();

            shootTimer++;
            if (shootTimer >= 30)
            {
                shootTimer = 0;
                int target = -1;
                float bestDist = 1000f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy())
                    {
                        float dist = Vector2.Distance(npc.Center, Projectile.Center);
                        if (dist < bestDist)
                        {
                            bestDist = dist;
                            target = i;
                        }
                    }
                }

                if (target != -1 && bestDist < 800f)
                {
                    Vector2 dir = Vector2.Normalize(Main.npc[target].Center - Projectile.Center);
                    Vector2 vel = dir * 8f;
                    int bolt = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<PrismaticBolt>(), (int)(100 * owner.GetDamage(DamageClass.Summon).Multiplicative), 1f, owner.whoAmI);
                    Main.projectile[bolt].netUpdate = true;
                }
            }
            Projectile.timeLeft = 18000;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}