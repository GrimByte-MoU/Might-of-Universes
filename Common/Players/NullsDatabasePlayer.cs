using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Common.Players;
public class NullsDatabasePlayer : ModPlayer
{
    public bool hasNullsDatabase;
    private int healCooldown;

    public override void ResetEffects()
    {
        hasNullsDatabase = false;
    }

    public override void PostUpdate()
    {
        if (healCooldown > 0)
            healCooldown--;

        if (hasNullsDatabase && healCooldown <= 0 && Player.statLife <= Player.statLifeMax2 * 0.5f)
        {
            Player.statLife += 150;
            Player.Heal(150);
            
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (Main.debuff[Player.buffType[i]] && !BuffID.Sets.NurseCannotRemoveDebuff[Player.buffType[i]])
                {
                    Player.DelBuff(i);
                    i--;
                }
            }
            
            healCooldown = 1800;
        }
    }
}
