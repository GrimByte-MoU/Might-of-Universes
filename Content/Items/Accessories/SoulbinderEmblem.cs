using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.Localization;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SoulbinderEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName and Tooltip are set in localization files
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Increase Reaper and Magic damage
            player.GetDamage(DamageClass.Magic) += 0.12f;
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.reaperDamageMultiplier += 1.12f;

            // Increase Reaper and Magic crit chance
            player.GetCritChance(DamageClass.Magic) += 10;
            reaperPlayer.reaperCritChance += 10;

            // Apply the Soulbinder effects
            player.GetModPlayer<SoulbinderEmblemPlayer>().hasSoulbinderEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentNebula, 15)
                .AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1)
                .AddIngredient(ItemID.DestroyerEmblem, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class SoulbinderEmblemPlayer : ModPlayer
    {
        public bool hasSoulbinderEmblem = false;
        private int healthToManaCooldown = 0;

        public override void ResetEffects()
        {
            hasSoulbinderEmblem = false;
        }

        public override void PostUpdate()
        {
            if (!hasSoulbinderEmblem) return;

            // Decrease cooldown timer
            if (healthToManaCooldown > 0)
                healthToManaCooldown--;
        }

        public virtual bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref Player.HurtModifiers modifiers)
        {
            if (!hasSoulbinderEmblem || healthToManaCooldown > 0) return true;

            // Convert damage to mana damage
            int manaDamage = Math.Min(damage, Player.statMana);
            Player.statMana -= manaDamage;
            
            // Any remaining damage goes to health
            damage -= manaDamage;
            
            // Start cooldown (3 seconds)
            healthToManaCooldown = 180;
            
            // Visual effect
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(
                    Player.position,
                    Player.width,
                    Player.height,
                    DustID.Clentaminator_Purple,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    0,
                    default,
                    Main.rand.NextFloat(1f, 1.5f)
                );
            }
            
            // Show text
            if (Main.myPlayer == Player.whoAmI && manaDamage > 0)
            {
                CombatText.NewText(Player.getRect(), Color.Blue, "-" + manaDamage + " Mana");
            }
            
            return damage > 0;
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (!hasSoulbinderEmblem) return;
            
            // Set mana cost to 0 since we'll handle it in OnMissingMana
            mult = 0f;
        }

        public override void OnMissingMana(Item item, int neededMana)
        {
            if (!hasSoulbinderEmblem) return;
            
            // Take health instead of mana
            int healthCost = neededMana;
            
            // Ensure we don't kill the player
            if (healthCost >= Player.statLife - 1)
                healthCost = Player.statLife - 1;
            
            if (healthCost > 0)
            {
                // Reduce health
                Player.statLife -= healthCost;
                
                // Visual effect
                Player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromKey(Player.name + " sacrificed their life force")), 0, 1);
                
                // Show text
                if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(Player.getRect(), Color.Red, "-" + healthCost + " HP");
                }
            }
        }
    }
}
