using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CyberFlower : MoUProjectile
    {
        private const int FlowerDuration = 300;
        private const int DamageInterval = 5;
        
        private int damageTimer = 0;

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = FlowerDuration;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            int targetIndex = (int)Projectile.ai[0];

            if (targetIndex < 0 || targetIndex >= Main.maxNPCs || !Main.npc[targetIndex].active || Main.npc[targetIndex].life <= 0)
            {
                Projectile.Kill();
                return;
            }

            NPC target = Main.npc[targetIndex];
            Projectile.Center = target.Center;

            damageTimer++;

            if (damageTimer >= DamageInterval)
            {
                damageTimer = 0;
                DealFlowerDamage(target);
            }

            Projectile.rotation += 0.05f;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.JunglePlants,
                    0f, -2f,
                    100,
                    Color.LimeGreen,
                    1.5f
                );
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(10))
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 offset = Main.rand.NextVector2Circular(Projectile.width * 0.5f, Projectile.height * 0.5f);
                    Dust petal = Dust.NewDustPerfect(
                        Projectile.Center + offset,
                        DustID.JungleSpore,
                        Vector2.Zero,
                        100,
                        Color.Pink,
                        2f
                    );
                    petal.noGravity = true;
                }
            }
        }

        private void DealFlowerDamage(NPC target)
        {
            Player player = Main.player[Projectile.owner];

            if (player.active && !player.dead)
            {
                NPC.HitInfo hitInfo = new NPC.HitInfo
                {
                    Damage = Projectile.damage,
                    Knockback = 0f,
                    HitDirection = 0
                };

                target.StrikeNPC(hitInfo);

                int healAmount = (int)(Projectile.damage * 0.01f);
                if (healAmount < 1) healAmount = 1;
                player.Heal(healAmount);

                int manaAmount = (int)(Projectile.damage * 0.01f);
                if (manaAmount < 1) manaAmount = 1;
                player.statMana += manaAmount;
                if (player.statMana > player.statManaMax2)
                {
                    player.statMana = player.statManaMax2;
                }
                player.ManaEffect(manaAmount);

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendStrikeNPC(target, hitInfo);
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center - new Vector2(10, 10),
                    20, 20,
                    DustID.JunglePlants
                );
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}