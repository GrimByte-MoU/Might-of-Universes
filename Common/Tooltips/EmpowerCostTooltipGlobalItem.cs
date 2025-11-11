using System;
using System.Collections.Generic;
using System.Globalization;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.Util;
using MightofUniverses.Common.Abstractions; // optional interface for per-item base cost

namespace MightofUniverses.Common.Tooltips
{
    // Replaces {0} in localized tooltips with the player's effective soul cost.
    public class EmpowerCostTooltipGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => false;

        // Optional fallback while migrating items to IHasSoulCost.
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
                ["Ketsumatsu"] = 125f,
                ["CelestialReaper"] = 125f,
                ["NewMoon"] = 150f
            };

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var modItem = item.ModItem;
            if (modItem == null || modItem.Mod?.Name != "MightofUniverses")
                return;

            // Fast path: only proceed if any line contains {0}
            bool hasPlaceholder = false;
            for (int i = 0; i < tooltips.Count; i++)
            {
                var t = tooltips[i].Text;
                if (!string.IsNullOrEmpty(t) && t.IndexOf("{0}", StringComparison.Ordinal) >= 0)
                {
                    hasPlaceholder = true;
                    break;
                }
            }
            if (!hasPlaceholder)
                return;

            // Determine base cost via interface first, then fallback dictionary
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

            for (int i = 0; i < tooltips.Count; i++)
            {
                var line = tooltips[i];
                if (string.IsNullOrEmpty(line.Text))
                    continue;

                if (line.Text.IndexOf("{0}", StringComparison.Ordinal) >= 0)
                {
                    try
                    {
                        line.Text = string.Format(CultureInfo.InvariantCulture, line.Text, effectiveCost);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}