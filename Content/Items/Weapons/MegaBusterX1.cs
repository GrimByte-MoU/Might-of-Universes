namespace MightofUniverses.Content.Items.Weapons
{
    public class MegaBusterX1 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 54;
            Item.height = 24;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.MegaBusterShot>();
            Item.shootSpeed = 24f;
            Item.useAmmo = AmmoID.None;
            Item.channel = true;
            Item.scale = 1.2f;
            Item.maxStack = 1;
        }

        public override void HoldItem(Player player)
        {
            // Use the X1-specific player class so charge mode toggles the correct state
            var modPlayer = player.GetModPlayer<MegaBusterX1Player>();
            
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
            var modPlayer = player.GetModPlayer<MegaBusterX1Player>();
            
            if (modPlayer.isChargeMode)
            {
                return !player.channel;
            }
            
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<MegaBusterX1Player>();
            
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
                .AddIngredient(ModContent.ItemType<MegaBuster>())
                .AddIngredient(ItemID.LunarBar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}