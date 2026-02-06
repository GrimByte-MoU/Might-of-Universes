using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Kasurikama : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 60f;

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 95;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<KasurikamaSpinProjectile>();
            Item.shootSpeed = 1f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.maxStack = 1;
            Item.crit += 15;
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
                    int damage = (int)(player.GetWeaponDamage(Item) * 5.0f);
                    float kb = player.GetWeaponKnockback(Item);

                    Projectile.NewProjectile(
                        src,
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<KasurikamaGiantSickle>(),
                        damage,
                        kb,
                        player.whoAmI
                    );

                    SoundEngine.PlaySound(SoundID.Item71, player.Center);
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<KasurikamaSpinProjectile>()] == 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 7)
                .AddIngredient(ItemID.SoulofMight, 7)
                .AddRecipeGroup("IronBar", 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}