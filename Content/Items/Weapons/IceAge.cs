using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;
using Terraria.DataStructures;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class IceAge : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 225f;

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.damage = 130;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IceAgeSpike>();
            Item.shootSpeed = 16f;
            Item.noMelee = false;
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
                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);

                    SoundEngine.PlaySound(SoundID.Item120, player.Center);

                    for (int i = 0; i < 8; i++)
                    {
                        Projectile.NewProjectile(
                            src,
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<IceAgeBlizzardBall>(),
                            (int)(damage * 3.0f),
                            kb,
                            player.whoAmI,
                            0f,
                            i
                        );
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 3.0f);
                        dust.noGravity = true;
                        dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
                    }
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SheerCold>(), 180);
            
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(8f, target.Center);

            if (!target.active)
            {
                reaper.AddSoulEnergy(8f, target.Center);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GreatBlizzard88>()
                .AddIngredient(ItemID.LunarBar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}