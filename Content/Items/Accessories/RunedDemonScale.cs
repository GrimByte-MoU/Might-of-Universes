using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content. Items.Buffs;
using MightofUniverses. Content.Items.Materials;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Accessories
{
    public class RunedDemonScale :  ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RunedDemonScalePlayer>().hasRunedDemonScale = true;
            player.GetDamage(DamageClass.Generic) += 0.10f;
            player.GetAttackSpeed(DamageClass. Generic) += 0.10f;
            player.statLifeMax2 -= 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 5)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 5)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ItemID.TissueSample, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class RunedDemonScalePlayer : ModPlayer
    {
        public bool hasRunedDemonScale = false;

        public override void ResetEffects()
        {
            hasRunedDemonScale = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC. HitInfo hit, int damageDone)
        {
            if (hasRunedDemonScale && damageDone > 0)
            {
                ApplyDebuffs(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC. HitInfo hit, int damageDone)
        {
            if (hasRunedDemonScale && damageDone > 0 && proj.owner == Player.whoAmI)
            {
                ApplyDebuffs(target);
            }
        }

        private void ApplyDebuffs(NPC target)
        {
            if (target != null && target.active && ! target.friendly)
            {
                target. AddBuff(ModContent.BuffType<Demonfire>(), 120);
                target.AddBuff(ModContent.BuffType<HellsMark>(), 120);
                if (Main.rand.NextBool(3))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                        Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Torch, velocity.X, velocity.Y);
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                    }
                }
            }
        }
    }
}