// Content/Items/Accessories/EarthenCrown.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft. Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses. Content.Items.Accessories
{
    public class EarthenCrown : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item. height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Master;
            Item. master = true;
            Item. value = Item.sellPrice(gold: 15);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // +25% damage to ALL classes
            player.GetDamage(DamageClass.Generic) += 0.25f;

            // +150% Pacifist damage (separate, doesn't stack with generic)
            var pacifistPlayer = player.GetModPlayer<PacifistPlayer>();
            pacifistPlayer.pacifistDamageMultiplier += 1.50f;

            // +15% crit chance to ALL classes
            player.GetCritChance(DamageClass.Generic) += 15;

            if (!hideVisual && Main.rand.NextBool(10))
            {
                int dust = Dust.NewDust(player.Center, 0, 0, DustID.GoldCoin,
                    0f, 0f, 150, Color.Gold, 0.9f);
                Main.dust[dust].noGravity = true;
            }
        }

    }
}