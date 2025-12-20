using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class NecromasterEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID. Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass. Summon) += 0.12f;
            player.GetDamage<ReaperDamageClass>() += 0.12f;
            player.GetCritChance<ReaperDamageClass>() += 10f;
            player.GetModPlayer<NecromasterEmblemPlayer>().hasNecromasterEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentStardust, 15)
                .AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1)
                .AddIngredient(ItemID.DestroyerEmblem, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class NecromasterEmblemPlayer : ModPlayer
    {
        public bool hasNecromasterEmblem = false;

        public override void ResetEffects()
        {
            hasNecromasterEmblem = false;
        }

        public override void PostUpdate()
        {
            if (! hasNecromasterEmblem) return;

            int minionCount = Player.numMinions;

            if (minionCount > 0)
            {
                // +2 HP/sec regen per minion
                Player.lifeRegen += minionCount * 2;
            }
            else
            {
                int unusedMinionSlots = Player.maxMinions;
                
                // +2 defense per unused minion slot (unchanged)
                Player.statDefense += unusedMinionSlots * 2;
                
                // NEW: +2 armor penetration per unused minion slot - BUFFED
                Player.GetArmorPenetration(DamageClass.Generic) += unusedMinionSlots * 2;

                if (Main.rand.NextBool(20))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust. NewDust(Player.position, Player.width, Player.height, 
                            DustID. StarRoyale, Main.rand.NextFloat(-1f, 1f), 
                            Main.rand.NextFloat(-1f, 1f), 0, default, 0.8f);
                    }
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (! hasNecromasterEmblem) return;

            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();

            if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
            {
                reaperPlayer. AddSoulEnergy(1, target.Center);

                if (Main.rand.NextBool(5))
                {
                    Dust.NewDust(target. position, target.width, target.height, 
                        DustID.StarRoyale, 0f, 0f, 0, default, 1f);
                }
            }

            if (Player.numMinions == 0 && proj.owner == Player.whoAmI)
            {
                if (target.life - damageDone <= 0)
                {
                    int unusedMinionSlots = Player. maxMinions;
                    reaperPlayer.AddSoulEnergy(unusedMinionSlots, target.Center);

                    for (int i = 0; i < unusedMinionSlots; i++)
                    {
                        Dust.NewDust(target. position, target.width, target. height, 
                            DustID.StarRoyale, Main.rand.NextFloat(-2f, 2f), 
                            Main.rand.NextFloat(-2f, 2f), 0, default, 1.2f);
                    }
                }
            }
        }
    }
}