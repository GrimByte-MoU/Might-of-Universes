using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class CyberOneVisor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.40f;
            player.findTreasure = true;
            player.detectCreature = true;
            player.dangerSense = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CyberOneChestplate>() 
                && legs.type == ModContent.ItemType<CyberOneLegArmor>();
        }

        public override void UpdateArmorSet(Player player)
        {
            string key = ModKeybindManager.ArmorAbility?.GetAssignedKeys().Count > 0 
                ? ModKeybindManager.ArmorAbility.GetAssignedKeys()[0] 
                : "[Unbound]";

            player.setBonus = $"Grants flight and slow fall\n" +
                             $"+100% nonweapon damage\n" +
                             $"Press '{key}' to teleport to cursor and leave an EMP Trap\n" +
                             $"EMP Trap deals heavy damage and Paralyzes non-boss enemies for 3 seconds\n" +
                             $"The teleport has a 5 second cooldown";
            
            player.GetModPlayer<CyberOnePlayer>().hasCyberOneSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 8)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 8)
                .AddIngredient(ItemID.Nanites, 20)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}