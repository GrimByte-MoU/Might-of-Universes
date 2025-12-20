using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content. Items.Accessories
{
    public class WraithwalkerEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item. width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Red;
            Item. accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player. GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetDamage<ReaperDamageClass>() += 0.12f;
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetCritChance<ReaperDamageClass>() += 10f;
            player.GetModPlayer<WraithwalkerPlayer>().hasWraithwalkerEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentVortex, 15)
                .AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1)
                .AddIngredient(ItemID.DestroyerEmblem, 1)
                .AddTile(TileID. LunarCraftingStation)
                .Register();
        }
    }

    public class WraithwalkerPlayer : ModPlayer
    {
        public bool hasWraithwalkerEmblem = false;
        private int ammoConsumeTimer = 0;

        public override void ResetEffects()
        {
            hasWraithwalkerEmblem = false;
        }

        public override void PostUpdateEquips()
        {
            if (! hasWraithwalkerEmblem) return;

            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            int soulEnergy = (int)reaperPlayer.soulEnergy;

            // Per 10 souls:  +0.5% ranged damage, +0.5% attack speed, +1 armor pen
            // Capped at 30 stacks (300 souls max)
            int stacks = Math.Min(soulEnergy / 10, 30);

            Player.GetDamage(DamageClass.Ranged) += stacks * 0.005f;
            Player.GetAttackSpeed(DamageClass.Ranged) += stacks * 0.005f;
            Player.GetArmorPenetration(DamageClass. Ranged) += stacks;

            // Every 2 seconds:  Consume 1 ammo for souls
            ammoConsumeTimer++;
            if (ammoConsumeTimer >= 120)
            {
                ammoConsumeTimer = 0;

                // Find ammo in inventory
                for (int i = 54; i < 58; i++)
                {
                    Item ammo = Player.inventory[i];
                    if ((ammo. ammo == AmmoID.Bullet || ammo.ammo == AmmoID. Arrow) && ammo.stack > 0)
                    {
                        int soulGain = ammo.damage / 2;
                        ammo.stack--;
                        if (ammo.stack <= 0)
                            ammo.TurnToAir();

                        reaperPlayer.AddSoulEnergy(soulGain, Player.Center);

                        // Visual effect
                        for (int j = 0; j < 5; j++)
                        {
                            Dust. NewDust(Player.position, Player.width, Player.height, 
                                DustID. Smoke, 0, 0, 100, Color.Gray, 1.5f);
                        }

                        if (Main.myPlayer == Player. whoAmI)
                        {
                            CombatText. NewText(Player.getRect(), Color. Cyan, "+" + soulGain + " Souls", false);
                        }
                        break;
                    }
                }
            }
        }
    }
}