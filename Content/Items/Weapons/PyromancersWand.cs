namespace MightofUniverses.Content.Items.Weapons
{
    public class PyromancersWand : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 150;
            Item.DamageType = DamageClass.Summon;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.crit = 5;
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<FireMageMinion>();
            Item.shootSpeed = 10f;
            Item.buffType = ModContent.BuffType<FireMageBuff>();
            Item.buffTime = 2;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.owner == player.whoAmI && p.type == type)
                    p.Kill();
            }

            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddIngredient(ItemID.ImpStaff, 1)
                .AddIngredient(ItemID.MeteoriteBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}