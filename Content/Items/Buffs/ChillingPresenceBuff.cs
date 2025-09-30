namespace MightofUniverses.Content.Items.Buffs
{
    public class ChillingPresenceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ReaperPlayer>().chillingPresence = true;
        }
    }
}