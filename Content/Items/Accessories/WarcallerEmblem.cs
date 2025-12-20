using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items. Accessories
{
    public class WarcallerEmblem :  ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass. Melee) += 0.12f;
            player.GetDamage(DamageClass. Summon) += 0.12f;
            player.GetCritChance(DamageClass. Melee) += 10;
            player.GetModPlayer<WarcallerPlayer>().warcallerEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddIngredient(ItemID.FragmentStardust, 10)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class WarcallerPlayer : ModPlayer
    {
        public bool warcallerEffect;

        public override void ResetEffects()
        {
            warcallerEffect = false;
        }

        public override void UpdateEquips()
        {
            if (warcallerEffect)
            {
                Player.statDefense += Player.numMinions * 2;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (warcallerEffect && item.DamageType == DamageClass. Melee)
            {
                Player.MinionAttackTargetNPC = target.whoAmI;
            }
        }
    }
}