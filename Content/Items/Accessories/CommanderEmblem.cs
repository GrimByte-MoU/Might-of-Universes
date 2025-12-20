using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items. Accessories
{
    public class CommanderEmblem : ModItem
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
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetDamage(DamageClass. Summon) += 0.12f;
            player.GetCritChance(DamageClass. Ranged) += 10;
            player.GetModPlayer<CommanderPlayer>().commanderEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentVortex, 10)
                .AddIngredient(ItemID.FragmentStardust, 10)
                .AddIngredient(ItemID. DestroyerEmblem)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class CommanderPlayer : ModPlayer
    {
        public bool commanderEffect;

        public override void ResetEffects()
        {
            commanderEffect = false;
        }

        public override void UpdateEquips()
        {
            if (commanderEffect)
            {
                // +3% ranged attack speed per minion (max 30% with 10 minions)
                Player.GetAttackSpeed(DamageClass.Ranged) += Player.numMinions * 0.03f;
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (! commanderEffect || proj.DamageType != DamageClass.Summon) return;

            // Below 50% HP:  Minions deal bonus damage equal to held ammo damage
            if (Player.statLife <= Player.statLifeMax2 * 0.5f)
            {
                Item heldItem = Player.HeldItem;
                int ammoDamage = 0;

                // Check if holding a ranged weapon
                if (heldItem.useAmmo > 0)
                {
                    // Find ammo in inventory
                    for (int i = 54; i < 58; i++)
                    {
                        Item ammo = Player.inventory[i];
                        if (ammo.ammo == heldItem.useAmmo && ammo.stack > 0)
                        {
                            ammoDamage = ammo.damage;
                            break;
                        }
                    }
                }

                if (ammoDamage > 0)
                {
                    modifiers.FlatBonusDamage += ammoDamage;
                }
            }
        }
    }
}