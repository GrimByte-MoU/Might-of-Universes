using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Rarities;
using System;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WarcrimeSash : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TacticianRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WarcrimeSashPlayer>().hasWarcrimeSash = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BeltOfVials>(), 1)
                .AddIngredient(ModContent.ItemType<VialofLordsVenom>(), 1)
                .AddIngredient(ModContent.ItemType<LiquidTerra>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class WarcrimeSashPlayer : ModPlayer
    {
        public bool hasWarcrimeSash = false;
        
        public override void ResetEffects()
        {
            hasWarcrimeSash = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasWarcrimeSash && IsReaperWeapon(item))
            {
                ApplyDebuffs(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasWarcrimeSash && IsReaperProjectile(proj))
            {
                ApplyDebuffs(target);
            }
        }

        private bool IsReaperWeapon(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ReaperDamageClass>();
        }
        
        private bool IsReaperProjectile(Projectile proj)
        {
            return proj.DamageType == ModContent.GetInstance<ReaperDamageClass>();
        }

        private void ApplyDebuffs(NPC target)
        {
            target.AddBuff(ModContent.BuffType<LordsVenom>(), 180);
            target.AddBuff(ModContent.BuffType<Sunfire>(), 240);
            target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 300);
            target.AddBuff(ModContent.BuffType<DeadlyCorrupt>(), 300);
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 360);
            
            for (int i = 0; i < 5; i++)
            {
                int dustType = Main.rand.Next(new int[] {
                    DustID.Poisoned,
                    DustID.Torch,
                    DustID.PurpleTorch,
                    DustID.RainbowTorch
                });
                
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    dustType,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-1f, 1f),
                    100,
                    default,
                    Main.rand.NextFloat(0.7f, 1.0f)
                );
                
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.velocity *= 0.5f;
                dust.scale *= 0.8f;
            }
        }
    }
}