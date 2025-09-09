using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SolunarScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 37;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SolunarProjectile>();
            Item.shootSpeed = 15f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);
            
            Dust.NewDust(target.position, target.width, target.height, DustID.PurpleTorch);
            Dust.NewDust(target.position, target.width, target.height, DustID.OrangeTorch);
            Lighting.AddLight(target.Center, 0.8f, 0f, 0.8f);
            Lighting.AddLight(target.Center, 1f, 0.5f, 0f);
        }

 public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(50f))
        {
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, 
                ModContent.ProjectileType<SolunarScytheMedallion>(), 
                damage * 2, knockback, player.whoAmI, 0f);
                
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, 
                ModContent.ProjectileType<SolunarScytheMedallion>(), 
                damage * 2, knockback, player.whoAmI, MathHelper.Pi);
            return false;
        }
    }

    // Double helix projectiles
    float offset = 20f;
    Vector2 position1 = position + Vector2.Normalize(velocity).RotatedBy(MathHelper.PiOver2) * offset;
    Vector2 position2 = position + Vector2.Normalize(velocity).RotatedBy(-MathHelper.PiOver2) * offset;
    
    Projectile.NewProjectile(source, position1, velocity, type, damage, knockback, player.whoAmI, 0.5f);
    Projectile.NewProjectile(source, position2, velocity, type, damage, knockback, player.whoAmI, -0.5f);
    
    return false;
}


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SolunarToken>(12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
