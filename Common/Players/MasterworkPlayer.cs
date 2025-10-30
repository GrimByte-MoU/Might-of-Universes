using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Input;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class MasterworkPlayer : ModPlayer
    {
        public bool hasMasterworkSet = false;
        private int abilityCooldown = 0;

        public override void ResetEffects()
        {
            hasMasterworkSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasMasterworkSet) return;
            Player.GetDamage(DamageClass.Generic) *= 0.70f;
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.80f;
        }

        public override void PostUpdate()
        {
            if (!hasMasterworkSet) return;

            if (abilityCooldown > 0)
            {
                abilityCooldown--;
            }

            if (ModKeybindManager.ArmorAbility != null && 
                ModKeybindManager.ArmorAbility.JustPressed && 
                abilityCooldown <= 0)
            {
                ActivateAbility();
            }

            Lighting.AddLight(Player.Center, 0.9f, 0.7f, 0.2f);

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Copper, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(3))
            {
                Color color = Main.rand.NextBool() ? Color.Gold : Color.Silver;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, color, 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
            }

            if (Main.rand.NextBool(6))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Electric, 0f, 0f, 100, default, 0.5f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.velocity *= 2f;
            }
        }

        private void ActivateAbility()
        {
            abilityCooldown = 600;

            if (Main.myPlayer == Player.whoAmI)
            {
                for (int i = 0; i < 15; i++)
                {
                    Vector2 randomVelocity = Main.rand.NextVector2Circular(8f, 8f);
                    
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        randomVelocity,
                        ModContent.ProjectileType<TwirlyWhirly>(),
                        100,
                        2f,
                        Player.whoAmI
                    );
                }
            }

            for (int i = 0; i < 30; i++)
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 200, 100),
                    1 => new Color(200, 200, 200),
                    _ => new Color(150, 100, 50)
                };

                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                Dust dust = Dust.NewDustPerfect(Player.Center, DustID.Copper, velocity, 100, dustColor, 1.5f);
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, Player.Center);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasMasterworkSet) return;
            if (proj.DamageType != ModContent.GetInstance<Common.PacifistDamageClass>()) return;

            target.AddBuff(ModContent.BuffType<Shred>(), 180);
        }
    }
}