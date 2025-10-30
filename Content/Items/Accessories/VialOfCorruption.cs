using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Accessories
{
    public class VialOfCorruption : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VialOfCorruptionPlayer>().hasVialOfCorruption = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WormTooth, 5)
                .AddIngredient(ItemID.RottenChunk, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class VialOfCorruptionPlayer : ModPlayer
    {
        public bool hasVialOfCorruption = false;

        public override void ResetEffects()
        {
            hasVialOfCorruption = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVialOfCorruption && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Corrupted>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVialOfCorruption && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Corrupted>(), 180);
            }
        }
    }
}