using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class HellsTalon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 35;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HellsTalonProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(2f, target.Center);
            target.AddBuff(BuffID.OnFire, 180);
            
            Dust.NewDust(target.position, target.width, target.height, DustID.Torch);
            Lighting.AddLight(target.Center, 1f, 0.3f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            
            if (ReaperPlayer.SoulReleaseKey.JustPressed && reaper.ConsumeSoulEnergy(40f))
            {
                Projectile.NewProjectile(source, position, velocity, 
                    ModContent.ProjectileType<HellsoulProjectile>(), 
                    (int)(damage * 1.5f), knockback * 2f, player.whoAmI);
                return false;
            }

            int projectileCount = Main.rand.Next(1, 3); // 1 or 2 projectiles
            for (int i = 0; i < projectileCount; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f)));
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
