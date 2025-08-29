using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    public class KeyOfTheVoidGatePlayer : ModPlayer
    {
        public bool usedKeyOfTheVoidGate;
        private const float DodgeChance = 0.02f;

        public override void ResetEffects()
        {
            // Add accessory slot if consumed
            if (usedKeyOfTheVoidGate)
            {
                Player.extraAccessorySlots += 1;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["usedKeyOfTheVoidGate"] = usedKeyOfTheVoidGate;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("usedKeyOfTheVoidGate"))
                usedKeyOfTheVoidGate = tag.GetBool("usedKeyOfTheVoidGate");
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (!usedKeyOfTheVoidGate || Main.rand.NextFloat() >= DodgeChance)
                return;

            // Cancel the damage
            modifiers.SetMaxDamage(0);
            
            // Smoke effect
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.Shadowflame, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3), 150, default, 1.5f);
            }
        }
        
        public override void OnHurt(Player.HurtInfo info)
        {
            if (usedKeyOfTheVoidGate && Main.rand.NextFloat() < DodgeChance)
            {
                // Heal for the damage that would have been taken
                Player.Heal(info.Damage);
            }
        }
    }
}
