using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Accessories
{
    public class VolatileSand : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VolatileSandPlayer>().hasVolatileSand = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SandstorminaBottle, 1)
                .AddIngredient(ItemID.SandBlock, 25)
                .AddIngredient(ItemID.Bomb, 5)
                .AddIngredient(ItemID.Scorpion, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class VolatileSandPlayer : ModPlayer
    {
        public bool hasVolatileSand;
        private int sandTimer;

        public override void ResetEffects()
        {
            hasVolatileSand = false;
        }

        public override void PostUpdate()
        {
            if (!hasVolatileSand)
                return;

            sandTimer++;
            if (sandTimer >= 120)
            {
                sandTimer = 0;
                NPC target = null;
                float highestLife = 0f;
                float maxRange = 50 * 16;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.CanBeChasedBy(Player) && Vector2.Distance(Player.Center, npc.Center) <= maxRange)
                    {
                        if (npc.lifeMax > highestLife)
                        {
                            highestLife = npc.lifeMax;
                            target = npc;
                        }
                    }
                }

                if (target != null && Main.myPlayer == Player.whoAmI)
                {
                    Projectile.NewProjectile(
    Main.player[Main.myPlayer].GetSource_Accessory(Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem]),
    target.Center,
    Vector2.Zero,
    ModContent.ProjectileType<VolatileSandExplosion>(),
    20,
    0f,
    Main.player[Main.myPlayer].whoAmI,
    ai0: target.whoAmI
);
                }
            }
        }
    }
}
