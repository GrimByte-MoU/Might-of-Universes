using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticVial : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PrismaticVialPlayer>().hasPrismaticVial = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle, 5)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class PrismaticVialPlayer : ModPlayer
    {
        public bool hasPrismaticVial = false;

        public override void ResetEffects()
        {
            hasPrismaticVial = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticVial && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticVial && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            }
        }
    }
}