using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
public class CandiedHeart : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.sellPrice(platinum: 1);
        Item.rare = ItemRarityID.Cyan;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statDefense += 8;
        player.statLifeMax2 += 30;

        if (player.statLife > player.statLifeMax2 * 0.5f)
        {
            player.GetDamage(DamageClass.Generic) += 0.10f;
            player.GetCritChance(DamageClass.Generic) += 10f;
        }
        else
        {
            player.lifeRegen += 4;
            player.endurance += 0.10f;
        }
    }
}
}