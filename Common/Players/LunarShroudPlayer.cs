using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class LunarShroudSetPlayer : ModPlayer
    {
        private const int MaxSoulBonus = 400;
        private bool wearingFull;

        public override void ResetEffects()
        {
            wearingFull = false;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.LunarShroudCowl>()
             && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.LunarShroudChestplate>()
             && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.LunarShroudPants>())
            {
                wearingFull = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += MaxSoulBonus;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingFull)
            {
                Player.setBonus =
                    $"+{MaxSoulBonus} max souls\n" +
                    "Your Reaper attacks inflict Lunar Reap for 3 seconds.\n" +
                    "When consuming Souls, you gain Lunar Shroud buff which lasts until you take damage. Consuming Souls also grants +1 Death Mark.";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingFull) return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<LunarShroudBuff>(), int.MaxValue);
                reaper.AddDeathMarks(1);
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (!wearingFull || item == null) return;
            if (item.DamageType != reaperClass) return;
            if (target == null || !target.active) return;

            if (Main.netMode != NetmodeID.MultiplayerClient)
                target.AddBuff(ModContent.BuffType<LunarReap>(), 3 * 60);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (!wearingFull || proj == null) return;
            if (proj.DamageType != reaperClass) return;
            if (target == null || !target.active) return;

            if (Main.netMode != Terraria.ID.NetmodeID.MultiplayerClient)
                target.AddBuff(ModContent.BuffType<LunarReap>(), 3 * 60);
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo info)
        {
            if (!wearingFull || !Player.active || Player.dead) return;
            if (Player.HasBuff(ModContent.BuffType<LunarShroudBuff>()))
            {
                Player.ClearBuff(ModContent.BuffType<LunarShroudBuff>());
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo info)
        {
            if (!wearingFull || !Player.active || Player.dead) return;
            if (Player.HasBuff(ModContent.BuffType<LunarShroudBuff>()))
            {
                Player.ClearBuff(ModContent.BuffType<LunarShroudBuff>());
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (!wearingFull || !Player.active || Player.dead) return;
            if (Player.HasBuff(ModContent.BuffType<LunarShroudBuff>()))
            {
                Player.ClearBuff(ModContent.BuffType<LunarShroudBuff>());
            }
        }
    }
}