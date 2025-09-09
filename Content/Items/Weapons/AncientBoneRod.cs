using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AncientBoneRod : ModItem
    {
        public enum RodMode
        {
            Hunt = 0,
            Spray = 1
        }

        private const int PelletsPerUse = 5;
        private const float SprayConeDegrees = 30f;
        private const float PelletSpeed = 12f;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.reuseDelay = 0;
            Item.UseSound = SoundID.Item8;

            Item.DamageType = DamageClass.Magic;
            Item.damage = 80;
            Item.knockBack = 2f;
            Item.crit += 4;
            Item.mana = 12;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
            Item.shoot = ModContent.ProjectileType<TarPellet>();
            Item.shootSpeed = 16f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            var mp = player.GetModPlayer<AncientBoneRodPlayer>();

            // Right-click: swap mode
            if (player.altFunctionUse == 2)
            {
                mp.RodMode = (RodMode)(((int)mp.RodMode + 1) % 2);
                if (Main.myPlayer == player.whoAmI)
                {
                    string modeName = mp.RodMode == RodMode.Hunt ? "Hunt" : "Spray";
                    CombatText.NewText(player.getRect(), new Color(200, 230, 255), modeName);
                }
                return false;
            }

            if (mp.RodMode == RodMode.Hunt)
            {
                Item.channel = false;
                Item.autoReuse = false;
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.mana = 12;
                Item.UseSound = SoundID.Item8;
            }
            else 
            {
                Item.channel = false;
                Item.autoReuse = true;
                Item.useTime = 6;
                Item.useAnimation = 6;
                Item.mana = 4;
                Item.UseSound = SoundID.Item34;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
            int type, int damage, float knockback)
        {
            var mp = player.GetModPlayer<AncientBoneRodPlayer>();
            Vector2 cursor = Main.MouseWorld;

            if (mp.RodMode == RodMode.Hunt)
            {

                const int spikeCount = 16;
                const float radius = 160f;
                const float spikeSpeed = 20f;
                int spikeDamage = (int)(damage * 0.50f);

                for (int i = 0; i < spikeCount; i++)
                {
                    float angle = MathHelper.TwoPi * i / spikeCount;
                    Vector2 spawnPos = cursor + angle.ToRotationVector2() * radius;
                    Vector2 vel = (cursor - spawnPos).SafeNormalize(Vector2.Zero) * spikeSpeed;

                    Projectile.NewProjectile(
                        source,
                        spawnPos,
                        vel,
                        ModContent.ProjectileType<AncientBoneSpike>(),
                        spikeDamage,
                        0f,
                        player.whoAmI,
                        ai0: cursor.X,
                        ai1: cursor.Y
                    );
                }
                return false;
            }
            else
            {
                Vector2 origin = player.MountedCenter;
                Vector2 baseDir = (cursor - origin).SafeNormalize(Vector2.UnitX);
                float halfCone = SprayConeDegrees * 0.5f;

                for (int i = 0; i < PelletsPerUse; i++)
                {
                    float angle = MathHelper.ToRadians(Main.rand.NextFloat(-halfCone, halfCone));
                    float speed = PelletSpeed * Main.rand.NextFloat(0.85f, 1.05f);
                    Vector2 vel = baseDir.RotatedBy(angle) * speed;

                    Projectile.NewProjectile(
                        source,
                        origin,
                        vel,
                        ModContent.ProjectileType<TarPellet>(),
                        damage,
                        knockback * 0.5f,
                        player.whoAmI
                    );
                }
                return false;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BoneWand, 1)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 5)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}