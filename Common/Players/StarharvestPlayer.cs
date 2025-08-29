using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class StarharvestPlayer : ModPlayer
    {
        public bool hasStarharvest;

        public override void ResetEffects()
        {
            hasStarharvest = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasStarharvest && modifiers.DamageType == DamageClass.Summon && Main.rand.NextFloat() < 0.2f)
            {
                Player.Heal((int)(modifiers.FinalDamage.Base * 0.05f));
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, DustID.HealingPlus);
                }
            }
        }
    }
}
