using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BriarfangProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public float MovementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);

            if (!projOwner.frozen)
            {
                if (MovementFactor == 0f)
                {
                    MovementFactor = 2.2f;
                    Projectile.netUpdate = true;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    MovementFactor -= 1.6f;
                }
                else
                {
                    MovementFactor += 1.4f;
                }
            }

            Projectile.position += Projectile.velocity * MovementFactor;

            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass);
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 180);

            bool hasFlower = NPCHasFlower(target);

            if (hasFlower)
            {
                Player player = Main.player[Projectile.owner];
                
                for (int i = 0; i < 3; i++)
                {
                    Vector2 toTarget = target.Center - player.Center;
                    toTarget.Normalize();
                    toTarget = toTarget.RotatedByRandom(MathHelper.ToRadians(15));
                    toTarget *= 12f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        player.Center,
                        toTarget,
                        ModContent.ProjectileType<CyberLeaf>(),
                        (int)(Projectile.damage * 0.75f),
                        2f,
                        Projectile.owner,
                        target.whoAmI
                    );
                }

                SoundEngine.PlaySound(SoundID.Grass, target.Center);
            }
            else
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<CyberFlower>(),
                    (int)(Projectile.damage * 0.5f),
                    0f,
                    Projectile.owner,
                    target.whoAmI
                );

                SoundEngine.PlaySound(SoundID.Grass with { Pitch = -0.3f }, target.Center);

                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(target.Center - new Vector2(15, 15), 30, 30, DustID.JunglePlants);
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }
        }

        private bool NPCHasFlower(NPC target)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<CyberFlower>() && proj.ai[0] == target.whoAmI)
                {
                    return true;
                }
            }
            return false;
        }
    }
}