namespace MightofUniverses.Content.Items.Buffs
{
    public class RalliedBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 15;
            player.endurance += 0.10f;
        }
    }
}