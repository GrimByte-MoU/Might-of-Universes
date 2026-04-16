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
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ReapersEcho : ModItem, IHasSoulCost, IScytheWeapon
    {
        public float BaseSoulCost => 450f;
        private int swingCounter = 0;
        public int EmpowermentDurationTicks => 180;


        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.damage = 125;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EchoAxehead>();
            Item.shootSpeed = 18f;
            Item.maxStack = 1;
            Item.scale = 1.2f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                if (reaper.ConsumeSoulEnergy(BaseSoulCost))
                {
                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);

                    for (int i = 0; i < 10; i++)
                    {
                        Projectile.NewProjectile(
                            src,
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<EchoBlade>(),
                            damage * 2,
                            kb,
                            player.whoAmI,
                            i,
                            0f
                        );
                    }

                    Vector2 dir = Main.MouseWorld - player.Center;
                    if (dir.LengthSquared() < 0.0001f) dir = new Vector2(player.direction, 0f);
                    dir.Normalize();
                    Vector2 velocity = dir * 20f;

                    Projectile.NewProjectile(
                        src,
                        player.Center,
                        velocity,
                        ModContent.ProjectileType<EchoSoulsaw>(),
                        damage * 5,
                        kb * 1.5f,
                        player.whoAmI
                    );

                    ReaperEmpowermentState.Apply(player, 180, vals =>
                    {
                        vals.Defense = 15;
                        vals.ReaperDamage = 0.15f;
                        vals.CritChance = 15f;
                        vals.ArmorPenetration = 20;
                        vals.LifestealPercent = 15;
                    });

                    SoundEngine.PlaySound(SoundID.Item84, player.Center);

                    for (int i = 0; i < 60; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 60f;
                        Vector2 dustDir = angle.ToRotationVector2();
                        Vector2 dustVel = dustDir * Main.rand.NextFloat(8f, 15f);
                        
                        int dustType = Main.rand.Next(new int[] { 
                            DustID.RainbowMk2, 
                            DustID.Electric, 
                            DustID.PurpleTorch 
                        });
                        
                        Dust dust = Dust.NewDustPerfect(
                            player.Center,
                            dustType,
                            dustVel,
                            100,
                            default,
                            Main.rand.NextFloat(1.5f, 2.5f)
                        );
                        dust.noGravity = true;
                        dust.fadeIn = 1.2f;
                    }
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);

            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.RainbowMk2,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    100,
                    default,
                    Main.rand.NextFloat(1.2f, 1.8f)
                );
                dust.noGravity = true;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            swingCounter++;

            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EchoAxehead>(), damage, knockback, player.whoAmI);

            if (swingCounter >= 5)
            {
                swingCounter = 0;

                Vector2 baseDir = velocity;
                baseDir.Normalize();

                for (int i = -1; i <= 2; i++)
                {
                    Vector2 shardVel = baseDir.RotatedBy(MathHelper.ToRadians(5 * i)) * 16f;
                    Projectile.NewProjectile(
                        source,
                        position,
                        shardVel,
                        ModContent.ProjectileType<EchoShard>(),
                        (int)(damage * 0.75f),
                        knockback * 0.75f,
                        player.whoAmI
                    );
                }

                SoundEngine.PlaySound(SoundID.Item30, position);
            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    position,
                    10,
                    10,
                    DustID.RainbowMk2,
                    velocity.X * 0.5f,
                    velocity.Y * 0.5f,
                    100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CactusScythe>()
                .AddIngredient<AncientBoneScythe>()
                .AddIngredient<BloodTithe>()
                .AddIngredient<DebtCollector>()
                .AddIngredient<GreatBlizzard88>()
                .AddIngredient<CelestialReaper>()
                .AddIngredient<NewMoon>()
                .AddIngredient<ChlorophyteScythe>()
                .AddIngredient<CriticalFailure>()
                .AddIngredient<DemonsFinger>()
                .AddIngredient<SweetHarvester>()
                .AddIngredient<IndustrialSeverence>()
                .AddIngredient<LifesTwilight>()
                .AddIngredient<MeteoriteHarvester>()
                .AddIngredient<MidnightsReap>()
                .AddIngredient<Orcus>()
                .AddIngredient<ScytheofChristmasFuture>()
                .AddIngredient<TrickstersDue>()
                .AddIngredient<TempleReaper>()
                .AddIngredient<WardensHook>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}