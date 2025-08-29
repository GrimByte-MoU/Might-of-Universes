using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MiniAegis : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 18000;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                owner.ClearBuff(ModContent.BuffType<MiniAegisBuff>());
                return;
            }
            if (owner.HasBuff(ModContent.BuffType<MiniAegisBuff>()))
                Projectile.timeLeft = 2;

            // Follow the player
            Vector2 idlePos = owner.Center + new Vector2(50f * owner.direction, -30f);
            Projectile.velocity = (idlePos - Projectile.Center) * 0.1f;

            // Blocking hostile projectiles
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile hostileProj = Main.projectile[i];
                if (hostileProj.active && hostileProj.hostile && !hostileProj.minion &&
                    Projectile.Hitbox.Intersects(hostileProj.Hitbox))
                {
                    int pierceBlocked = (int)(Projectile.localAI[1] == 0 ? 1 : Projectile.localAI[1]);
                    if (hostileProj.penetrate > 0 && hostileProj.penetrate <= pierceBlocked)
                    {
                        hostileProj.Kill();
                    }
                    else
                    {
                        hostileProj.penetrate -= pierceBlocked;
                        if (hostileProj.penetrate <= 0)
                            hostileProj.Kill();
                    }
                }
            }

            // Knockback enemies
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && Projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    Vector2 knockDir = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 8f;
                    npc.velocity += knockDir;
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = Projectile.damage,
                        Knockback = Projectile.knockBack,
                        HitDirection = owner.direction
                    });
                }
            }
        }
    }
}
