using Terraria.GameInput;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Common.Players
{
    public class AvidityChalicePlayer : ModPlayer
    {
        public bool hasAvidityChalice;
        private bool chaliceActive = false;
        private int chaliceCooldown = 0;
        private int spawnTimer = 0;
        private int totalDrops = 0;
        private const int dropRate = 1;
        private const int maxDrops = 20;

        public override void ResetEffects()
        {
            hasAvidityChalice = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (hasAvidityChalice && chaliceCooldown <= 0 && ModKeybindManager.Ability3.JustPressed)
            {
                chaliceCooldown = 60 * 30; // 30 seconds
                chaliceActive = true;
                spawnTimer = 0;
                totalDrops = 0;
            }
        }

        public override void PostUpdate()
        {
            if (chaliceCooldown > 0)
                chaliceCooldown--;

            if (!hasAvidityChalice)
            {
                chaliceActive = false;
                spawnTimer = 0;
                totalDrops = 0;
                return;
            }

            if (!chaliceActive)
                return;

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (totalDrops >= maxDrops)
            {
                chaliceActive = false;
                return;
            }

            spawnTimer++;
            if (spawnTimer >= dropRate)
            {
                spawnTimer = 0;
                totalDrops++;

                Vector2 offset = new Vector2(Main.rand.NextFloat(-15f, 15f) * 16f, -Main.rand.NextFloat(4f, 10f) * 16f);
                Vector2 spawnPos = Player.Center + offset;
                Vector2 velocity = (Player.Center - spawnPos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(6f, 10f);
                velocity.Y += 2f;

                bool isGreen = Main.rand.NextFloat() < 0.15f;
                int type = isGreen ? ModContent.ProjectileType<GreenChaliceDrop>() : ModContent.ProjectileType<GoldChaliceDrop>();

                Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, velocity, type, 0, 0f, Player.whoAmI);
            }
        }
    }
}
