using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Chillrend : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Frostbrand);
            Item.damage = 80;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 0f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Frostfeed>(), 180);
            if (!target.boss && target.type != NPCID.TargetDummy && Main.rand.NextFloat() < 0.26f)
            {
                target.AddBuff(ModContent.BuffType<Paralyze>(), 120);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Frostbrand, 1)
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}