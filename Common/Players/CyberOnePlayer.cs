using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Input;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class CyberOnePlayer : ModPlayer
    {
        public bool hasCyberOneSet = false;
        private int teleportCooldown = 0;

        public override void ResetEffects()
        {
            hasCyberOneSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasCyberOneSet) return;
            
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 1f;
            
            int wingSlot = ModContent.GetInstance<CyberOneWings>().Item.wingSlot;
            Player.wings = wingSlot;
            Player.wingsLogic = wingSlot;
            Player.wingTimeMax = 600;
        }

        public override void PostUpdate()
        {
            if (hasCyberOneSet)
            {
                if (Main.rand.NextBool(5))
                {
                    Dust dust = Dust.NewDustDirect(
                        Player.position, 
                        Player.width, 
                        Player.height, 
                        DustID.Electric, 
                        0f, 
                        0f, 
                        100, 
                        Color.Cyan, 
                        0.8f
                    );
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                }

                if (ModKeybindManager.ArmorAbility != null && 
                    ModKeybindManager.ArmorAbility.JustPressed &&
                    teleportCooldown <= 0)
                {
                    TeleportToCursor();
                }
            }

            if (teleportCooldown > 0)
                teleportCooldown--;
        }

        private void TeleportToCursor()
        {
            Vector2 oldPosition = Player.Center;
            Vector2 newPosition = Main.MouseWorld;

            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Player.position,
                    Player.width,
                    Player.height,
                    DustID.Electric,
                    0f,
                    0f,
                    100,
                    Color.Cyan,
                    2f
                );
                dust.noGravity = true;
                dust.velocity *= 3f;
            }

            Player.Teleport(newPosition, 1);
            Player.velocity = Vector2.Zero;

            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Player.position,
                    Player.width,
                    Player.height,
                    DustID.Electric,
                    0f,
                    0f,
                    100,
                    Color.Cyan,
                    2f
                );
                dust.noGravity = true;
                dust.velocity *= 3f;
            }

            if (Main.myPlayer == Player.whoAmI)
            {
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    oldPosition,
                    Vector2.Zero,
                    ModContent.ProjectileType<EMPTrap>(),
                    750,
                    5f,
                    Player.whoAmI
                );
            }

            SoundEngine.PlaySound(SoundID.Item8, Player.Center);
            teleportCooldown = 300;
        }
    }
}