using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Accessories
{
    public class DeathknightEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.12f;
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            player.GetDamage<ReaperDamageClass>() += 0.12f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.GetCritChance<ReaperDamageClass>() += 10f;
            player.GetModPlayer<DeathknightEmblemPlayer>().hasDeathknightEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 15)
                .AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1)
                .AddIngredient(ItemID.DestroyerEmblem, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class DeathknightEmblemPlayer : ModPlayer
    {
        public bool hasDeathknightEmblem = false;
        private int soulGainCooldown = 0;
        private int soulDrainTimer = 0;

        public override void ResetEffects()
        {
            hasDeathknightEmblem = false;
        }

        public override void PostUpdate()
        {
            if (!hasDeathknightEmblem) return;

            if (soulGainCooldown > 0)
                soulGainCooldown--;

            bool holdingMelee = Player.HeldItem.CountsAsClass(DamageClass.Melee);
            if (holdingMelee)
            {
                soulDrainTimer++;
                if (soulDrainTimer >= 60)
                {
                    soulDrainTimer = 0;
                    var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                    reaperPlayer.soulEnergy = System.Math.Max(0, reaperPlayer.soulEnergy - 10);
                }
                Player.endurance += 0.1f;
                Player.lifeRegen += 3;
            }
            else
            {
                soulDrainTimer = 0;
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (!hasDeathknightEmblem || soulGainCooldown > 0) return;

            int soulEnergyGain = info.Damage / 2;
            
            if (soulEnergyGain > 0)
            {
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                reaperPlayer.AddSoulEnergy(soulEnergyGain, Player.Center);
                soulGainCooldown = 180;

                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(
                        Player.position,
                        Player.width,
                        Player.height,
                        DustID.Blood,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        0,
                        default,
                        Main.rand.NextFloat(1f, 1.5f)
                    );
                }
                
                if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "+" + soulEnergyGain + " Soul Energy");
                }
            }
        }
    }
}
