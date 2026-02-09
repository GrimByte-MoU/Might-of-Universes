using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SigilOfWinter : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string key = ModKeybindManager.Ability2 != null && ModKeybindManager.Ability2.GetAssignedKeys().Count > 0
                ? string.Join(", ", ModKeybindManager.Ability2.GetAssignedKeys())
                : "Unbound";
            string line =
                $"Press [{key}] to activate: gain +30 defense and +25% damage reduction for 5s, but lose 90% movement speed\n" +
                "This effect ends early if you are hit.\n" +
                "If it completes, heal 50 HP and remove all debuffs\n" +
                "45 second cooldown";
            tooltips.Insert(1, new TooltipLine(Mod, "SigilOfWinterEffect", line));
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SigilOfWinterPlayer>().hasSigilOfWinter = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 5)
                .AddIngredient(ItemID.FrozenTurtleShell, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}