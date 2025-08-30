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

    // 20s cooldown (in ticks), 5s flash window (in ticks)
    private int cooldownTimer = 0;
    private int flashTimer = 0;

    // Store base values so we can revert cleanly
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
        // Ensure we revert if the player dies mid-boost
        if (boostApplied)
        {
            Player.maxRunSpeed = savedMaxRunSpeed;
            Player.runAcceleration = savedRunAcceleration;
            boostApplied = false;
        }
        flashTimer = 0;
        // Optional: keep cooldown or clear it on death
        // cooldownTimer = 0;
    }

    public override void PostUpdate()
    {
        if (cooldownTimer > 0)
            cooldownTimer--;
        if (flashTimer > 0)
            flashTimer--;

        // End-of-flash cleanup: revert speeds once when timer hits zero
        if (flashTimer == 0 && boostApplied)
        {
            Player.maxRunSpeed = savedMaxRunSpeed;
            Player.runAcceleration = savedRunAcceleration;
            boostApplied = false;
        }

        if (hasHallowedMirror && cooldownTimer <= 0)
        {
            // Detect large instantaneous movement (teleport) since last tick
            if (Vector2.Distance(Player.position, lastPosition) > 500f)
            {
                cooldownTimer = 1200; // 20 seconds
                flashTimer = 300;     // 5 seconds

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

                CombatText.NewText(Player.Hitbox, Color.LightGoldenrodYellow, "Hallowed Flash!");
            }
        }

        // Always update lastPosition each tick so teleport detection is accurate
        lastPosition = Player.position;
    }
}
}
