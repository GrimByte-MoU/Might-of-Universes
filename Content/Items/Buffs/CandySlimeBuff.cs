using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Buffs
{
    public class CandySlimeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.vanityPet[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<CandySlimeMinion>()] == 0 && player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(player.GetSource_Buff(buffIndex),
                        player.Center,
                        new Vector2(Main.rand.Next(-2, 3), -5),
                        ModContent.ProjectileType<CandySlimeMinion>(),
                        20,
                        0f,
                        player.whoAmI);
                }
            }
        }
    }
}
