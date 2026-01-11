using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TerraiumLongsword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(gold: 65);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.5f;
            Item.maxStack = 1;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            // Soft blue glow as it swings
            Lighting.AddLight(hitbox.Center.ToVector2(), 0.2f, 0.4f, 0.9f);

            // Cloud + water particles
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Cloud, 0f, 0f, 150, default, 1.2f);
                Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Water, 0f, 0f, 150, Color.LightBlue, 1.3f);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 2; i++)
                {
                    
                    int proj = Projectile.NewProjectile(
                        player.GetSource_OnHit(target),
                        target.Center + new Vector2(Main.rand.Next(-40, 40), -600f),
                        Vector2.Zero,
                        ModContent.ProjectileType<TerraiumLightning>(),
                        (int)(damageDone * 0.75f),
                        0f,
                        player.whoAmI,
                        target.whoAmI,
                        i * 10
                    );
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
