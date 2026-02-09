using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Materials;



namespace MightofUniverses.Content.Items.Accessories
{
    public class ColdCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ColdCrownPlayer>().hasColdCrown = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 7)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.IceBlock, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class ColdCrownPlayer : ModPlayer
    {
        public bool hasColdCrown = false;
        private const float COLD_CROWN_RADIUS = 15f * 16f;
        private readonly Color coldAuraColor = new Color(150, 200, 255, 100);
        private readonly HashSet<int> slowedNPCs = new HashSet<int>();

        public override void ResetEffects()
        {
            hasColdCrown = false;
        }

        public override void PostUpdate()
        {
            if (!hasColdCrown) 
            {
                slowedNPCs.Clear();
                return;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                
                if (!npc.active || npc.friendly || npc.boss || npc.townNPC)
                    continue;
                
                float distance = Vector2.Distance(Player.Center, npc.Center);
                
                if (distance <= COLD_CROWN_RADIUS)
                {
                    npc.velocity *= 0.5f;
                    slowedNPCs.Add(i);

                    if (Main.rand.NextBool(15))
                    {
                        Dust.NewDust(
                            npc.position,
                            npc.width,
                            npc.height,
                            DustID.IceTorch,
                            0f,
                            0f,
                            0,
                            default,
                            Main.rand.NextFloat(0.8f, 1.2f)
                        );
                    }
                }
                else
                {
                    slowedNPCs.Remove(i);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasColdCrown && Vector2.Distance(Player.Center, target.Center) <= COLD_CROWN_RADIUS)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(
                        target.position,
                        target.width,
                        target.height,
                        DustID.IceTorch,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        0,
                        default,
                        Main.rand.NextFloat(1f, 1.5f)
                    );
                }
            }
        }

        public void OnNPCKilled(Player player, NPC target, int damage, double knockback, bool crit)
        {
            if (hasColdCrown && 
                Vector2.Distance(Player.Center, target.Center) <= COLD_CROWN_RADIUS && 
                !target.SpawnedFromStatue)
            {
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                reaperPlayer.AddSoulEnergy(10, target.Center);

                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDust(
                        target.position,
                        target.width,
                        target.height,
                        DustID.IceTorch,
                        Main.rand.NextFloat(-3f, 3f),
                        Main.rand.NextFloat(-3f, 3f),
                        0,
                        default,
                        Main.rand.NextFloat(1.2f, 2f)
                    );
                }

                if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(target.getRect(), coldAuraColor, "+10 Soul Energy");
                }
            }
        }
    }
}
