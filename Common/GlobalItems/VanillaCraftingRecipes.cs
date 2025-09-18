using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace VanillaCraftingRecipes
{
    public class VanillaCraftingRecipesSystem : ModSystem
    {
        // RecipeGroup identifiers
        public const string CopperTinGroup = "VanillaCraftingRecipes:CopperOrTinBar";
        public const string IronLeadGroup = "VanillaCraftingRecipes:IronOrLeadBar";
        public const string SilverTungstenGroup = "VanillaCraftingRecipes:SilverOrTungstenBar";
        public const string GoldPlatinumGroup = "VanillaCraftingRecipes:GoldOrPlatinumBar";
        public const string AnyFishGroup = "VanillaCraftingRecipes:AnyFishBasic";

        // Added for newer recipes (Hardmode alt metals)
        public const string CobaltPalladiumGroup = "VanillaCraftingRecipes:CobaltOrPalladiumBar";
        public const string MythrilOrichalcumGroup = "VanillaCraftingRecipes:MythrilOrOrichalcumBar";

        public override void AddRecipeGroups()
        {
            string any = Language.GetTextValue("LegacyMisc.37"); // "Any"

            // Copper / Tin
            RecipeGroup copperTin = new RecipeGroup(
                () => $"{any} Copper/Tin Bar",
                ItemID.CopperBar,
                ItemID.TinBar
            );
            RecipeGroup.RegisterGroup(CopperTinGroup, copperTin);

            // Iron / Lead
            RecipeGroup ironLead = new RecipeGroup(
                () => $"{any} Iron/Lead Bar",
                ItemID.IronBar,
                ItemID.LeadBar
            );
            RecipeGroup.RegisterGroup(IronLeadGroup, ironLead);

            // Silver / Tungsten
            RecipeGroup silverTungsten = new RecipeGroup(
                () => $"{any} Silver/Tungsten Bar",
                ItemID.SilverBar,
                ItemID.TungstenBar
            );
            RecipeGroup.RegisterGroup(SilverTungstenGroup, silverTungsten);

            // Gold / Platinum
            RecipeGroup goldPlatinum = new RecipeGroup(
                () => $"{any} Gold/Platinum Bar",
                ItemID.GoldBar,
                ItemID.PlatinumBar
            );
            RecipeGroup.RegisterGroup(GoldPlatinumGroup, goldPlatinum);

            // Any Fish (basic common catches; expand as desired)
            RecipeGroup anyFish = new RecipeGroup(
                () => $"{any} Fish",
                ItemID.Bass,
                ItemID.Trout,
                ItemID.Salmon,
                ItemID.AtlanticCod,
                ItemID.RedSnapper,
                ItemID.Tuna,
                ItemID.NeonTetra
                // Add others if you want them to count, e.g. ItemID.ArmoredCavefish, ItemID.Damselfish, etc.
            );
            RecipeGroup.RegisterGroup(AnyFishGroup, anyFish);

            // New Hardmode bar alternates for added recipes
            RecipeGroup cobaltPalladium = new RecipeGroup(
                () => $"{any} Cobalt/Palladium Bar",
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            );
            RecipeGroup.RegisterGroup(CobaltPalladiumGroup, cobaltPalladium);

            RecipeGroup mythrilOrichalcum = new RecipeGroup(
                () => $"{any} Mythril/Orichalcum Bar",
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            );
            RecipeGroup.RegisterGroup(MythrilOrichalcumGroup, mythrilOrichalcum);
        }

        public override void AddRecipes()
        {
            // ---------------------------
            // Existing Cell Phone set
            // ---------------------------

            // Depth Meter
            Recipe depth = Recipe.Create(ItemID.DepthMeter);
            depth.AddRecipeGroup(CopperTinGroup, 3);
            depth.AddRecipeGroup(IronLeadGroup, 3);
            depth.AddIngredient(ItemID.Chain);
            depth.AddTile(TileID.TinkerersWorkbench);
            depth.Register();

            // Compass
            Recipe compass = Recipe.Create(ItemID.Compass);
            compass.AddRecipeGroup(IronLeadGroup, 8);
            compass.AddIngredient(ItemID.Lens);
            compass.AddIngredient(ItemID.Cloud, 5);
            compass.AddTile(TileID.TinkerersWorkbench);
            compass.Register();

            // Radar
            Recipe radar = Recipe.Create(ItemID.Radar);
            radar.AddRecipeGroup(IronLeadGroup, 5);
            radar.AddIngredient(ItemID.Wire, 10);
            radar.AddIngredient(ItemID.Lens);
            radar.AddTile(TileID.TinkerersWorkbench);
            radar.Register();

            // Tally Counter
            Recipe tally = Recipe.Create(ItemID.TallyCounter);
            tally.AddIngredient(ItemID.Bone, 15);
            tally.AddRecipeGroup(IronLeadGroup, 5);
            tally.AddTile(TileID.TinkerersWorkbench);
            tally.Register();

            // Lifeform Analyzer
            Recipe lifeform = Recipe.Create(ItemID.LifeformAnalyzer);
            lifeform.AddRecipeGroup(GoldPlatinumGroup, 5);
            lifeform.AddRecipeGroup(IronLeadGroup, 5);
            lifeform.AddIngredient(ItemID.Wire, 15);
            lifeform.AddTile(TileID.TinkerersWorkbench);
            lifeform.Register();

            // Stopwatch
            Recipe stopwatch = Recipe.Create(ItemID.Stopwatch);
            stopwatch.AddRecipeGroup(GoldPlatinumGroup, 5);
            stopwatch.AddIngredient(ItemID.Wire, 15);
            stopwatch.AddTile(TileID.TinkerersWorkbench);
            stopwatch.Register();

            // DPS Meter
            Recipe dpsMeter = Recipe.Create(ItemID.DPSMeter);
            dpsMeter.AddIngredient(ItemID.Wire, 25);
            dpsMeter.AddIngredient(ItemID.JungleSpores, 10);
            dpsMeter.AddRecipeGroup(GoldPlatinumGroup, 5);
            dpsMeter.AddTile(TileID.TinkerersWorkbench);
            dpsMeter.Register();

            // Metal Detector
            Recipe metalDetector = Recipe.Create(ItemID.MetalDetector);
            metalDetector.AddRecipeGroup(GoldPlatinumGroup, 10);
            metalDetector.AddIngredient(ItemID.Sapphire);
            metalDetector.AddIngredient(ItemID.Ruby);
            metalDetector.AddIngredient(ItemID.Diamond);
            metalDetector.AddIngredient(ItemID.Emerald);
            metalDetector.AddIngredient(ItemID.Topaz);
            metalDetector.AddIngredient(ItemID.Amethyst);
            metalDetector.AddTile(TileID.TinkerersWorkbench);
            metalDetector.Register();

            // Weather Radio
            Recipe weatherRadio = Recipe.Create(ItemID.WeatherRadio);
            weatherRadio.AddIngredient(ItemID.RainCloud, 10);
            weatherRadio.AddIngredient(ItemID.Cloud, 10);
            weatherRadio.AddRecipeGroup(SilverTungstenGroup, 5);
            weatherRadio.AddTile(TileID.TinkerersWorkbench);
            weatherRadio.Register();

            // Fisherman's Pocket Guide
            Recipe fishGuide = Recipe.Create(ItemID.FishermansGuide);
            fishGuide.AddRecipeGroup(AnyFishGroup, 10);
            fishGuide.AddIngredient(ItemID.PalmWood, 10);
            fishGuide.AddIngredient(ItemID.Book);
            fishGuide.AddTile(TileID.TinkerersWorkbench);
            fishGuide.Register();

            // Sextant
            Recipe sextant = Recipe.Create(ItemID.Sextant);
            sextant.AddIngredient(ItemID.FallenStar, 5);
            sextant.AddIngredient(ItemID.Glass, 5);
            sextant.AddRecipeGroup(GoldPlatinumGroup, 4);
            sextant.AddTile(TileID.TinkerersWorkbench);
            sextant.Register();

            // Magic Mirror
            Recipe magicMirror = Recipe.Create(ItemID.MagicMirror);
            magicMirror.AddIngredient(ItemID.FallenStar, 3);
            magicMirror.AddRecipeGroup(SilverTungstenGroup, 3);
            magicMirror.AddIngredient(ItemID.Glass, 10);
            magicMirror.AddTile(TileID.Anvils);
            magicMirror.Register();

            // Ice Mirror
            Recipe iceMirror = Recipe.Create(ItemID.IceMirror);
            iceMirror.AddIngredient(ItemID.FallenStar, 3);
            iceMirror.AddIngredient(ItemID.IceBlock, 25);
            iceMirror.AddIngredient(ItemID.Glass, 10);
            iceMirror.AddTile(TileID.Anvils);
            iceMirror.Register();

            // ---------------------------
            // Newly added recipes
            // ---------------------------

            // Lava Charm: 1 Magma Stone + 1 Shackle + 5 Hellstone Bars
            Recipe lavaCharm = Recipe.Create(ItemID.LavaCharm);
            lavaCharm.AddIngredient(ItemID.MagmaStone);
            lavaCharm.AddIngredient(ItemID.Shackle);
            lavaCharm.AddIngredient(ItemID.HellstoneBar, 5);
            lavaCharm.AddTile(TileID.TinkerersWorkbench);
            lavaCharm.Register();

            // Anklet of the Wind: 15 Jungle Spores + 3 Vines + 2 Swiftness Potion
            Recipe anklet = Recipe.Create(ItemID.AnkletoftheWind);
            anklet.AddIngredient(ItemID.JungleSpores, 15);
            anklet.AddIngredient(ItemID.Vine, 3);
            anklet.AddIngredient(ItemID.SwiftnessPotion, 2);
            anklet.AddTile(TileID.TinkerersWorkbench);
            anklet.Register();

            // Ice Skates: 20 Ice Blocks + 10 Silk + 5 Boreal Wood
            Recipe iceSkates = Recipe.Create(ItemID.IceSkates);
            iceSkates.AddIngredient(ItemID.IceBlock, 20);
            iceSkates.AddIngredient(ItemID.Silk, 10);
            iceSkates.AddIngredient(ItemID.BorealWood, 5);
            iceSkates.AddTile(TileID.TinkerersWorkbench);
            iceSkates.Register();

            // Water Walking Boots: 10 Coral + 5 Waterleaf + 5 Palm Wood + 3 Water Walking Potion
            Recipe waterWalking = Recipe.Create(ItemID.WaterWalkingBoots);
            waterWalking.AddIngredient(ItemID.Coral, 10);
            waterWalking.AddIngredient(ItemID.Waterleaf, 5);
            waterWalking.AddIngredient(ItemID.PalmWood, 5);
            waterWalking.AddIngredient(ItemID.WaterWalkingPotion, 3);
            waterWalking.AddTile(TileID.TinkerersWorkbench);
            waterWalking.Register();

            // Cobalt Shield: 10 Cobalt/Palladium Bars + 20 Bones + 5 Hellstone Bars
            Recipe cobaltShield = Recipe.Create(ItemID.CobaltShield);
            cobaltShield.AddRecipeGroup(CobaltPalladiumGroup, 10);
            cobaltShield.AddIngredient(ItemID.Bone, 20);
            cobaltShield.AddIngredient(ItemID.HellstoneBar, 5);
            cobaltShield.AddTile(TileID.TinkerersWorkbench);
            cobaltShield.Register();

            // Magic Quiver: 10 Orichalcum/Mythril Bars + 500 Wooden Arrows + 20 Bones
            Recipe magicQuiver = Recipe.Create(ItemID.MagicQuiver);
            magicQuiver.AddRecipeGroup(MythrilOrichalcumGroup, 10);
            magicQuiver.AddIngredient(ItemID.WoodenArrow, 500);
            magicQuiver.AddIngredient(ItemID.Bone, 20);
            magicQuiver.AddTile(TileID.MythrilAnvil); // Hardmode station
            magicQuiver.Register();

            // Slime Staff: 250 Gel + 25 Wood + 1 Ruby
            Recipe slimeStaff = Recipe.Create(ItemID.SlimeStaff);
            slimeStaff.AddIngredient(ItemID.Gel, 250);
            slimeStaff.AddIngredient(ItemID.Wood, 25);
            slimeStaff.AddIngredient(ItemID.Ruby, 1);
            slimeStaff.AddTile(TileID.WorkBenches);
            slimeStaff.Register();

            // Rod of Discord:
            // 15 Hallowed Bars + 25 Souls of Light + 25 Crystal Shards +
            // 20 Chlorophyte Bars + 5 each Souls of Fright/Sight/Might
            Recipe rodOfDiscord = Recipe.Create(ItemID.RodofDiscord);
            rodOfDiscord.AddIngredient(ItemID.HallowedBar, 15);
            rodOfDiscord.AddIngredient(ItemID.SoulofLight, 25);
            rodOfDiscord.AddIngredient(ItemID.CrystalShard, 25);
            rodOfDiscord.AddIngredient(ItemID.ChlorophyteBar, 20);
            rodOfDiscord.AddIngredient(ItemID.SoulofFright, 5);
            rodOfDiscord.AddIngredient(ItemID.SoulofSight, 5);
            rodOfDiscord.AddIngredient(ItemID.SoulofMight, 5);
            rodOfDiscord.AddTile(TileID.MythrilAnvil);
            // Optional gating:
            // rodOfDiscord.AddCondition(Condition.DownedMechBossAll);
            rodOfDiscord.Register();

            // Hermes Boots: 15 Silk + 5 Swiftness Potions
            Recipe hermes = Recipe.Create(ItemID.HermesBoots);
            hermes.AddIngredient(ItemID.Silk, 15);
            hermes.AddIngredient(ItemID.SwiftnessPotion, 5);
            hermes.AddTile(TileID.Loom);
            hermes.Register();

            // Obsidian Rose: 5 Fireblossom + 25 Obsidian
            Recipe obsidianRose = Recipe.Create(ItemID.ObsidianRose);
            obsidianRose.AddIngredient(ItemID.Fireblossom, 5);
            obsidianRose.AddIngredient(ItemID.Obsidian, 25);
            obsidianRose.AddTile(TileID.Hellforge);
            obsidianRose.Register();
        }
    }
}