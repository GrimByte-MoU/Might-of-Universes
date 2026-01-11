using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LiquidTerra : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
             Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<LiquidTerraPlayer>().hasLiquidTerra = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddIngredient(ModContent.ItemType<PrismaticVial>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class LiquidTerraPlayer : ModPlayer
    {
        public bool hasLiquidTerra = false;

        public override void ResetEffects()
        {
            hasLiquidTerra = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasLiquidTerra && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasLiquidTerra && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);
            }
        }
    }
}