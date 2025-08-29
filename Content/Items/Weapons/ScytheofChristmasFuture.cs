using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ScytheofChristmasFuture : ModItem
    {
        private int regenTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 65;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Leaf1>();
            Item.shootSpeed = 14f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(175f))
        {
            player.lifeRegen += 40;
            regenTimer = 300;
            Main.NewText("175 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }
    if (Main.rand.NextFloat() <= 0.15f)
    {
        Projectile.NewProjectile(source, position, velocity, 
            ModContent.ProjectileType<Leaf2>(), 
            (int)(damage * 1.25f), knockback, player.whoAmI);
    }
    else
    {
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    }
    return true;
}

public override void UpdateInventory(Player player)
{
    if (regenTimer > 0)
    {
        regenTimer--;
        if (regenTimer <= 0)
        {
            player.lifeRegen -= 40;
        }
    }
}


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FestiveSpirit>(12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}