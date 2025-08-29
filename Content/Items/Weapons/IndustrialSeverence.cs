using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class IndustrialSeverence : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 70;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IndustrialGear>();
            Item.shootSpeed = 14f;
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    int numGears = Main.rand.Next(1, 8); // 1-7 gears
    
    for (int i = 0; i < numGears; i++)
    {
        float speedMultiplier = Main.rand.NextFloat(0.75f, 1.5f);
        Vector2 newVelocity = velocity * speedMultiplier;
        
        float spread = MathHelper.ToRadians(5);
        newVelocity = newVelocity.RotatedByRandom(spread);
        
        Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
    }
    
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(25f))
        {
            Projectile.NewProjectile(source, position, velocity, 
                ModContent.ProjectileType<TeslaSpear>(), 
                damage * 2, knockback * 1.5f, player.whoAmI);
            Main.NewText("25 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }
    
    return false;
}
        
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            target.AddBuff(ModContent.BuffType<Shred>(), 180);
            reaperPlayer.AddSoulEnergy(5f, target.Center); // Call this where souls are gathered

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cog, 50)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 25)
                .AddIngredient(ItemID.SoulofFright, 20)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

