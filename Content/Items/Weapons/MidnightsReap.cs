using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class MidnightsReap : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 38;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MidnightWaveProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);
            
            Dust.NewDust(target.position, target.width, target.height, DustID.PurpleTorch);
            Lighting.AddLight(target.Center, 0.8f, 0f, 0.8f);
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(50f))
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-7.5f, 7.5f)));
                Projectile.NewProjectile(source, position, newVelocity * 1.5f, 
                    ModContent.ProjectileType<MidnightSoulProjectile>(), 
                    (int)(damage * 1.5f), knockback, player.whoAmI, 0f, i * 5); // Delay based on i
            }
            Main.NewText("50 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }

    position.Y -= Item.height / 2;
    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    return true;
}



        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<HellsTalon>()
                .AddIngredient<Bloodletter>()
                .AddIngredient<WardensHook>()
                .AddIngredient<MeteoriteHarvester>()
                .AddTile(TileID.DemonAltar)
                .Register();

                CreateRecipe()
                .AddIngredient<HellsTalon>()
                .AddIngredient<Evildoer>()
                .AddIngredient<WardensHook>()
                .AddIngredient<MeteoriteHarvester>()
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
