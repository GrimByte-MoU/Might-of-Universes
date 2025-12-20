using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GoblinPistol : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
            Item.scale = 0.5f;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 12;
            Item.knockBack = 2f;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.UseSound = SoundID.Item11;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.maxStack = 1;
        }
         public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Venom, 60);
    }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinScrap>(), 10)
                .AddIngredient(ModContent.ItemType<GoblinLeather>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
