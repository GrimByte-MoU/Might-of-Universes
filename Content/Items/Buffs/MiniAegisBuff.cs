using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class MiniAegisBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player. ownedProjectileCounts[ModContent.ProjectileType<Projectiles.MiniAegis>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        // THIS ADDS THE COUNTER
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            
            // Find the Mini Aegis projectile
            int empowerLevel = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player. whoAmI && 
                    proj.type == ModContent.ProjectileType<Projectiles.MiniAegis>())
                {
                    empowerLevel = (int)proj.localAI[0];
                    break;
                }
            }
            
            int totalSlots = 1 + empowerLevel;
            tip = $"Mini Aegis protecting you\nMinion Slots: {totalSlots}";
        }
    }
}