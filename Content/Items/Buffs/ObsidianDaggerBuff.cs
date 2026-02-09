using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Buffs
{
    public class ObsidianDaggerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int daggerType = ModContent.ProjectileType<ObsidianDaggerMinion>();
            
            if (player.ownedProjectileCounts[daggerType] > 0)
                player.buffTime[buffIndex] = 18000;
            if (player.whoAmI == Main.myPlayer)
            {
                float usedByOthers = player.slotsMinions - (player.ownedProjectileCounts[daggerType] * 0.5f);
                float freeSlots = player.maxMinions - usedByOthers;

                int current = player.ownedProjectileCounts[daggerType];
                int canFit = (int)System.Math.Floor((freeSlots + 1e-4f) / 0.5f);
                int targetCount = System.Math.Max(0, canFit);
                int toSpawn = targetCount - current;
                if (toSpawn > 0)
                {
                    for (int i = 0; i < toSpawn; i++)
                    {
                        Projectile.NewProjectile(
                            player.GetSource_Buff(buffIndex),
                            player.Center,
                            Vector2.Zero,
                            daggerType,
                            player.GetWeaponDamage(player.HeldItem),
                            player.HeldItem.knockBack,
                            player.whoAmI
                        );
                    }
                }
            }
        }
    }
}