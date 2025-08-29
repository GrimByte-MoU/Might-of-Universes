using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace MightOfUniverses.Content.Items.Accessories
{
    public class InfernalTablet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalTabletPlayer>().hasInfernalTablet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshBlock, 25)
                .AddIngredient(ItemID.Hellstone, 10)
                .AddIngredient(ItemID.ObsidianBrick, 30)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }

    public class InfernalTabletPlayer : ModPlayer
    {
        public bool hasInfernalTablet = false;
        public int infernalTabletCooldown = 0;
        private const int COOLDOWN_TIME = 600; // 10 seconds

        public override void ResetEffects()
        {
            hasInfernalTablet = false;
        }

        public override void PostUpdate()
        {
            if (infernalTabletCooldown > 0)
                infernalTabletCooldown--;

            if (hasInfernalTablet)
            {
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                
                // Check if the Soul Release key is pressed and not on cooldown
                if (ReaperPlayer.SoulReleaseKey.JustPressed && infernalTabletCooldown <= 0)
                {
                    // Calculate health to sacrifice (25% of current health)
                    int healthToSacrifice = (int)(Player.statLife * 0.25f);
                    
                    // Ensure we don't kill the player
                    if (healthToSacrifice >= Player.statLife - 1)
                        healthToSacrifice = Player.statLife - 1;
                    
                    if (healthToSacrifice > 0)
                    {
                        // Sacrifice health
                        Player.statLife -= healthToSacrifice;
                        
                        // Calculate soul energy to gain (50% of health sacrificed)
                        float soulEnergyGain = healthToSacrifice * 0.5f;
                        
                        // Add soul energy
                        reaperPlayer.AddSoulEnergy(soulEnergyGain, Player.Center);
                        
                        // Start cooldown
                        infernalTabletCooldown = COOLDOWN_TIME;
                        
                        // Visual effects
                        Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " sacrificed their life force"), 0, 1);
                        
                        // Create fire particles
                        for (int i = 0; i < 30; i++)
                        {
                            Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                            Dust.NewDust(
                                Player.Center,
                                0,
                                0,
                                DustID.Torch,
                                velocity.X,
                                velocity.Y,
                                0,
                                default,
                                Main.rand.NextFloat(1f, 2f)
                            );
                        }
                        
                        // Show text
                        if (Main.myPlayer == Player.whoAmI)
                        {
                            CombatText.NewText(Player.getRect(), Color.Red, "-" + healthToSacrifice + " HP");
                            CombatText.NewText(Player.getRect(), Color.White, "+" + soulEnergyGain + " Soul Energy");
                        }
                    }
                }
            }
        }
    }
}
