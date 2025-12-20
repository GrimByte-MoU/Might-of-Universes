using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ChristmasCleaver : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 100;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 200;
            Item.knockBack = 8f;
            Item.crit = 10;
            Item.maxStack = 1;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
            int lifeSteal = damageDone / 20;
            player.statLife += lifeSteal;
            player.Heal(lifeSteal);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 5)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
