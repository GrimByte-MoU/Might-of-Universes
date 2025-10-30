using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BeltOfVials : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BeltOfVialsPlayer>().hasBeltOfVials = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VialOfCorruption>(), 1)
                .AddIngredient(ModContent.ItemType<PrismaticVial>(), 1)
                .AddIngredient(ModContent.ItemType<TrappedDemonFire>(), 1)
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 3)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class BeltOfVialsPlayer : ModPlayer
    {
        public bool hasBeltOfVials = false;
        
        public override void ResetEffects()
        {
            hasBeltOfVials = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasBeltOfVials && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyDebuffs(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasBeltOfVials && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyDebuffs(target);
            }
        }

        private void ApplyDebuffs(NPC target)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
            target.AddBuff(ModContent.BuffType<Demonfire>(), 300);
            target.AddBuff(ModContent.BuffType<Corrupted>(), 600);
            
            for (int i = 0; i < 3; i++)
            {
                int dustType = Main.rand.Next(new int[] {
                    DustID.RainbowTorch,
                    DustID.Torch,
                    DustID.PurpleTorch
                });
                
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    dustType,
                    Main.rand.NextFloat(-0.5f, 0.5f),
                    Main.rand.NextFloat(-0.5f, 0.5f),
                    100,
                    default,
                    Main.rand.NextFloat(0.5f, 0.8f)
                );
                
                dust.noGravity = true;
                dust.fadeIn = 0.05f;
                dust.velocity *= 0.3f;
                dust.scale *= 0.6f;
            }
        }
    }
}