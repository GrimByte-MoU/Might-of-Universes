namespace MightofUniverses.Content.Items.Buffs
{
    public class BattleCryBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.20f;
            player.moveSpeed += 0.35f;
        }
    }
}