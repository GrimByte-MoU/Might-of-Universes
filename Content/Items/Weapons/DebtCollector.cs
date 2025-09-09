using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Weapons
{
    public class DebtCollector : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 85;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SoulCoin>();
            Item.shootSpeed = 16f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
            target.AddBuff(BuffID.OnFire3, 180);
            
            Dust.NewDust(target.position, target.width, target.height, DustID.Torch);
            Lighting.AddLight(target.Center, 1f, 0.3f, 0f);
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(50f))
        {
            for (int i = 0; i < 7; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-7.5f, 7.5f)));
                Projectile.NewProjectile(source, position, newVelocity * 1.5f, 
                    ModContent.ProjectileType<GreedySoul>(), 
                    (int)(damage * 1.5f), knockback, player.whoAmI, 0f, i * 5); // Delay based on i
            }
            return false;
        }
    }

    position.Y -= Item.height / 2;
    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    return false;
}


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GreedySpirit>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}