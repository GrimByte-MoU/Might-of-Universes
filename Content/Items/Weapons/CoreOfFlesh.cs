using Terraria.Audio;
using MightofUniverses.Content.Items.Projectiles; // our projectile types

namespace MightofUniverses.Content.Items.Weapons
{
    public class CoreOfFlesh : ModItem
    {
        // 0 = Flesh Tongues, 1 = Laser Beam, 2 = Sickle of Demons
        public static int Mode = 0;

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            // IMPORTANT: Give Item.shoot a valid default so Shoot() will be invoked.
            Item.shoot = ModContent.ProjectileType<CoreFlesh_FleshTongue>();
        }

        public override void HoldItem(Player player)
        {
            // Dim yellowish light while being used
            if (player.itemAnimation > 0 && player.HeldItem == Item)
            {
                Lighting.AddLight(player.Center, new Vector3(0.9f, 0.8f, 0.35f) * 0.5f);
            }
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Right-click: cycle mode
                Mode = (Mode + 1) % 3;
                if (Main.myPlayer == player.whoAmI)
                {
                    string msg = Mode switch
                    {
                        0 => "Flesh Tongues",
                        1 => "Laser Beam",
                        _ => "Sickle of Demons"
                    };
                    CombatText.NewText(player.getRect(), Color.LimeGreen, msg);
                    SoundEngine.PlaySound(SoundID.MenuTick with { PitchVariance = 0.2f }, player.Center);
                }
                return false;
            }

            // Configure per-mode stats and set a valid Item.shoot (so Shoot() runs)
            switch (Mode)
            {
                case 0: // Flesh Tongues: 3/s
                    Item.useTime = 20;
                    Item.useAnimation = 20;
                    Item.reuseDelay = 0;
                    Item.UseSound = SoundID.Item103;
                    Item.knockBack = 2.0f;
                    Item.shoot = ModContent.ProjectileType<CoreFlesh_FleshTongue>();
                    break;

                case 1: // Laser Beam: 3-shot burst + ~0.5s pause
                    Item.useTime = 10;
                    Item.useAnimation = 10;
                    Item.reuseDelay = 30; // ~0.5s
                    Item.UseSound = SoundID.Item91;
                    Item.knockBack = 6.5f;
                    Item.shoot = ModContent.ProjectileType<CoreFlesh_Laser>();
                    break;

                case 2: // Sickle of Demons: 3/s, consumes 10 souls per shot
                    Item.useTime = 20;
                    Item.useAnimation = 20;
                    Item.reuseDelay = 0;
                    Item.UseSound = SoundID.Item71;
                    Item.knockBack = 3.0f;
                    Item.shoot = ModContent.ProjectileType<CoreFlesh_DemonSickle>();

                    var reaper = player.GetModPlayer<ReaperPlayer>();
                    const int costPerShot = 10;
                    if (reaper.soulEnergy < costPerShot)
                    {
                        Mode = 1;
                        if (Main.myPlayer == player.whoAmI)
                        {
                            CombatText.NewText(player.getRect(), Color.OrangeRed, "Out of souls! Switching to Laser");
                        }
                        return false;
                    }
                    break;
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (Mode == 2)
            {
                var reaper = player.GetModPlayer<ReaperPlayer>();
                const int costPerShot = 10;
                if (reaper.soulEnergy >= costPerShot)
                {
                    reaper.soulEnergy -= costPerShot;
                }
            }
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Mode)
            {
                case 0: // Flesh Tongues (true beam)
                {
                    int scaled = (int)(damage * 0.75f);
                    Vector2 dir = velocity.SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(
                        source,
                        position,
                        Vector2.Zero,
                        ModContent.ProjectileType<CoreFlesh_FleshTongue>(),
                        scaled,
                        knockback,
                        player.whoAmI,
                        ai0: dir.ToRotation()
                    );
                    break;
                }

                case 1: // Laser Beam: 3 lasers, 5° spread, 80% damage, pierce 1
                {
                    int scaled = (int)(damage * 0.80f);
                    float spread = MathHelper.ToRadians(5f);
                    for (int i = 0; i < 3; i++)
                    {
                        float t = i / 2f; // 0, 0.5, 1
                        float rot = MathHelper.Lerp(-spread, spread, t);
                        Vector2 v = velocity.RotatedBy(rot).SafeNormalize(Vector2.UnitX) * 16f;
                        Projectile.NewProjectile(source, position, v, ModContent.ProjectileType<CoreFlesh_Laser>(), scaled, knockback, player.whoAmI);
                    }
                    break;
                }

                case 2: // Sickle of Demons
                {
                    int scaled = (int)(damage * 2.5f);
                    Vector2 aim = velocity.SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(
                        source,
                        position,
                        aim * 0.01f,
                        ModContent.ProjectileType<CoreFlesh_DemonSickle>(),
                        scaled,
                        knockback,
                        player.whoAmI,
                        ai0: aim.ToRotation(),
                        ai1: 0f
                    );
                    break;
                }
            }

            // Suppress default shot (we spawned manually)
            return false;
        }
    }
}