using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class ElementalBeetle : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.25f;
            player.GetKnockback(DamageClass.Summon) += 4f;
            player.maxMinions += 2;
            player.GetModPlayer<ElementalBeetlePlayer>().hasElementalBeetle = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PrismaticBeetle>()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class ElementalBeetlePlayer : ModPlayer
    {
        public bool hasElementalBeetle;

        public override void ResetEffects()
        {
            hasElementalBeetle = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasElementalBeetle && hit.DamageType == DamageClass.Summon)
            {
                target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);

                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        target.position,
                        target.width,
                        target.height,
                        DustID.RainbowMk2,
                        0f,
                        0f,
                        100,
                        default,
                        1.5f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasElementalBeetle && proj.DamageType == DamageClass.Summon)
            {
                target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);

                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        target.position,
                        target.width,
                        target.height,
                        DustID.RainbowMk2,
                        0f,
                        0f,
                        100,
                        default,
                        1.5f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                }
            }
        }
    }
}