using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Input
{
    // Try without the Autoload attribute
    public class ModKeybindManager : ModSystem
    {
        public static ModKeybind Ability1;
        public static ModKeybind Ability2;
        public static ModKeybind Ability3;
        public static ModKeybind Ability4;
        public static ModKeybind Ability5;
        public static ModKeybind ArmorAbility;

        public override void Load()
        {
            if (Main.dedServ) return;

            Ability1 = KeybindLoader.RegisterKeybind(Mod, "Ability 1", "Z");
            Ability2 = KeybindLoader.RegisterKeybind(Mod, "Ability 2", "X");
            Ability3 = KeybindLoader.RegisterKeybind(Mod, "Ability 3", "C");
            Ability4 = KeybindLoader.RegisterKeybind(Mod, "Ability 4", "V");
            Ability5 = KeybindLoader.RegisterKeybind(Mod, "Ability 5", "B");
            ArmorAbility = KeybindLoader.RegisterKeybind(Mod, "Armor Set Ability", "Q");
        }

        public override void Unload()
        {
            Ability1 = null;
            Ability2 = null;
            Ability3 = null;
            Ability4 = null;
            Ability5 = null;
            ArmorAbility = null;
        }
    }
}
