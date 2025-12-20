using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CobaltScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 50f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 40;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CobaltScytheProjectile>();
            Item.shootSpeed = 8f;
            Item.maxStack = 1;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    Vector2 from = player.MountedCenter;
                    Vector2 dir = Main.MouseWorld - from;
                    if (dir.LengthSquared() < 0.0001f) dir = new Vector2(player.direction, 0f);
                    dir.Normalize();
                    Vector2 baseVel = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 8f);

                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);

                    for (int i = -1; i <= 2; i++)
                    {
                        Vector2 newVelocity = baseVel.RotatedBy(MathHelper.ToRadians(10 * i));
                        Projectile.NewProjectile(
                            src,
                            from,
                            newVelocity,
                            ModContent.ProjectileType<CobaltShotProjectile>(),
                            damage * 2,
                            kb,
                            player.whoAmI
                        );
                    }
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);

            if (!target.active)
            {
                reaper.AddSoulEnergy(3f, target.Center);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}