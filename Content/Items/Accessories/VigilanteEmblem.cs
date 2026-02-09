using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using System;

namespace MightofUniverses.Content.Items. Accessories
{
    public class VigilanteEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item. value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID. Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass. Melee) += 0.12f;
            player.GetDamage(DamageClass. Ranged) += 0.12f;
            player.GetCritChance(DamageClass. Melee) += 10;
            player.GetCritChance(DamageClass. Ranged) += 10;
            player.GetModPlayer<VigilantePlayer>().vigilanteEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddIngredient(ItemID.FragmentVortex, 10)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class VigilantePlayer : ModPlayer
    {
        public bool vigilanteEffect;

        public override void ResetEffects()
        {
            vigilanteEffect = false;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (! vigilanteEffect) return;

            float distance = Vector2.Distance(target.Center, Player.Center) / 16f;
            
            if (proj.DamageType == DamageClass.Melee)
            {
                float bonus = Math.Min(distance / 20f, 0.2f);
                modifiers. SourceDamage += bonus;
            }
            else if (proj.DamageType == DamageClass.Ranged)
            {
                float bonus = Math.Max(0.2f - (distance / 25f), 0f);
                modifiers.SourceDamage += bonus;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (! vigilanteEffect || proj.DamageType != DamageClass.Ranged) return;

            float distance = Vector2.Distance(target.Center, Player.Center) / 16f;

            if (distance < 10f)
            {
                int heal = (int)(damageDone * 0.05f);
                if (heal > 0)
                {
                    Player.statLife = Math.Min(Player.statLife + heal, Player.statLifeMax2);
                    Player.HealEffect(heal);
                }
            }
        }
    }
}