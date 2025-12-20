using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class MegaBuster : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 140;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 54;
            Item.height = 24;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.MegaBusterShot>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.None;
            Item.channel = true;
            Item.scale = 1.2f;
            Item.maxStack = 1;
        }

        public override void HoldItem(Player player)
        {
            var modPlayer = player.GetModPlayer<MegaBusterPlayer>();
            
            Item.autoReuse = !modPlayer.isChargeMode;
            
            if (Main.mouseRight && Main.mouseRightRelease)
            {
                modPlayer.isChargeMode = !modPlayer.isChargeMode;
                
                string mode = modPlayer.isChargeMode ? "Charge Shot" : "Standard Shot";
                Main.NewText($"Mega Buster: {mode} Mode", Color.Cyan);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        public override bool CanUseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<MegaBusterPlayer>();
            
            if (modPlayer.isChargeMode)
            {
                return !player.channel;
            }
            
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<MegaBusterPlayer>();
            
            if (modPlayer.isChargeMode)
            {
                modPlayer.ReleaseCharge(player, position, velocity);
                return false;
            }
            else
            {
                Projectile.NewProjectile(
                    source,
                    position,
                    velocity,
                    ModContent.ProjectileType<Projectiles.MegaBusterShot>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
                return false;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.VenusMagnum)
                .AddIngredient(ItemID.MartianConduitPlating, 20)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}