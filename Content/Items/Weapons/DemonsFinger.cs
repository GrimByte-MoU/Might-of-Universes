using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class DemonsFinger : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 50f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 55;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DemonsFingerProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed && reaper.ConsumeSoulEnergy(SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost)))
            {
                Vector2 from = player.MountedCenter;
                Vector2 dir = Main.MouseWorld - from;
                if (dir.LengthSquared() < 0.0001f) dir = new Vector2(player.direction, 0f);
                dir.Normalize();
                Vector2 baseVel = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 16f);

                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);

                for (int i = 0; i < 7; i++)
                {
                    Vector2 newVelocity = baseVel.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-7.5f, 7.5f)));
                    Projectile.NewProjectile(
                        src,
                        from,
                        newVelocity * 1.5f,
                        ModContent.ProjectileType<Demonsoul>(),
                        (int)(damage * 1.5f),
                        kb,
                        player.whoAmI,
                        0f,
                        i * 5
                    );
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(4f, target.Center);
            target.AddBuff(BuffID.OnFire3, 180);

            Dust.NewDust(target.position, target.width, target.height, DustID.Torch);
            Lighting.AddLight(target.Center, 1f, 0.3f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position.Y -= Item.height / 2;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HellsTalon>())
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}