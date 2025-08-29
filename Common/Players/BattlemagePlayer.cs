using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Accessories;
using Microsoft.Xna.Framework;

    public class BattlemagePlayer : ModPlayer
    {
        public bool battlemageEffect;

        public override void ResetEffects()
        {
            battlemageEffect = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (battlemageEffect && item.DamageType == DamageClass.Melee)
            {
                Player.manaRegen += 10;
            }
        }
    }
