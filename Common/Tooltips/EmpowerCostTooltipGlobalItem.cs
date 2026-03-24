using System;
using System.Collections.Generic;
using System.Globalization;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.Util;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.Tooltips
{
    public class EmpowerCostTooltipGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => false;

        private static readonly Dictionary<string, float> BaseSoulCosts =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Bloodletter"] = 50f,
                ["CopperScythe"] = 25f,
                ["Evildoer"] = 30f,
                ["GoldScythe"] = 40f,
                ["GummyScythe"] = 30f,
                ["HellsTalon"] = 40f,
                ["IronScythe"] = 30f,
                ["LeadScythe"] = 30f,
                ["MeteoriteHarvester"] = 40f,
                ["MidnightsReap"] = 50f,
                ["PlatinumScythe"] = 40f,
                ["SolunarScythe"] = 50f,
                ["SweetHarvester"] = 75f,
                ["TinScythe"] = 25f,
                ["WardensHook"] = 25f,
                ["PalladiumScythe"] = 70f,
                ["ChlorophyteScythe"] = 140f,
                ["Orcus"] = 30f,
                ["OrichalcumScythe"] = 40f,
                ["TitaniumScythe"] = 75f,
                ["AdamantiteScythe"] = 40f,
                ["BloodTithe"] = 100f,
                ["CobaltScythe"] = 40f,
                ["CriticalFailure"] = 35f,
                ["DemonsFinger"] = 40f,
                ["GlitchScythe"] = 40f,
                ["MythrilScythe"] = 40f,
                ["ChlorotaniumScythe"] = 200f,
                ["DebtCollector"] = 50f,
                ["GreatBlizzard88"] = 50f,
                ["LifesTwilight"] = 50f,
                ["TempleReaper"] = 220f,
                ["TrickstersDue"] = 300f,
                ["MartianTally"] = 70f,
                ["AncientBoneScythe"] = 100f,
                ["Ketsumatsu"] = 275f,
                ["CelestialReaper"] = 200f,
                ["NewMoon"] = 260f,
                ["ReapersEcho"] = 450f,
                ["IceAge"] = 275f,
                ["Kasurikama"] = 60f,
                ["CycloneKama"] = 200f,
                ["Pompeii"] = 300f,
                ["BiomeCleanser"] = 255f,
                ["Aokigahara"] = 275f,
            };

        private static readonly Dictionary<string, int> EmpowermentDurations =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["CopperScythe"] = 180,
                ["TinScythe"] = 180,
                ["IronScythe"] = 180,
                ["LeadScythe"] = 180,
                ["SilverScythe"] = 180,
                ["TungstenScythe"] = 180,
                ["GoldScythe"] = 240,
                ["PlatinumScythe"] = 240,
                ["Orcus"] = 180,
                ["NewMoon"] = 300,
                ["Aokigahara"] = 240,
                ["ReapersEcho"] = 300,
            };

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var modItem = item.ModItem;
            if (modItem == null || modItem.Mod?.Name != "MightofUniverses")
                return;

            bool hasPlaceholder = false;
            for (int i = 0; i < tooltips.Count; i++)
            {
                var t = tooltips[i].Text;
                if (!string.IsNullOrEmpty(t) && 
                    (t.IndexOf("{0}", StringComparison.Ordinal) >= 0 || 
                     t.IndexOf("{1}", StringComparison.Ordinal) >= 0))
                {
                    hasPlaceholder = true;
                    break;
                }
            }
            if (!hasPlaceholder)
                return;

            float baseCost;
            if (modItem is IHasSoulCost hasCost)
            {
                baseCost = hasCost.BaseSoulCost;
            }
            else if (!BaseSoulCosts.TryGetValue(modItem.Name, out baseCost))
            {
                return;
            }

            var player = Main.LocalPlayer;
            if (player is null)
                return;

            int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, baseCost);

            int baseDuration = 0;
            if (modItem is IHasSoulCost hasCost2)
            {
                baseDuration = hasCost2.EmpowermentDurationTicks;
            }
            else
            {
                EmpowermentDurations.TryGetValue(modItem.Name, out baseDuration);
            }

            float effectiveDuration = 0f;
            if (baseDuration > 0)
            {
                var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
                int finalDuration = baseDuration;
                
                if (acc.EmpowerDurationMultiplier > 1f)
                    finalDuration = (int)(finalDuration * acc.EmpowerDurationMultiplier);
                
                if (acc.EmpowerExtraDurationTicks > 0)
                    finalDuration += acc.EmpowerExtraDurationTicks;

                effectiveDuration = finalDuration / 60f;
            }

            for (int i = 0; i < tooltips.Count; i++)
            {
                var line = tooltips[i];
                if (string.IsNullOrEmpty(line.Text))
                    continue;

                if (line.Text.IndexOf("{0}", StringComparison.Ordinal) >= 0 ||
                    line.Text.IndexOf("{1}", StringComparison.Ordinal) >= 0)
                {
                    try
                    {
                        line.Text = string.Format(
                            CultureInfo.InvariantCulture, 
                            line.Text, 
                            effectiveCost,      
                            effectiveDuration   
                        );
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}