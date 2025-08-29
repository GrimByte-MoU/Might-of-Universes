using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class OminousAmphorae : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<OminousAmphoraePlayer>().hasOminousAmphorae = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 5)
                .AddIngredient(ItemID.ClayPot)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class OminousAmphoraePlayer : ModPlayer
    {
        public bool hasOminousAmphorae;
        private int cooldownTimer;

        public override void ResetEffects()
        {
            hasOminousAmphorae = false;
        }

        public override void PostUpdate()
        {
            if (cooldownTimer > 0)
                cooldownTimer--;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (hasOminousAmphorae && cooldownTimer <= 0)
            {
                int damageThreshold = (int)(Player.statLifeMax2 * 0.2f);
                if (info.Damage > damageThreshold)
                {
                    Player.statLife += info.Damage;
                    int debuffDuration = (int)((info.Damage * 2f / 10f) * 60);
                    Player.AddBuff(ModContent.BuffType<OminousPrice>(), debuffDuration);
                    cooldownTimer = 600;
                }
            }
        }
    }
}
