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
            ApplyDemonFireIfReaper(target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyDemonFireIfReaper(target);
        }

        private void ApplyDemonFireIfReaper(NPC target)
        {
            if (hasTrappedDemonFire && IsReaperDamage())
            {
                // Apply Corrupted debuff for 3 seconds (180 frames)
                target.AddBuff(ModContent.BuffType<Demonfire>(), 180);
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