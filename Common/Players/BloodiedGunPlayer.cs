using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    public class BloodiedGunPlayer : ModPlayer
    {
        public bool hasBloodiedGun = false;
        public bool isFiringBloodiedPlatedGun = false;
        public bool isFiringLunacyApexGun = false;

        private int bloodiedCooldown = 0;

        public override void ResetEffects()
        {
            hasBloodiedGun = false;
            isFiringBloodiedPlatedGun = false;
            isFiringLunacyApexGun = false;
        }

        public override void PostUpdateEquips()
        {
            if (isFiringLunacyApexGun)
            {
                Player.statDefense += 7;
                Player.endurance += 0.05f; // 5% damage reduction
            }
            else if (isFiringBloodiedPlatedGun)
            {
                Player.statDefense += 5;
                Player.endurance += 0.03f; // 3% damage reduction
            }
        }

        public override void PostUpdate()
        {
            if (!hasBloodiedGun)
                return;

            if (bloodiedCooldown > 0)
                bloodiedCooldown--;

            Rectangle playerRect = Player.getRect();

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.dontTakeDamage || !npc.Hitbox.Intersects(playerRect))
                    continue;

                if (bloodiedCooldown == 0)
                {
                    int damage = (int)(Player.HeldItem.damage * 2.5f);
                    float knockback = 6f;
                    Vector2 knockDir = (npc.Center - Player.Center).SafeNormalize(Vector2.UnitX);

                    npc.StrikeNPC(new NPC.HitInfo()
                    {
                        Damage = damage,
                        Knockback = knockback,
                        HitDirection = knockDir.X > 0 ? 1 : -1,
                        Crit = false,
                        DamageType = DamageClass.Ranged
                    }, false);

                    CombatText.NewText(npc.Hitbox, CombatText.DamagedFriendly, damage);

                    // Heal the player for 5 HP
                    int healAmount = 5;
                    Player.statLife += healAmount;
                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;
                    Player.HealEffect(healAmount, true);

                    bloodiedCooldown = 20;
                }
            }
        }
    }
}
