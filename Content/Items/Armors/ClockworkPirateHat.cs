using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ClockworkPirateHat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.10f;
            player.GetModPlayer<ClockworkPlayer>().ammoSaveChance += 0.10f;
            player.endurance += 0.01f;
            player.GetModPlayer<ClockworkPlayer>().defensePenFlat += 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ClockworkPirateVest>() &&
                   legs.type == ModContent.ItemType<ClockworkPiratePants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+15% damage against bosses\n 3 gears orbits the player that when hit by a ranged projectile will increase it's damage, armor penetration and velocity\n Grants 4 seconds of flight and fall damage immunity";
            player.GetModPlayer<ClockworkPlayer>().hasClockworkSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 8)
                .AddIngredient(ItemID.Cog, 10)
                .AddIngredient(ItemID.Wire, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
