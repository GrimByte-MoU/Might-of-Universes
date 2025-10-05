using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class NecromasterEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName and Tooltip are set in localization files
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
            // Increase Reaper and Summon damage
            player.GetDamage(DamageClass.Summon) += 0.12f;
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.reaperDamageMultiplier += 1.12f;
            reaperPlayer.reaperCritChance += 10;

            // Apply the Necromaster effects
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
            if (!hasNecromasterEmblem) return;

            // Count active minions
            int minionCount = Player.numMinions;
            
            if (minionCount > 0)
            {
                // Minions provide life regeneration
                Player.lifeRegen += minionCount * 2;
            }
            else
            {
                // No minions - bonus for unused minion slots
                int unusedMinionSlots = Player.maxMinions;
                
                // Each unused minion slot gives +1 soul energy gathered and +2 defense
                Player.statDefense += unusedMinionSlots * 2;
                
                // We'll handle the soul energy gathering bonus in OnHitNPC
                // Just for visual effect, create some dust
                if (Main.rand.NextBool(20))
                {
                    for (int i = 0; i < unusedMinionSlots; i++)
                    {
                        Dust.NewDust(
                            Player.position,
                            Player.width,
                            Player.height,
                            DustID.StarRoyale,
                            Main.rand.NextFloat(-1f, 1f),
                            Main.rand.NextFloat(-1f, 1f),
                            0,
                            default,
                            Main.rand.NextFloat(0.8f, 1.2f)
                        );
                    }
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasNecromasterEmblem) return;
            
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            
            // Check if the projectile is a minion
            if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
            {
                // Minions gather 1 soul energy per strike
                reaperPlayer.AddSoulEnergy(1, target.Center);
                
                // Visual effect
                Dust.NewDust(
                    target.position,
                    target.width,
                    target.height,
                    DustID.StarRoyale,
                    0f,
                    0f,
                    0,
                    default,
                    1f
                );
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!hasNecromasterEmblem) return;
            
            // If no minions, apply bonus soul energy gathering
            if (Player.numMinions == 0)
            {
                int unusedMinionSlots = Player.maxMinions;
                
                // Each unused minion slot gives +1 soul energy gathered
                // We'll store this in a field to use in OnHitNPC
                if (proj.owner == Main.myPlayer && proj.DamageType == DamageClass.Generic)
                {
                    var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                    reaperPlayer.AddSoulEnergy(unusedMinionSlots, target.Center);
                    
                    // Visual effect
                    for (int i = 0; i < unusedMinionSlots; i++)
                    {
                        Dust.NewDust(
                            target.position,
                            target.width,
                            target.height,
                            DustID.StarRoyale,
                            Main.rand.NextFloat(-1f, 1f),
                            Main.rand.NextFloat(-1f, 1f),
                            0,
                            default,
                            Main.rand.NextFloat(0.8f, 1.2f)
                        );
                    }
                }
            }
        }
    }
}
