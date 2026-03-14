using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class ScabbardOfTheCore : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Melee) += 25f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.20f;
            player.GetModPlayer<ScabbardOfTheCorePlayer>().hasScabbard = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddIngredient(ItemID.MagmaStone, 1)
                .AddIngredient(ItemID.EyeoftheGolem, 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class ScabbardOfTheCorePlayer : ModPlayer
    {
        public bool hasScabbard = false;

        public override void ResetEffects()
        {
            hasScabbard = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasScabbard && item.DamageType == DamageClass.Melee)
            {
                target.AddBuff(ModContent.BuffType<CoreHeat>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasScabbard && proj.DamageType == DamageClass.Melee)
            {
                target.AddBuff(ModContent.BuffType<CoreHeat>(), 180);
            }
        }
    }
}