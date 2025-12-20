using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GoblinToxigun : ModItem
    {
        private int grenadeTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 2);
            Item.scale = 1f;

            Item.useTime = 16;
            Item.useAnimation = 16;
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

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            grenadeTimer++;
            if (grenadeTimer >= 22)
            {
                grenadeTimer = 0;
                Vector2 grenadeVelocity = velocity * 0.5f;
                int grenadeProj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, grenadeVelocity, 
                    ModContent.ProjectileType<ToxicGrenadeProjectile>(), damage * 2, knockback * 2f, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item61, player.position);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GoblinsCurse>(), 60);
            target.AddBuff(BuffID.Venom, 120);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinPistol>())
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}


