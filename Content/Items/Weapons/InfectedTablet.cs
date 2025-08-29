using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Weapons
{
    public class InfectedTablet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 18;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 3);
            Item.mana = 2;
            Item.channel = true;
            Item.useTurn = true;
        }

        public override void HoldItem(Player player)
        {
            player.GetModPlayer<InfectedTabletPlayer>().holdingTablet = true;
        }
    }
}
