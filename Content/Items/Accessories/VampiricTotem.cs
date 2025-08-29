using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightOfUniverses.Content.Items.Accessories
{
    public class VampiricTotem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName and Tooltip are automatically set from .hjson files in 1.4.4
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VampiricTotemPlayer>().hasVampiricTotem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Vertebrae, 5)
                .AddIngredient(ItemID.Shadewood, 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class VampiricTotemPlayer : ModPlayer
    {
        public bool hasVampiricTotem = false;
        private const float LIFESTEAL_PERCENT = 0.05f; // 5% lifesteal

        public override void ResetEffects()
        {
            hasVampiricTotem = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyLifestealIfReaper(damageDone);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyLifestealIfReaper(damageDone);
        }

        private void ApplyLifestealIfReaper(int damageDone)
        {
            if (hasVampiricTotem && IsReaperDamage())
            {
                // Calculate healing amount (5% of damage)
                int healAmount = (int)(damageDone * LIFESTEAL_PERCENT);
                
                // Ensure at least 1 health is restored if damage was dealt
                if (damageDone > 0 && healAmount < 1)
                    healAmount = 1;
                
                if (healAmount > 0)
                {
                    // Heal the player
                    Player.HealEffect(healAmount);
                    Player.statLife += healAmount;
                    
                    // Cap health at max
                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;
                    
                    // Visual effect - red particles
                    for (int i = 0; i < 5; i++)
                    {
                        Dust.NewDust(
                            Player.position,
                            Player.width,
                            Player.height,
                            DustID.Blood,
                            Main.rand.NextFloat(-2f, 2f),
                            Main.rand.NextFloat(-2f, 2f),
                            0,
                            default,
                            Main.rand.NextFloat(0.7f, 1.2f)
                        );
                    }
                }
            }
        }

        private bool IsReaperDamage()
        {
            // Check if the player is using reaper weapons or has reaper armor
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            return reaperPlayer.hasReaperArmor || reaperPlayer.reaperDamageMultiplier > 1f;
        }
    }
}
