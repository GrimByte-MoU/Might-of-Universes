using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Accessories
{
    public class TabulatingMachine : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 2);
        }
        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<TabulatingMachinePlayer>().HasTabulatingMachine = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TabulatingMachinePlayer>().HasTabulatingMachine = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 10)
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.GoldBar, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 10)
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.PlatinumBar, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class TabulatingMachinePlayer : ModPlayer
    {
        public bool HasTabulatingMachine;

        public override void ResetEffects()
        {
            HasTabulatingMachine = false;
        }

        public override void PostUpdate()
        {
            if (!HasTabulatingMachine)
            {
                int type = ModContent.ItemType<TabulatingMachine>();
                if (ContainsItem(Player.bank?.item, type) ||
                    ContainsItem(Player.bank2?.item, type) ||
                    ContainsItem(Player.bank3?.item, type) ||
                    ContainsItem(Player.bank4?.item, type))
                {
                    HasTabulatingMachine = true;
                }
            }
        }

        private static bool ContainsItem(Item[] items, int type)
        {
            if (items == null)
                return false;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && !items[i].IsAir && items[i].type == type)
                    return true;
            }
            return false;
        }
    }
}