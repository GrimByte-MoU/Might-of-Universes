using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Consumables
{
    public class LunarCoreShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Purple;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.consumable = true;
            Item.UseSound = SoundID.Item4;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.GetModPlayer<LunarShardPlayer>().consumedLunarCoreShard;
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<LunarShardPlayer>().consumedLunarCoreShard = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncPlayer, number: player.whoAmI);
            }

            Main.NewText("Your accessory capacity has expanded!", 50, 255, 130);

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<LunarShardPlayer>().consumedLunarCoreShard)
            {
                tooltips.Add(new TooltipLine(Mod, "Used", "[c/FF0000:Already consumed]"));
            }
        }
    }
}