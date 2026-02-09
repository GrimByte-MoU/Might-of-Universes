using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using System;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ArcaneGladius : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 72;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Pink;
            Item.mana = 12;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.channel = false;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ModContent.ProjectileType<ArcaneGladiusProjectile>();
            Item.shootSpeed = 12f;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numProjectiles = 3;
            float radius = 160f;

            Vector2 cursorPos = Main.MouseWorld;

            for (int i = 0; i < numProjectiles; i++)
            {
                float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(0f, radius);
                Vector2 spawnOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
                Vector2 spawnPos = player.Center + spawnOffset;

                Vector2 toCursor = cursorPos - spawnPos;
                if (toCursor.Length() < 0.01f) toCursor = new Vector2(0, -1);
                toCursor.Normalize();
                Vector2 projVelocity = toCursor * Item.shootSpeed;

                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    projVelocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );
            }
            return false;
        }
        public override void OnSpawn(IEntitySource source)
{
    Player player = Main.player[Main.myPlayer];
    float meleeBoost = player.GetDamage(DamageClass.Melee).Additive;
    float magicBoost = player.GetDamage(DamageClass.Magic).Additive;
    float totalBoost = 1f + meleeBoost + magicBoost;

    player.HeldItem.damage = (int)(player.HeldItem.damage * totalBoost);
}
    }
}
