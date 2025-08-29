using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Accessories
{
    public class HallowedMirror : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HallowedMirrorPlayer>().hasHallowedMirror = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 5)
                .AddIngredient(ItemID.MagicMirror)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class HallowedMirrorPlayer : ModPlayer
    {
        public bool hasHallowedMirror;
        private int cooldownTimer = 0;
        private Vector2 lastPosition;

        public override void ResetEffects()
        {
            hasHallowedMirror = false;
        }

        public override void PostUpdate()
        {
            if (cooldownTimer > 0)
                cooldownTimer--;

            if (hasHallowedMirror && cooldownTimer <= 0)
            {
                if ((Player.position - lastPosition).Length() > 500f) // Significant distance = teleport
                {
                    cooldownTimer = 1200; // 20 seconds
                    Player.immune = true;
                    Player.immuneTime = 60; // 1 second of invincibility
                    Player.maxRunSpeed *= 1.5f;
                    Player.runAcceleration *= 1.5f;
                    Player.wingTime = Player.wingTimeMax;
                    CombatText.NewText(Player.Hitbox, Color.LightGoldenrodYellow, "Hallowed Flash!");
                }

                lastPosition = Player.position;
            }
        }
    }
}
