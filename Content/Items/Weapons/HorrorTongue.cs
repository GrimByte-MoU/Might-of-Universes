using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class HorrorTongue : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Bladetongue);
            Item.rare = ItemRarityID.Lime;
            Item.damage = 80;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spread = MathHelper.ToRadians(5f);
            Vector2 v1 = velocity.RotatedBy(-spread * 0.5f);
            Vector2 v2 = velocity.RotatedBy( spread * 0.5f);

            Projectile.NewProjectile(source, position, v1, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, v2, type, damage, knockback, player.whoAmI);
            return false; // suppress default single shot
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageDone <= 0 || player.moonLeech) return;
            int heal = Utils.Clamp((int)(damageDone * 0.05f), 1, 50);
            player.statLife = Utils.Clamp(player.statLife + heal, 0, player.statLifeMax2);
            player.HealEffect(heal, broadcast: true);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bladetongue, 1)
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}