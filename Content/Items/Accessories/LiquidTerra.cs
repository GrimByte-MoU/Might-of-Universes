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
                .AddIngredient(ModContent.ItemType<ElementalCore>(), 5)
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
            ApplyLiquidTerraIfReaper(target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyLiquidTerraIfReaper(target);
        }

        private void ApplyLiquidTerraIfReaper(NPC target)
        {
            if (hasLiquidTerra && IsReaperDamage())
            {
                // Apply Elements Harmony debuff for 3 seconds (180 frames)
                target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);
            }
        }

        private bool IsReaperDamage()
        {
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            return reaperPlayer.hasReaperArmor || reaperPlayer.reaperDamageMultiplier > 1f;
        }
    }
}