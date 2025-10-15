using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class PrimalPlateMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 23;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.35f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrimalPlateChestplate>() &&
                   legs.type == ModContent.ItemType<PrimalPlateGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            string key = ModKeybindManager.ArmorAbility?.GetAssignedKeys().Count > 0 
                ? ModKeybindManager.ArmorAbility.GetAssignedKeys()[0] 
                : "[Unbound]";
            
            player.setBonus = $"+100% pacifist damage, -30% damage to all classes\nEight Primal Spikes orbit close dealing 125 damage on contact and inflicting Tarred\nSpikes disappear on hit and respawn after 5 seconds. Press '{key}' to launch all spikes at the cursor";
            player.GetModPlayer<PrimalPlatePlayer>().hasPrimalPlateSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 8)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}