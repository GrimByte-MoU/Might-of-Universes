using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FloatingHeartCrystalMage : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 999999;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Hovering bobbing motion above player
            Vector2 hoverOffset = new Vector2(0f, -60f + (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 5f);
            Projectile.Center = player.Center + hoverOffset;
            //Projectile.rotation += 0.02f;

            // Heal + mana restore every 2 seconds (every 120 ticks)
            if (++Projectile.localAI[0] >= 120)
            {
                Projectile.localAI[0] = 0;

                if (Main.rand.NextBool(2))
                {
                    bool healed = false;

                    if (player.statLife < player.statLifeMax2)
                    {
                        player.statLife += 15;
                        if (player.statLife > player.statLifeMax2)
                            player.statLife = player.statLifeMax2;
                        player.HealEffect(15, true);
                        healed = true;
                    }

                    if (player.statMana < player.statManaMax2)
                    {
                        player.statMana += 40;
                        if (player.statMana > player.statManaMax2)
                            player.statMana = player.statManaMax2;
                        CombatText.NewText(player.Hitbox, new Color(100, 255, 200), "Mana Restored", true);
                        healed = true;
                    }

                    if (healed)
                    {
                        SoundEngine.PlaySound(SoundID.Item4, player.Center);
                    }
                }
            }
        }

        public override bool? CanHitNPC(NPC target) => false;
        public override bool CanHitPlayer(Player target) => false;
    }
}
