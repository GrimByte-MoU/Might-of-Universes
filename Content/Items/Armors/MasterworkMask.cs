using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class MasterworkMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.30f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MasterworkBoilerplate>() &&
                   legs.type == ModContent.ItemType<MasterworkTreads>();
        }

        public override void UpdateArmorSet(Player player)
        {
            string key = ModKeybindManager.ArmorAbility?.GetAssignedKeys().Count > 0 
                ? ModKeybindManager.ArmorAbility.GetAssignedKeys()[0] 
                : "[Unbound]";
            
            player.setBonus = $"+80% pacifist damage, -30% damage to all classes\nAll pacifist damage inflicts Shred for 3 seconds\nPress '{key}' to fire 15 homing Twirly Whirlies that home in on nearby enemies dealing 100 damage\nThis ability has a 10 second cooldown)";
            player.GetModPlayer<MasterworkPlayer>().hasMasterworkSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.Cog, 20)
                .AddIngredient(ItemID.Wire, 23)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}