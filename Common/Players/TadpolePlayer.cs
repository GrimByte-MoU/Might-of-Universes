using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;

namespace MightofUniverses.Common.Players
{
    public class TadpolePlayer : ModPlayer
    {
        public bool hasPsychicTadpole;
        public bool hasStellarTadpole;
        
        // Visual effects
        private int dustTimer;

        public override void ResetEffects()
        {
            hasPsychicTadpole = false;
            hasStellarTadpole = false;
        }

        public override void PostUpdateEquips()
        {
            // Create visual effects when flying with wings
            if ((hasPsychicTadpole || hasStellarTadpole) && Player.wingTime < Player.wingTimeMax)
            {
                CreateFlightVisuals();
            }
        }
        
        private void CreateFlightVisuals()
        {
            // Create dust effects every few frames
            dustTimer++;
            if (dustTimer >= 3)
            {
                dustTimer = 0;
                
                // Determine dust color based on which tadpole is equipped
                Color dustColor = hasStellarTadpole ? 
                    new Color(180, 120, 255) : // Purple for Stellar Tadpole
                    new Color(255, 120, 180);  // Pink for Psychic Tadpole
                
                // Create dust at player's feet
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        new Vector2(Player.position.X, Player.position.Y + Player.height - 4),
                        Player.width,
                        8,
                        DustID.Pixie,
                        Player.velocity.X * 0.2f,
                        Player.velocity.Y * 0.2f,
                        100,
                        dustColor,
                        hasStellarTadpole ? 1.2f : 0.8f
                    );
                    dust.noGravity = true;
                    dust.velocity.X *= 0.3f;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y) * 0.5f - 1f;
                }
            }
        }
    }
}

