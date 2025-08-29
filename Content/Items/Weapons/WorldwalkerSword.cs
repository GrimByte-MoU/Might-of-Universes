using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class WorldwalkerSword : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 180;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    int projType = 0;
    int projDamage = damage;
    float projKB = knockback;

    // Evil biomes (Corruption/Crimson)
    if (player.ZoneCorrupt || player.ZoneCrimson)
    {
        projType = ModContent.ProjectileType<WorldwalkerEvilBolt>();
    }
    // Snow
    else if (player.ZoneSnow)
    {
        projType = ModContent.ProjectileType<WorldwalkerChilly>();
    }
    // Jungle
    else if (player.ZoneJungle)
    {
        projType = ModContent.ProjectileType<WorldwalkerStinger>();
        projDamage = (int)(damage * 0.5f);
    }
    // Underworld
    else if (player.ZoneUnderworldHeight)
    {
        projType = ModContent.ProjectileType<WorldwalkerFireWave>();
    }
    // Desert
    else if (player.ZoneDesert)
    {
    }
    // Hallow
    else if (player.ZoneHallow)
    {
    }
    else if (player.ZoneOverworldHeight &&
            !player.ZoneCorrupt &&
            !player.ZoneCrimson &&
            !player.ZoneSnow &&
            !player.ZoneJungle &&
            !player.ZoneDesert &&
            !player.ZoneHallow &&
            !player.ZoneDungeon &&
            !player.ZoneUnderworldHeight)
    {
        projType = ModContent.ProjectileType<WorldwalkerPurityBoulder>();
    }
    if (projType != 0)
    {
        if (projType == ModContent.ProjectileType<WorldwalkerStinger>())
        {
            int count = Main.rand.Next(5, 8);
            for (int i = 0; i < count; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10));
                perturbedSpeed *= Main.rand.NextFloat(0.4f, 1f);
                Projectile.NewProjectile(source, position, perturbedSpeed, projType, projDamage, projKB, player.whoAmI);
            }
        }
        else
        {
            Projectile.NewProjectile(source, position, velocity, projType, projDamage, projKB, player.whoAmI);
        }
    }

    return false;
}


    }
}
