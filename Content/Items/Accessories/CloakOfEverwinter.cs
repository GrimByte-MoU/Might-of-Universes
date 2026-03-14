using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CloakOfEverwinter : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(platinum: 1, gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaCost -= 0.12f;
            player.manaFlower = true;
            player.aggro -= 400;
            Player.tileRangeX += 5;
            Player.tileRangeY += 3;
            player.manaMagnet = true;
            player.GetModPlayer<CloakOfEverwinterPlayer>().hasCloak = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ArcaneFlower, 1)
                .AddIngredient(ItemID.MagnetFlower, 1)
                .AddIngredient(ItemID.ManaCloak, 1)
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 5)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class CloakOfEverwinterPlayer : ModPlayer
    {
        public bool hasCloak = false;

        public override void ResetEffects()
        {
            hasCloak = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasCloak && item.DamageType == DamageClass.Magic)
            {
                target.AddBuff(ModContent.BuffType<SheerCold>(), 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasCloak && proj.DamageType == DamageClass.Magic)
            {
                target.AddBuff(ModContent.BuffType<SheerCold>(), 180);
            }
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (hasCloak && Player.whoAmI == Main.myPlayer)
            {
                int starCount = Main.rand.Next(3, 6);
                
                for (int i = 0; i < starCount; i++)
                {
                    Vector2 starPos = Player.Center + new Vector2(Main.rand.Next(-400, 400), -600);
                    Vector2 starVel = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(4f, 7f));
                    
                    int damage = 30 + (int)(Player.GetDamage(DamageClass.Magic).Additive * 50f);
                    
                    Projectile.NewProjectile(
                        Player.GetSource_OnHurt(info.DamageSource),
                        starPos,
                        starVel,
                        ProjectileID.StarVeilStar,
                        damage,
                        0f,
                        Player.whoAmI
                    );
                }
            }
        }
    }
}