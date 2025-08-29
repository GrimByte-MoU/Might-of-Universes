using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Conecussion : ModItem
    {
        public override void SetDefaults()
        {
            // DefaultToWhip handles useStyle, animation, etc.
            Item.DefaultToWhip(ModContent.ProjectileType<ConecussionWhip>(), 40, 2f, 20);
            Item.DamageType = DamageClass.Summon;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 3);
        }

        public override bool MeleePrefix() => true;
    }
}

