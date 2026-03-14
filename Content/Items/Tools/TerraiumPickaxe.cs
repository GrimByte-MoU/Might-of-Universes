using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Tools
{
    public class TerraiumPickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 6;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(platinum: 2);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 250;
            Item.tileBoost += 5;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
            Item.scale = 1.2f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 
                    DustID.TerraBlade, 0f, 0f, 100, default, 2.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }

            Lighting.AddLight(hitbox.Center.ToVector2(), 0.3f, 1.0f, 0.3f);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}