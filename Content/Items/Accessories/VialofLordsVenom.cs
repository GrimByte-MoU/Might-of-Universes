using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class VialofLordsVenom : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ModContent.RarityType<TacticianRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VialofLordsVenomPlayer>().hasVialofLordsVenom = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TacticiansEssence>(), 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class VialofLordsVenomPlayer : ModPlayer
    {
        public bool hasVialofLordsVenom = false;

        public override void ResetEffects()
        {
            hasVialofLordsVenom = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVialofLordsVenom && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<LordsVenom>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasVialofLordsVenom && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<LordsVenom>(), 180);
            }
        }
    }
}