using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content. Items.Materials;
using System. Collections.Generic;

namespace MightofUniverses.Content.Items.Accessories
{
    public class HallowedCore : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HallowedCorePlayer>().hasHallowedCore = true;
            player.statDefense += 5;
            player.statLifeMax2 += 50;
            player.GetDamage(DamageClass.Generic) += 0.07f;
            Lighting.AddLight(player.Center, 0.3f, 0.5f, 0.8f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HallowedCorePlayer : ModPlayer
    {
        public bool hasHallowedCore = false;

        public override void ResetEffects()
        {
            hasHallowedCore = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasHallowedCore && damageDone > 0)
            {
                ApplyRebukingLight(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasHallowedCore && damageDone > 0 && proj. owner == Player.whoAmI)
            {
                ApplyRebukingLight(target);
            }
        }

        private void ApplyRebukingLight(NPC target)
        {
            if (target != null && target.active && !target.friendly)
            {
                target. AddBuff(ModContent.BuffType<RebukingLight>(), 120);
                if (Main.rand.NextBool(3))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                        Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.HallowedTorch, velocity.X, velocity. Y);
                        dust.noGravity = true;
                        dust.scale = 1.3f;
                        dust.color = Color.Gold;
                    }
                    Lighting.AddLight(target.Center, 0.8f, 0.8f, 0.3f);
                }
            }
        }
    }
}