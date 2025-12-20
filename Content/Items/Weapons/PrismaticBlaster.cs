using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
public class PrismaticBlaster : ModItem
{
    private int grenadeTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 2);
            Item.scale = 1f;

            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item11;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 35;
            Item.knockBack = 1f;
            Item.noMelee = true;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 25f;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
    }

    public override Vector2? HoldoutOffset()
    {
        return new Vector2(-2f, -2f);
    }

  public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
  {
      Vector2 muzzleOffset = velocity;
      muzzleOffset.Normalize();
      muzzleOffset *= 25f;
      if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
      {
          position += muzzleOffset;
      }
  
      for (int i = 0; i < 2; i++)
      {
          Vector2 bulletVelocity = velocity.RotatedBy(MathHelper.ToRadians(5 * i));
          Projectile.NewProjectile(source, position, bulletVelocity, type, damage, knockback, player.whoAmI);
      }
  
      grenadeTimer++;
      if (grenadeTimer >= 15)
      {
          grenadeTimer = 0;
          Vector2 grenadeVelocity = velocity * 0.5f;
          int grenadeProj = Projectile.NewProjectile(source, position, grenadeVelocity, 
              ModContent.ProjectileType<PrismaticGrenade>(), damage * 2, knockback * 2f, player.whoAmI);
          SoundEngine.PlaySound(SoundID.Item61, player.position);
      }
  
      return false; // Return false, OnHitNPC is not called from this method
  }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<GoblinToxigun>())
            .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
}