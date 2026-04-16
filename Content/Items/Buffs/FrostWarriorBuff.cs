namespace MightofUniverses.Content.Items.Buffs
{
    public class FrostWarriorBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<FrostWarriorMinion>()] > 0)
                player.buffTime[buffIndex] = 18000;
        }
    }
}