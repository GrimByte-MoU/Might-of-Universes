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
    private int flashTimer = 0;
    private bool boostApplied = false;
    private float savedMaxRunSpeed = 0f;
    private float savedRunAcceleration = 0f;

    private Vector2 lastPosition;

    public override void ResetEffects()
    {
        hasHallowedMirror = false;
    }

    public override void UpdateDead()
    {
        if (boostApplied)
        {
            Player.maxRunSpeed = savedMaxRunSpeed;
            Player.runAcceleration = savedRunAcceleration;
            boostApplied = false;
        }
        flashTimer = 0;
    }

    public override void PostUpdate()
    {
        if (cooldownTimer > 0)
            cooldownTimer--;
        if (flashTimer > 0)
            flashTimer--;

        if (flashTimer == 0 && boostApplied)
        {
            Player.maxRunSpeed = savedMaxRunSpeed;
            Player.runAcceleration = savedRunAcceleration;
            boostApplied = false;
        }

        if (hasHallowedMirror && cooldownTimer <= 0)
        {
            if (Vector2.Distance(Player.position, lastPosition) > 500f)
            {
                cooldownTimer = 1200;
                flashTimer = 300;

                Player.immune = true;
                Player.immuneTime = 60;
                Player.wingTime = Player.wingTimeMax;
                if (!boostApplied)
                {
                    savedMaxRunSpeed = Player.maxRunSpeed;
                    savedRunAcceleration = Player.runAcceleration;

                    Player.maxRunSpeed = savedMaxRunSpeed * 1.5f;
                    Player.runAcceleration = savedRunAcceleration * 1.5f;

                    boostApplied = true;
                }
            }
        }
        lastPosition = Player.position;
    }
}
}
