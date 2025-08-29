using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Consumables
{
    public class KeyOfTheVoidGate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item2;
            Item.rare = ModContent.RarityType<EldritchRarity>();
            Item.consumable = true;
            Item.maxStack = 1;
        }

        public override bool? UseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<KeyOfTheVoidGatePlayer>();
            if (!modPlayer.usedKeyOfTheVoidGate)
            {
                modPlayer.usedKeyOfTheVoidGate = true;
                CombatText.NewText(player.Hitbox, CombatText.HealLife, 50, dramatic: true);
                return true;
            }
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.GetModPlayer<KeyOfTheVoidGatePlayer>().usedKeyOfTheVoidGate;
        }
    }
}
