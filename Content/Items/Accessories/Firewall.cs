using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Firewall : ModItem
    {
        public const int DASH_DURATION = 15;
        public const int DASH_COOLDOWN = 400;
        public const float DASH_VELOCITY = 10f;
        public const int DASH_DAMAGE = 50;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FirewallPlayer>().hasFirewall = true;
            player.statDefense += 6;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.noKnockback = true;
            player.fireWalk = true;
        }
    }

    public class FirewallPlayer : ModPlayer
    {
        public bool hasFirewall;
        public bool dashing;
        public int dashTimer;
        public int dashCooldown;
        public int dashDirection;
        private bool rightKeyDown;
        private bool leftKeyDown;
        private int rightKeyPressTime;
        private int leftKeyPressTime;
        private int rightKeyReleaseTime;
        private int leftKeyReleaseTime;
        private int rightTapCount;
        private int leftTapCount;
        private const int TAP_WINDOW = 15;

        public override void ResetEffects()
        {
            hasFirewall = false;
        }

        public bool CanUseDash()
        {
            return hasFirewall && Player.dashType == 0 && !Player.mount.Active && dashCooldown <= 0 && !dashing;
        }

        public void StartDash(int direction)
        {
            dashing = true;
            dashTimer = Firewall.DASH_DURATION;
            dashDirection = direction;
            dashCooldown = Firewall.DASH_COOLDOWN;
            SoundEngine.PlaySound(SoundID.Item74, Player.Center);
        }

        public override void PreUpdate()
        {
            if (dashCooldown > 0)
            {
                dashCooldown--;
            }

            if (dashTimer > 0)
            {
                dashTimer--;
                if (dashTimer <= 0)
                {
                    dashing = false;
                }
            }

            HandleDoubleTapDetection();
        }
        
        private void HandleDoubleTapDetection()
        {
            bool rightKeyPressed = Player.controlRight && !Player.controlLeft;
            if (rightKeyPressed && !rightKeyDown)
            {
                rightKeyDown = true;
                rightKeyPressTime = 0;

                if (rightKeyReleaseTime < TAP_WINDOW)
                {
                    rightTapCount++;
                    if (rightTapCount >= 2 && CanUseDash())
                    {
                        StartDash(1);
                        rightTapCount = 0;
                    }
                }
                else
                {
                    rightTapCount = 1;
                }
            }
            else if (!rightKeyPressed && rightKeyDown)
            {
                rightKeyDown = false;
                rightKeyReleaseTime = 0;
            }

            bool leftKeyPressed = Player.controlLeft && !Player.controlRight;
            if (leftKeyPressed && !leftKeyDown)
            {
                leftKeyDown = true;
                leftKeyPressTime = 0;

                if (leftKeyReleaseTime < TAP_WINDOW)
                {
                    leftTapCount++;
                    if (leftTapCount >= 2 && CanUseDash())
                    {
                        StartDash(-1);
                        leftTapCount = 0;
                    }
                }
                else
                {
                    leftTapCount = 1;
                }
            }
            else if (!leftKeyPressed && leftKeyDown)
            {
                leftKeyDown = false;
                leftKeyReleaseTime = 0;
            }

            if (rightKeyDown)
            {
                rightKeyPressTime++;
            }
            else
            {
                rightKeyReleaseTime++;
            }
            
            if (leftKeyDown)
            {
                leftKeyPressTime++;
            }
            else
            {
                leftKeyReleaseTime++;
            }

            if (rightKeyReleaseTime > TAP_WINDOW * 2)
            {
                rightTapCount = 0;
            }
            
            if (leftKeyReleaseTime > TAP_WINDOW * 2)
            {
                leftTapCount = 0;
            }
        }

        public override void PreUpdateMovement()
        {
            if (dashing)
            {
                Player.immune = true;
                Player.immuneTime = 6;
                Player.velocity.X = Firewall.DASH_VELOCITY * dashDirection;

                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        Player.position, 
                        Player.width, 
                        Player.height,
                        DustID.GreenTorch, 
                        0f, 
                        0f, 
                        100, 
                        default, 
                        1.5f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                }

                Rectangle hitbox = Player.Hitbox;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && hitbox.Intersects(npc.Hitbox))
                    {
                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = Firewall.DASH_DAMAGE,
                            Knockback = 8f,
                            HitDirection = dashDirection
                        });
                        
                        SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                        
                        for (int d = 0; d < 20; d++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                npc.position, 
                                npc.width, 
                                npc.height,
                                DustID.GreenTorch, 
                                0f, 
                                0f, 
                                100, 
                                default, 
                                2f
                            );
                            dust.noGravity = true;
                            dust.velocity *= 2f;
                        }
                    }
                }
            }
        }
    }
}