using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Input
{
    // Loads only on client; avoids registering keybinds on dedicated server
    [Autoload(Side = ModSide.Client)]
    public class ModKeybindManager : ModSystem
    {
        public static ModKeybind Ability1;
        public static ModKeybind Ability2;
        public static ModKeybind Ability3;
        public static ModKeybind Ability4;
        public static ModKeybind Ability5;

        public override void Load()
        {
            // Default bindings can be "", letting players bind them themselves if you prefer
            Ability1 = KeybindLoader.RegisterKeybind(Mod, "Ability 1", "Z");
            Ability2 = KeybindLoader.RegisterKeybind(Mod, "Ability 2", "X");
            Ability3 = KeybindLoader.RegisterKeybind(Mod, "Ability 3", "C");
            Ability4 = KeybindLoader.RegisterKeybind(Mod, "Ability 4", "V");
            Ability5 = KeybindLoader.RegisterKeybind(Mod, "Ability 5", "B");
        }

        public override void Unload()
        {
            Ability1 = null;
            Ability2 = null;
            Ability3 = null;
            Ability4 = null;
            Ability5 = null;
        }
    }
}
