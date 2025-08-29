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
            ApplyCorruptedIfReaper(target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyCorruptedIfReaper(target);
        }

        private void ApplyCorruptedIfReaper(NPC target)
        {
            if (hasVialOfCorruption && IsReaperDamage())
            {
                // Apply Corrupted debuff for 3 seconds (180 frames)
                target.AddBuff(ModContent.BuffType<Corrupted>(), 180);
            }
        }

        private bool IsReaperDamage()
        {
            // Check if the player is using reaper weapons or has reaper armor
            // This is a placeholder - implement based on your mod's logic
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            return reaperPlayer.hasReaperArmor || reaperPlayer.reaperDamageMultiplier > 1f;
        }
    }
}
