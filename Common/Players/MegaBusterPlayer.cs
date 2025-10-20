using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class MegaBusterPlayer : ModPlayer
    {
        public bool isChargeMode = false;
        
        private int chargeTime = 0;
        private const int ChargeLevel1Time = 90;
        private const int ChargeLevel2Time = 180;
        
        private bool isCharging = false;
        private int chargeLevel = 0;

        public override void PostUpdate()
        {
            bool holdingMegaBuster = Player.HeldItem.type == ModContent.ItemType<Content.Items.Weapons.MegaBuster>();
            
            if (!holdingMegaBuster || !isChargeMode)
            {
                ResetCharge();
                return;
            }

            if (Player.channel)
            {
                isCharging = true;
                chargeTime++;
                
                if (chargeTime >= ChargeLevel2Time)
                {
                    chargeLevel = 2;
                }
                else if (chargeTime >= ChargeLevel1Time)
                {
                    chargeLevel = 1;
                }
                else
                {
                    chargeLevel = 0;
                }
                
                UpdateChargeVisuals();
            }
            else if (isCharging)
            {
                isCharging = false;
            }
        }

        private void UpdateChargeVisuals()
        {
            if (chargeLevel == 1 && chargeTime % 5 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float angle = (i / 3f) * MathHelper.TwoPi + (chargeTime * 0.1f);
                    float distance = 50f;
                    Vector2 position = Player.Center + new Vector2(
                        (float)System.Math.Cos(angle) * distance,
                        (float)System.Math.Sin(angle) * distance
                    );
                    
                    Dust dust = Dust.NewDustPerfect(position, DustID.GreenFairy, Vector2.Zero, 0, Color.Lime, 2.5f);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }
                
                Lighting.AddLight(Player.Center, 0.5f, 1.5f, 0.5f);
            }
            else if (chargeLevel == 2 && chargeTime % 5 == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    float angle = (i / 4f) * MathHelper.TwoPi + (chargeTime * 0.15f);
                    float distance = 60f;
                    Vector2 position = Player.Center + new Vector2(
                        (float)System.Math.Cos(angle) * distance,
                        (float)System.Math.Sin(angle) * distance
                    );
                    
                    Dust dust = Dust.NewDustPerfect(position, DustID.Torch, Vector2.Zero, 0, Color.Orange, 3.0f);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }
                
                for (int i = 0; i < 2; i++)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float distance = Main.rand.NextFloat(30f, 60f);
                    Vector2 position = Player.Center + new Vector2(
                        (float)System.Math.Cos(angle) * distance,
                        (float)System.Math.Sin(angle) * distance
                    );
                    
                    Dust dust = Dust.NewDustPerfect(position, DustID.YellowTorch, Vector2.Zero, 0, Color.Yellow, 2.0f);
                    dust.noGravity = true;
                }
                
                Lighting.AddLight(Player.Center, 1.5f, 1.0f, 0.3f);
            }
            
            if (chargeTime == ChargeLevel1Time)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                
                for (int i = 0; i < 30; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                    Dust dust = Dust.NewDustPerfect(Player.Center, DustID.GreenFairy, velocity, 0, Color.Lime, 2.0f);
                    dust.noGravity = true;
                }
            }
            else if (chargeTime == ChargeLevel2Time)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.3f }, Player.Center);
                
                for (int i = 0; i < 50; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                    Color dustColor = Main.rand.NextBool() ? Color.Orange : Color.Yellow;
                    int dustType = Main.rand.NextBool() ? DustID.Torch : DustID.YellowTorch;
                    Dust dust = Dust.NewDustPerfect(Player.Center, dustType, velocity, 0, dustColor, 2.5f);
                    dust.noGravity = true;
                }
            }
        }

        public void ReleaseCharge(Player player, Vector2 position, Vector2 velocity)
        {
            if (!isCharging && chargeTime == 0)
                return;

            int projectileType;
            int damage;
            float scale;
            
            if (chargeLevel == 0)
            {
                projectileType = ModContent.ProjectileType<MegaBusterWeakCharge>();
                damage = (int)(player.HeldItem.damage * 0.5f);
                scale = 1.0f;
            }
            else if (chargeLevel == 1)
            {
                projectileType = ModContent.ProjectileType<MegaBusterChargeLevel1>();
                damage = (int)(player.HeldItem.damage * 6f);
                scale = 1.5f;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, player.Center);
            }
            else
            {
                projectileType = ModContent.ProjectileType<MegaBusterChargeLevel2>();
                damage = (int)(player.HeldItem.damage * 12f);
                scale = 2.5f;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62, player.Center);
            }
            
            Projectile.NewProjectile(
                player.GetSource_ItemUse(player.HeldItem),
                position,
                velocity,
                projectileType,
                damage,
                player.HeldItem.knockBack * (1 + chargeLevel),
                player.whoAmI,
                ai0: scale
            );
            
            ResetCharge();
        }

        private void ResetCharge()
        {
            isCharging = false;
            chargeTime = 0;
            chargeLevel = 0;
        }
    }
}