using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class TrappedDemonFire : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TrappedDemonFirePlayer>().hasTrappedDemonFire = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DemonicEssence>(), 10)
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class TrappedDemonFirePlayer : ModPlayer
    {
        public bool hasTrappedDemonFire = false;

        public override void ResetEffects()
        {
            hasTrappedDemonFire = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasTrappedDemonFire && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Demonfire>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasTrappedDemonFire && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Demonfire>(), 180);
            }
        }
    }
}