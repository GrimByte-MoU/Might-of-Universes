using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AdamantiteScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 50;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AdamantiteScytheProjectile>();
            Item.shootSpeed = 10f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(4f, target.Center); // Cal

            if (!target.active)
            {
                reaper.AddSoulEnergy(4f, target.Center); // Cal
            }
            target.AddBuff(BuffID.Electrified, 180);
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(40f))
        {
            Projectile.NewProjectile(source, position, velocity, 
                ModContent.ProjectileType<AdamantiteSphereProjectile>(), 
                damage * 3, knockback, player.whoAmI);
            return false;
        }
    }
    return true;
}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}