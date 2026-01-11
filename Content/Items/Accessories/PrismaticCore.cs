using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses. Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticCore : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PrismaticCorePlayer>().hasPrismaticCore = true;
            player.statDefense += 7;
            player.statLifeMax2 += 65;
            player.GetDamage(DamageClass.Generic) += 0.10f;
            float hue = Main.GameUpdateCount * 0.02f % 1f;
            Color rainbowColor = Main.hslToRgb(hue, 1f, 0.5f);
            Lighting.AddLight(player.Center, rainbowColor.R / 255f * 0.5f, rainbowColor.G / 255f * 0.5f, rainbowColor.B / 255f * 0.5f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HallowedCore>())
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PrismaticCorePlayer : ModPlayer
    {
        public bool hasPrismaticCore = false;

        public override void ResetEffects()
        {
            hasPrismaticCore = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticCore && damageDone > 0)
            {
                ApplyPrismaticRend(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasPrismaticCore && damageDone > 0 && proj.owner == Player.whoAmI)
            {
                ApplyPrismaticRend(target);
            }
        }

        private void ApplyPrismaticRend(NPC target)
        {
            if (target != null && target.active && !target. friendly)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 120);
                if (Main.rand. NextBool(3))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        float hue = i / 8f;
                        Color rainbowColor = Main.hslToRgb(hue, 1f, 0.6f);
                        
                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
                        Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.RainbowMk2, velocity.X, velocity. Y);
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                        dust. color = rainbowColor;
                    }
                    float currentHue = Main.GameUpdateCount * 0.05f % 1f;
                    Color flashColor = Main.hslToRgb(currentHue, 1f, 0.5f);
                    Lighting. AddLight(target.Center, flashColor.R / 255f, flashColor.G / 255f, flashColor.B / 255f);
                }
            }
        }
    }
}