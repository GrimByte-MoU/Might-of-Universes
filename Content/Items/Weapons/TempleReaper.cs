using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TempleReaper : ModItem
    {
        private int buffTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 90;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TempleReaperShard>();
            Item.shootSpeed = 10f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(7f, target.Center);

            if (!target.active)
            {
                reaper.AddSoulEnergy(7f, target.Center);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            
            
if (ReaperPlayer.SoulReleaseKey.JustPressed)



            {
                if (reaper.ConsumeSoulEnergy(175f))
                {
                    player.Heal(150);
                    player.statDefense += 25;
                    player.endurance += 0.2f;
                    player.lifeRegen += 12;
                    buffTimer = 300; // 5 seconds
                    Main.NewText("175 souls released!", Color.Green);
                    return false;
                }
                else
                {
                    Main.NewText("Not enough soul energy to activate!", Color.Red);
                }
            }

            // Calculate the angle offset for the arc
            float spread = MathHelper.ToRadians(10); // 10 degree arc
            float baseSpeed = velocity.Length();
            double startAngle = Math.Atan2(velocity.Y, velocity.X) - spread / 2;
            double deltaAngle = spread / 2;

            for (int i = 0; i < 3; i++)
            {
                // Calculate the velocity for each projectile
                double offsetAngle = startAngle + deltaAngle * i;
                Vector2 newVelocity = new Vector2((float)Math.Cos(offsetAngle), (float)Math.Sin(offsetAngle)) * baseSpeed;

                // Determine if the projectile should be the stronger version
                int projectileType = type;
                if (Main.rand.NextFloat() <= 0.15f)
                {
                    projectileType = ModContent.ProjectileType<TempleReaperFireball>();
                    damage = (int)(damage * 1.25f);
                    knockback *= 2;
                }

                // Create the projectile
                Projectile.NewProjectile(source, position, newVelocity, projectileType, damage, knockback, player.whoAmI);
            }

            return true;
        }

        public override void UpdateInventory(Player player)
        {
            if (buffTimer > 0)
            {
                buffTimer--;
                if (buffTimer <= 0)
                {
                    player.lifeRegen -= 12;
                    player.statDefense -= 25;
                    player.endurance -= 0.2f;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LihzahrdBrick, 35)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
