using Terraria.ID;

namespace MightofUniverses.Common.Util
{
    // Version-agnostic crate IDs, resolved lazily and safely (no exceptions).
    public static class CrateIds
    {
        private const int Unknown = int.MinValue;

        private static int _pearlwood = Unknown, _mythril = Unknown, _titanium = Unknown;

        private static int _mirage = Unknown, _bramble = Unknown, _boreal = Unknown, _azure = Unknown,
                           _stockade = Unknown, _seaside = Unknown, _hellstone = Unknown, _defiled = Unknown,
                           _hematic = Unknown, _shroomite = Unknown, _hallowed = Unknown;

        // Generic HM crates (tiered)
        public static int Pearlwood => Resolve(ref _pearlwood, "PearlwoodCrate", "WoodenCrateHard");
        public static int Mythril   => Resolve(ref _mythril,   "MythrilCrate",   "IronCrateHard");
        public static int Titanium  => Resolve(ref _titanium,  "TitaniumCrate",  "GoldenCrateHard");

        // Biome HM crates
        public static int Mirage    => Resolve(ref _mirage,    "MirageCrate",    "OasisCrateHard", "DesertCrateHard");
        public static int Bramble   => Resolve(ref _bramble,   "BrambleCrate",   "JungleCrateHard");
        public static int Boreal    => Resolve(ref _boreal,    "BorealCrate",    "FrozenCrateHard", "SnowCrateHard");
        public static int Azure     => Resolve(ref _azure,     "AzureCrate",     "SkyCrateHard", "SkyCrate_Hard");
        public static int Stockade  => Resolve(ref _stockade,  "StockadeCrate",  "DungeonCrateHard");
        public static int Seaside   => Resolve(ref _seaside,   "SeasideCrate",   "OceanCrateHard");
        public static int Hellstone => Resolve(ref _hellstone, "HellstoneCrate", "ObsidianCrateHard", "UnderworldCrateHard");
        public static int Defiled   => Resolve(ref _defiled,   "DefiledCrate",   "CorruptCrateHard");
        public static int Hematic   => Resolve(ref _hematic,   "HematicCrate",   "CrimsonCrateHard");
        public static int Shroomite => Resolve(ref _shroomite, "ShroomiteCrate", "MushroomCrateHard");

        // Hallow HM crate has varied names across builds
        public static int Hallowed  => Resolve(ref _hallowed,  "HallowedCrate", "HallowedFishingCrate", "HallowedFishingCrateHard", "HallowedCrateHard");

        public static bool Is(int type, int crateId) => crateId > 0 && type == crateId;

        private static int Resolve(ref int cache, params string[] names)
        {
            if (cache != Unknown) return cache;

            foreach (var n in names)
            {
                // TryGetId avoids KeyNotFound exceptions across tML versions
                if (ItemID.Search.TryGetId(n, out int id) && id > 0)
                {
                    cache = id;
                    return cache;
                }
            }
            cache = -1; // Not found on this build
            return cache;
        }
    }
}