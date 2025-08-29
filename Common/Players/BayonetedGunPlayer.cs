using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    public class BayonetedGunPlayer : ModPlayer
    {
        public bool hasBayonetedGun = false;
        private int bayonetedCooldown = 0;

        public override void ResetEffects()
        {
            hasBayonetedGun = false;
        }

        public override void PostUpdate()
        {
            if (!hasBayonetedGun)
                return;

            if (bayonetedCooldown > 0)
                bayonetedCooldown--;

            Rectangle playerRect = Player.getRect();

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.dontTakeDamage || !npc.Hitbox.Intersects(playerRect))
                    continue;

                if (bayonetedCooldown == 0)
                {
                    int damage = (int)(Player.HeldItem.damage * 1.75f);
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
                    bayonetedCooldown = 15;
                }
            }
        }
    }
}
