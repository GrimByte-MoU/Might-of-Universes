using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class OrichalcumScythe : ModItem
    {
        private int buffTimer = 0;
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 45;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<OrichalcumScytheProjectile>();
            Item.shootSpeed = 10f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center); // Cal

            if (!target.active)
            {
                reaper.AddSoulEnergy(3f, target.Center); // Cal
            }
        }

 public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(40f))
        {
            player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
            player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.15f;
            buffTimer = 240; // 4 seconds
            Main.NewText("40 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }

    position.Y -= Item.height / 2;
    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    return false;
}

        public override void UpdateInventory(Player player)
        {
            if (buffTimer > 0)
            {
                buffTimer--;
                if (buffTimer <= 0)
                {
                player.GetAttackSpeed(DamageClass.Generic) -= 0.15f;
                player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) -= 0.15f;
                }
            }
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.OrichalcumBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}