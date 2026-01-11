using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Util;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using System;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class BiomeCleanser : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 200f;

        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 90;
            Item.damage = 150;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.value = Item.sellPrice(gold: 24);
            Item.UseSound = SoundID.Item71;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BiomeCleanserSpinProjectile>();
            Item.shootSpeed = 1f;
            Item.maxStack = 1;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<BiomeCleanserSpinProjectile>()] == 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 pos, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int scost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (player.GetModPlayer<ReaperPlayer>().ConsumeSoulEnergy(scost))
                {
                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int spearType = ModContent.ProjectileType<BiomeCleanserSpearProjectile>();
                    int spearDamage = (int)(Item.damage * 3.5f);
                    float angleStep = MathHelper.TwoPi / 16f;
                    float radius = 20f;
                    for (int i = 0; i < 16; i++)
                    {
                        float angle = i * angleStep;
                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                        Vector2 spawn = player.Center + offset;
                        Vector2 vel = offset.SafeNormalize(Vector2.UnitY) * 6f;
                        Projectile.NewProjectile(src, spawn, vel, spearType, spearDamage, 6f, player.whoAmI);
                    }
                }
            }
        }
    }
}