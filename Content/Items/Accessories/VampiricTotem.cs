using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightOfUniverses.Content.Items.Accessories
{
    public class VampiricTotem : ModItem
    {
        public override void SetStaticDefaults()
        {
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
        private const float LIFESTEAL_PERCENT = 0.05f;

        public override void ResetEffects()
        {
            hasVampiricTotem = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVampiricTotem && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyLifesteal(damageDone);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVampiricTotem && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyLifesteal(damageDone);
            }
        }

        private void ApplyLifesteal(int damageDone)
        {
            int healAmount = (int)(damageDone * LIFESTEAL_PERCENT);
            
            if (damageDone > 0 && healAmount < 1)
                healAmount = 1;
            
            if (healAmount > 0)
            {
                Player.HealEffect(healAmount);
                Player.statLife += healAmount;
                
                if (Player.statLife > Player.statLifeMax2)
                    Player.statLife = Player.statLifeMax2;
                
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
}