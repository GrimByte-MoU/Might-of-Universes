using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class CakeBlessing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CakeBlessingPlayer>().hasCakeBlessing = true;
        }
    }

    public class CakeBlessingPlayer : ModPlayer
    {
        public bool hasCakeBlessing;
        private int dodgeTimer = 0;

        public override void ResetEffects()
        {
            hasCakeBlessing = false;
        }

        public override void PostUpdate()
        {
            if (dodgeTimer > 0)
                dodgeTimer--;
        }

        public void TriggerDodgeBoost()
        {
            dodgeTimer = 120;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
{
    if (hasCakeBlessing)
    {
        modifiers.ModifyHurtInfo += delegate (ref Player.HurtInfo info)
        {
            Player.immuneTime += 60;
        };
    }
}

public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
{
    if (hasCakeBlessing)
    {
        modifiers.ModifyHurtInfo += delegate (ref Player.HurtInfo info)
        {
            Player.immuneTime += 60;
        };
    }
}


        public override void PostUpdateEquips()
        {
            if (!hasCakeBlessing) return;

            bool hasImmunityFrames = Player.immuneTime > 0;
            if (hasImmunityFrames || dodgeTimer > 0)
            {
                Player.GetCritChance(DamageClass.Generic) += 5f;
                Player.GetDamage(DamageClass.Generic) += 0.10f;
            }
        }
    }
}
