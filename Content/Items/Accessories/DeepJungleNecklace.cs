using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class DeepJungleNecklace : ModItem
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
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.20f;
            var modPlayer = player.GetModPlayer<DeepJungleNecklacePlayer>();
            modPlayer.hasNecklace = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkToothNecklace, 1)
                .AddIngredient(ItemID.SummonerEmblem, 1)
                .AddIngredient(ItemID.PygmyNecklace, 1)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class DeepJungleNecklacePlayer : ModPlayer
    {
        public bool hasNecklace = false;

        public override void ResetEffects()
        {
            hasNecklace = false;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasNecklace && proj.DamageType == DamageClass.Summon)
            {
                modifiers.ArmorPenetration += 20;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasNecklace && proj.DamageType == DamageClass.Summon)
            {
                target.AddBuff(ModContent.BuffType<NaturesToxin>(), 180);
            }
        }
    }
}