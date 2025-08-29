using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class BloodTithe : ModItem
    {
         private int buffTimer = 0;
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 95;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BloodTitheWave>();
            Item.shootSpeed = 8f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center); // Cal

            if (!target.active)
            {
                reaper.AddSoulEnergy(5f, target.Center); // Cal
            }
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(50f))
        {
            player.lifeRegen -= 5;
            player.GetAttackSpeed(DamageClass.Generic) += 0.3f;
            player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.25f;
            buffTimer = 300; // 4 seconds
            Main.NewText("50 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }
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
                player.lifeRegen += 5;
                player.GetAttackSpeed(DamageClass.Generic) -= 0.3f;
                player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) -= 0.25f;
                }
            }
        }


         public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}