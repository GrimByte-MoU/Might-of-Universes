using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BloodsoakedRing : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<BloodsoakedPlayer>();
            modPlayer.hasBloodsoakedRing = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 5)
                .AddIngredient(ItemID.BandofRegeneration)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class BloodsoakedPlayer : ModPlayer
    {
        public bool hasBloodsoakedRing;
        private int bloodCooldown;

        public override void ResetEffects()
        {
            hasBloodsoakedRing = false;
        }

        public override void PostUpdate()
        {
            if (hasBloodsoakedRing)
            {
                Player.lifeRegen += 6;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 1.25f);
            }

            if (bloodCooldown > 0)
                bloodCooldown--;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (hasBloodsoakedRing)
            {
                modifiers.FinalDamage *= 1.2f;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (hasBloodsoakedRing)
            {
                modifiers.FinalDamage *= 1.2f;
            }
        }

        public override void OnHurt(Player.HurtInfo info)
{
    if (hasBloodsoakedRing && bloodCooldown <= 0)
    {
        Vector2 sourcePosition = Player.position;
        Player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(info.Damage * 0.4f, sourcePosition);
        bloodCooldown = 1200; // 20 seconds
    }
}
    }
}
