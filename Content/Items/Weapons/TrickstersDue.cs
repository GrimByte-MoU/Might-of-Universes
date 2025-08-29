using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TrickstersDue : ModItem
    {
        private int buffTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 90;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TrickstersDueBolt>();
            Item.shootSpeed = 12f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            
if (ReaperPlayer.SoulReleaseKey.JustPressed)



            {
                if (reaper.ConsumeSoulEnergy(300f))
                {
                    player.lifeRegen += 60;
                    buffTimer = 300; // 5 seconds
                    Main.NewText("300 souls released!", Color.Pink);
                }
                else
                {
                    Main.NewText("Not enough soul energy to activate!", Color.Red);
                }
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            if (buffTimer > 0)
            {
                buffTimer--;
                if (buffTimer <= 0)
                {
                    player.lifeRegen -= 60;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentNebula, 10)
                .AddIngredient(ModContent.ItemType<FairyMetal>(), 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}

