using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TempleReaper : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 175f;

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
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(7f, target.Center);
            if (!target.active)
                player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(7f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: effectiveCost,
                    durationTicks: 300,
                    configure: vals =>
                    {
                        vals.Defense += 25;
                        vals.Endurance += 0.20f;
                        vals.LifeRegen += 12;
                    }
                );
                if (released)
                    player.Heal(150);
                return false;
            }

            float spread = MathHelper.ToRadians(10);
            float baseSpeed = velocity.Length();
            double startAngle = Math.Atan2(velocity.Y, velocity.X) - spread / 2;
            double deltaAngle = spread / 2;

            for (int i = 0; i < 3; i++)
            {
                double offsetAngle = startAngle + deltaAngle * i;
                Vector2 newVelocity = new Vector2((float)Math.Cos(offsetAngle), (float)Math.Sin(offsetAngle)) * baseSpeed;

                int projectileType = type;
                int projDamage = damage;
                float projKB = knockback;

                if (Main.rand.NextFloat() <= 0.15f)
                {
                    projectileType = ModContent.ProjectileType<TempleReaperFireball>();
                    projDamage = (int)(projDamage * 1.25f);
                    projKB *= 2f;
                }

                Projectile.NewProjectile(source, position, newVelocity, projectileType, projDamage, projKB, player.whoAmI);
            }

            return true;
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