using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class MartianTally : ModItem
    {
        private int BuffTimer = 0;
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 125;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MartianTallyBolt>();
            Item.shootSpeed = 14f;
            Item.scale = 1.5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(7f, target.Center);
        }

      public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(70f))
        {
            player.GetAttackSpeed(DamageClass.Generic) += 1f;
            player.GetArmorPenetration(DamageClass.Generic) += 999;
            BuffTimer = 180;
            Main.NewText("70 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }

    for (int i = 0; i < 2; i++)
    {
        Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f)));
        Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
    }
    return false;
}

  public override void UpdateInventory(Player player)
        {
            if (BuffTimer > 0)
            {
                BuffTimer--;
                if (BuffTimer <= 0)
                {
                    player.GetAttackSpeed(DamageClass.Generic) -= 1f;
                    player.GetArmorPenetration(DamageClass.Generic) -= 999;
                }
            }
        }
         public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}