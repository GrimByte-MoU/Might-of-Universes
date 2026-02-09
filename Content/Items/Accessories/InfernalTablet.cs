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
        private const int COOLDOWN_TIME = 600;

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

                if (ReaperPlayer.SoulReleaseKey.JustPressed && infernalTabletCooldown <= 0)
                {
                    int healthToSacrifice = (int)(Player.statLife * 0.25f);
                    if (healthToSacrifice >= Player.statLife - 1)
                        healthToSacrifice = Player.statLife - 1;
                    
                    if (healthToSacrifice > 0)
                    {
                        Player.statLife -= healthToSacrifice;
                        float soulEnergyGain = healthToSacrifice * 0.5f;
                        reaperPlayer.AddSoulEnergy(soulEnergyGain, Player.Center);
                        infernalTabletCooldown = COOLDOWN_TIME;
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
