using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Accessories
{
    public class ConjurerEmblem : ModItem
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
            player.GetDamage(DamageClass. Magic) += 0.12f;
            player.GetDamage(DamageClass. Summon) += 0.12f;
            player.GetCritChance(DamageClass. Magic) += 10;
            player.GetModPlayer<ConjurerPlayer>().conjurerEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID. FragmentNebula, 10)
                .AddIngredient(ItemID.FragmentStardust, 10)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class ConjurerPlayer : ModPlayer
    {
        public bool conjurerEffect;
        private int manaRestoreCooldown = 0;

        public override void ResetEffects()
        {
            conjurerEffect = false;
        }

        public override void PostUpdate()
        {
            if (manaRestoreCooldown > 0)
                manaRestoreCooldown--;
        }

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (conjurerEffect && item.DamageType == DamageClass. Magic)
            {
                crit += Player.numMinions * 2;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (conjurerEffect && proj.DamageType == DamageClass. Summon && manaRestoreCooldown <= 0)
            {
                Player.statMana = Math.Min(Player.statMana + 5, Player.statManaMax2);
                manaRestoreCooldown = 30;

                // Visual feedback
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDust(Player. position, Player.width, Player. height, 
                        DustID.MagicMirror, 0, 0, 100, Color.Blue, 1.0f);
                }
            }
        }
    }
}