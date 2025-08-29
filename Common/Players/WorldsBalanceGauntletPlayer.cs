using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class WorldsBalanceGauntletPlayer : ModPlayer
    {
        public bool hasWorldsBalanceGauntlet;

        public override void ResetEffects()
        {
            hasWorldsBalanceGauntlet = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasWorldsBalanceGauntlet && hit.DamageType == DamageClass.Melee)
            {
                target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);
                CreateElementalParticles();
            }
        }

        private void CreateElementalParticles()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 position = Player.Center;
                float radius = 48f;
                float rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
                Vector2 dustPos = position + new Vector2(radius, 0).RotatedBy(rotation);
                
                int[] dustTypes = new int[] { 
                    DustID.TerraBlade,
                    DustID.BlueTorch,
                    DustID.Ice,
                    DustID.GreenTorch
                };

                Dust dust = Dust.NewDustPerfect(dustPos, Main.rand.Next(dustTypes));
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity = Vector2.Zero;
            }
        }
    }
}

