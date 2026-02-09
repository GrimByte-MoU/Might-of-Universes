using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft. Xna.Framework;
using System;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses. Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SoulbinderEmblem : ModItem
    {
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
            player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetDamage<ReaperDamageClass>() += 0.12f;
            player.GetCritChance(DamageClass.Magic) += 10;
            player. GetCritChance<ReaperDamageClass>() += 10f;
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
        private int damageToManaCooldown = 0;
        private int manaToSoulCooldown = 0;

        public override void ResetEffects()
        {
            hasSoulbinderEmblem = false;
        }

        public override void PostUpdate()
        {
            if (damageToManaCooldown > 0)
                damageToManaCooldown--;

            if (manaToSoulCooldown > 0)
                manaToSoulCooldown--;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (! hasSoulbinderEmblem || damageToManaCooldown > 0) return;

            modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
            {
                int manaDamage = Math.Min(info. Damage / 2, Player.statMana);
                
                if (manaDamage > 0)
                {
                    Player.statMana -= manaDamage;
                    info. Damage -= manaDamage;

                    damageToManaCooldown = 180;

                    // Visual effect
                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDust(Player.position, Player.width, Player.height, 
                            DustID.Clentaminator_Purple, Main.rand.NextFloat(-2f, 2f), 
                            Main.rand.NextFloat(-2f, 2f), 0, default, 1.5f);
                    }

                    if (Main.myPlayer == Player.whoAmI)
                    {
                        CombatText.NewText(Player.getRect(), Color.Blue, "-" + manaDamage + " Mana", false);
                    }
                }
            };
        }
        public override void OnConsumeMana(Item item, int manaConsumed)
        {
            if (! hasSoulbinderEmblem || manaToSoulCooldown > 0) return;

            if (manaConsumed >= 20)
            {
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                int soulGain = manaConsumed / 2;
                reaperPlayer. AddSoulEnergy(soulGain, Player.Center);

                manaToSoulCooldown = 60;

                // Visual effect
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, 
                        DustID.PurificationPowder, Main.rand.NextFloat(-1f, 1f), 
                        Main.rand.NextFloat(-1f, 1f), 0, Color.Purple, 1.2f);
                }

                if (Main.myPlayer == Player. whoAmI)
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "+" + soulGain + " Souls", false);
                }
            }
        }
    }
}