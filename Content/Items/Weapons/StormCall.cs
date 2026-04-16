using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class StormCall : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<LightningReticle>();
            Item.shootSpeed = 0f;
            Item.mana = 15;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item20;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 targetPosition = Main.MouseWorld;
            
            int targetNPC = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.getRect().Contains(Main.MouseWorld.ToPoint()))
                {
                    targetNPC = i;
                    break;
                }
            }

            Projectile.NewProjectile(source, targetPosition, Vector2.Zero, type, damage, knockback, player.whoAmI, targetNPC);
            
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ItemID.NimbusRod)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}