using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Accessories
{
    public class QueensCrystalTear : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<QueensTearPlayer>().hasQueensTear = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 5)
                .AddIngredient(ItemID.WaterBucket)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class QueensTearPlayer : ModPlayer
    {
        public bool hasQueensTear;
        public bool dodgeNextHit;
        private int tearCooldown;

        public override void ResetEffects()
        {
            hasQueensTear = false;
        }

        public override void PostUpdate()
        {
            if (tearCooldown > 0)
                tearCooldown--;
        }

        public void OnSoulEnergyUsed()
        {
            if (hasQueensTear && tearCooldown <= 0)
            {
                dodgeNextHit = true;
                tearCooldown = 180;
                CombatText.NewText(Player.Hitbox, Color.Aqua, "Next hit dodged!");
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (dodgeNextHit)
            {
                modifiers.SetMaxDamage(0);
                dodgeNextHit = false;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (dodgeNextHit)
            {
                modifiers.SetMaxDamage(0);
                dodgeNextHit = false;
            }
        }

        public override void PostUpdateEquips()
        {
            if (hasQueensTear)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.reaperDamageMultiplier += 0.12f;
                reaper.reaperCritChance += 7;
            }
        }
    }
}
