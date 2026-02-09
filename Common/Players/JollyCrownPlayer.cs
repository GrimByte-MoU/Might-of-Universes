using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using System.Collections.Generic;

namespace MightofUniverses.Common.Players
{
    public class JollyCrownPlayer : ModPlayer
    {
        public bool hasJollyCrown = false;
        private int hitCooldown = 0;

        public override void ResetEffects()
        {
            hasJollyCrown = false;
        }

        public override void PostUpdate()
        {
            if (!hasJollyCrown)
                return;

            if (hitCooldown > 0)
                hitCooldown--;

            float auraRadius = 5 * 16f;
            bool inSnow = Player.ZoneSnow;
            float reduction = inSnow ? 0.6f : 0.5f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.boss && !npc.friendly && npc.damage > 0 && Vector2.Distance(npc.Center, Player.Center) <= auraRadius)
                {
                    npc.GetGlobalNPC<JollyCrownNPC>().jollyReduction = reduction;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
{
    if (!hasJollyCrown) return;

    float auraRadius = 5 * 16f;
    bool inSnow = Player.ZoneSnow;
    float reduction = inSnow ? 0.6f : 0.5f;
    int selfDamage = inSnow ? 250 : 100;
    int cooldownTime = inSnow ? 15 : 20;
    if (!npc.boss && Vector2.Distance(npc.Center, Player.Center) <= auraRadius && hitCooldown == 0)
    {
        CombatText.NewText(npc.Hitbox, Color.LightCyan, selfDamage);
        npc.StrikeNPC(new NPC.HitInfo
        {
            Damage = selfDamage,
            Knockback = 0f,
            HitDirection = 0,
            Crit = false
        });
        Player.statLife += (int)(hurtInfo.Damage * reduction);
        CombatText.NewText(Player.Hitbox, Color.Green, (int)(hurtInfo.Damage * reduction));

        hitCooldown = cooldownTime;
    }
}

    }

    public class JollyCrownNPC : GlobalNPC
{
    public float jollyReduction = 0f;

    public override bool InstancePerEntity => true;

    public override void ResetEffects(NPC npc)
    {
        jollyReduction = 0f;
    }

    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        if (jollyReduction > 0f)
        {
            modifiers.SourceDamage *= 1f - jollyReduction;
        }
    }
}

}
