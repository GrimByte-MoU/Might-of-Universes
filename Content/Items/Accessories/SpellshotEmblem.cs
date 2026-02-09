using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items. Accessories
{
    public class SpellshotEmblem :  ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetDamage(DamageClass. Ranged) += 0.12f;
            player.GetCritChance(DamageClass.Magic) += 10;
            player.GetCritChance(DamageClass. Ranged) += 10;
            player.GetModPlayer<SpellshotPlayer>().spellshotEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentNebula, 10)
                .AddIngredient(ItemID.FragmentVortex, 10)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class SpellshotPlayer : ModPlayer
    {
        public bool spellshotEffect;

        public override void ResetEffects()
        {
            spellshotEffect = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (! spellshotEffect || proj. owner != Player.whoAmI) return;

            if (proj.DamageType == DamageClass.Ranged && Main.rand.NextBool(10))
            {
                Player.statMana = Math.Min(Player.statMana + 20, Player.statManaMax2);

                if (Main.rand.NextBool(3))
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "+20 Mana", false);
                }
                
                if (Main.rand.NextBool(5))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust. NewDust(Player.position, Player.width, Player.height, 
                            DustID. MagicMirror, 0, 0, 100, Color.Cyan, 1.0f);
                    }
                }
            }
            if (proj.DamageType == DamageClass.Magic && Main.rand.NextBool(10))
            {
                for (int i = 54; i < 58; i++)
                {
                    Item ammo = Player.inventory[i];
                    
                    if (! ammo.IsAir && ammo.ammo > 0 && ammo.stack < ammo.maxStack)
                    {
                        ammo. stack++;

                        if (Main.rand. NextBool(3))
                        {
                            CombatText.NewText(Player.getRect(), Color.Yellow, "+1 " + ammo.Name, false);
                        }
                        
                        if (Main.rand.NextBool(5))
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                Dust.NewDust(Player.position, Player.width, Player.height, 
                                    DustID. Smoke, 0, 0, 100, Color.Yellow, 1.0f);
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}