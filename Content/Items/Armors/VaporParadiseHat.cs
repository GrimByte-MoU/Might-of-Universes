using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class VaporParadiseHat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.25f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VaporParadiseArmoredShirt>() &&
                   legs.type == ModContent.ItemType<VaporParadiseShoes>();
        }

        public override void UpdateArmorSet(Player player)
        {
            string key = ModKeybindManager.ArmorAbility.GetAssignedKeys().Count > 0 
                ? ModKeybindManager.ArmorAbility.GetAssignedKeys()[0] 
                : "[Unbound]";
            
            player.setBonus = $"+60% pacifist damage, -30% damage to all classes\nFour Vaporwave Suns orbit the player dealing 45 damage\nPress '{key}' to summon 4 more suns that orbit the player twice as fast, orbit further and deal double damage for 5 seconds\nThis ability has a 20 second cooldown";
            player.GetModPlayer<VaporParadisePlayer>().hasVaporParadiseSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VaporFragment>(), 10)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}