using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Input;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class PrismaticWizardHat : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrismaticChestplate>()
                && legs.type == ModContent.ItemType<PrismaticGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
        string key = ModKeybindManager.ArmorAbility?.GetAssignedKeys().Count > 0 
                ? ModKeybindManager.ArmorAbility.GetAssignedKeys()[0] 
                : "[Unbound]";

            player.setBonus = "+10% magic damage and magic weapons inflict Prismatic Rend for 3 seconds.\n Press '{key}' to restore 200 mana and remove mana sickness.\n This ability has a 5 second cooldown.";
            player.GetDamage(DamageClass.Magic) += 0.10f;
            // Mark the wizard set active on the player's PrismaticPlayer so global hooks and the key logic use it.
            player.GetModPlayer<PrismaticPlayer>().prismaticWizardSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.07f;
            player.GetCritChance(DamageClass.Magic) += 18;
            player.statManaMax2 += 100;
            player.manaCost -= 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}