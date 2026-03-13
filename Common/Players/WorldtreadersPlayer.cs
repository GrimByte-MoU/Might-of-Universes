using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WorldtreadersPlayer : ModPlayer
    {
        public bool hasWorldtreaders;
        private int flightTime;
        private const int MaxFlightTime = 300;

        public override void ResetEffects()
        {
            hasWorldtreaders = false;
        }

        public override void PostUpdate()
        {
            if (!hasWorldtreaders)
            {
                if (flightTime > 0)
                    flightTime = 0;
                return;
            }

            bool onGround = Player.velocity.Y == 0f && !Player.justJumped;
            bool inLiquid = Player.wet;

            if (onGround || inLiquid)
            {
                flightTime = 0;
            }

            bool wantsToFly = Player.controlJump && !Player.mount.Active;
            bool canFly = flightTime < MaxFlightTime && !inLiquid;

            if (wantsToFly && canFly && !onGround)
            {
                Player.noFallDmg = true;

                if (Player.velocity.Y > 0f)
                {
                    Player.velocity.Y *= 0.85f;
                }

                Player.velocity.Y -= 0.6f;

                if (Player.velocity.Y < -8f)
                {
                    Player.velocity.Y = -8f;
                }

                flightTime++;

                Player.fallStart = (int)(Player.position.Y / 16f);

                SpawnFlightDust();
            }
        }

        private void SpawnFlightDust()
        {
            Vector2 dustPos = new Vector2(
                Player.position.X + Player.width / 2f,
                Player.position.Y + Player.height
            );

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    dustPos - new Vector2(8, 0),
                    16,
                    8,
                    DustID.TerraBlade,
                    0f,
                    3f,
                    100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity.X = Main.rand.NextFloat(-1.5f, 1.5f);
                dust.velocity.Y = Main.rand.NextFloat(2f, 4f);
                dust.fadeIn = 1.2f;
            }

            if (Main.rand.NextBool(3))
            {
                Dust glow = Dust.NewDustPerfect(
                    dustPos,
                    DustID.TerraBlade,
                    new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1f, 3f)),
                    100,
                    default,
                    2.0f
                );
                glow.noGravity = true;
                glow.fadeIn = 1.5f;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (hasWorldtreaders && Player.lavaWet)
            {
                modifiers.FinalDamage *= 0.3125f;
            }
        }
    }
}