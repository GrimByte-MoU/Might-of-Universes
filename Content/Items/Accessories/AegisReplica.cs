// Content/Items/Accessories/AegisReplica. cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content. Items.Buffs;
using System;

namespace MightofUniverses. Content.Items.Accessories
{
    public class AegisReplica : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.accessory = true;
            Item. rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<AegisReplicaPlayer>();
            modPlayer.hasAegisReplica = true;
            modPlayer.hideVisual = hideVisual;
        }

    }

    public class AegisReplicaPlayer : ModPlayer
    {
        public bool hasAegisReplica;
        public bool hideVisual;
        
        public float shieldHealth;
        public float maxShieldHealth = 1500f;
        public int shieldRegenTimer;
        public const int SHIELD_REGEN_TIME = 1800;

        public override void ResetEffects()
        {
            hasAegisReplica = false;
            hideVisual = false;
        }

        public override void PostUpdateEquips()
        {
            if (! hasAegisReplica) return;

            Player. statDefense += 15;

            Player.buffImmune[ModContent.BuffType<TerrasRend>()] = true;
            Player. buffImmune[ModContent. BuffType<ElementsHarmony>()] = true;
            Player.buffImmune[ModContent.BuffType<PrismaticRend>()] = true;
            Player.buffImmune[ModContent.BuffType<HellsMark>()] = true;
            Player.buffImmune[ModContent.BuffType<Drowning>()] = true;
            Player.buffImmune[ModContent.BuffType<DeadlyCorrupt>()] = true;
            Player.buffImmune[ModContent.BuffType<Subjugated>()] = true;
            Player.buffImmune[ModContent.BuffType<RebukingLight>()] = true;
            Player.buffImmune[ModContent.BuffType<LunarReap>()] = true;

            if (shieldHealth > 0)
            {
                Player.noKnockback = true;
            }

            if (shieldHealth <= 0)
            {
                shieldRegenTimer++;
                
                if (shieldRegenTimer >= SHIELD_REGEN_TIME)
                {
                    shieldHealth = maxShieldHealth;
                    shieldRegenTimer = 0;
                    
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                        int dust = Dust.NewDust(Player. Center, 0, 0, DustID.GoldCoin,
                            velocity.X, velocity.Y, 100, Color.Gold, 1.5f);
                        Main.dust[dust].noGravity = true;
                    }
                    
                    CombatText. NewText(Player.getRect(), Color.Gold, "Shield Restored!", true);
                }
            }

            if (shieldHealth > 0 && Main.rand.NextBool(3) && ! hideVisual)
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * 64f,
                    (float)Math.Sin(angle) * 64f
                );
                
                int dust = Dust.NewDust(Player.Center + offset, 0, 0, DustID.GoldCoin,
                    0f, 0f, 150, Color.Gold, 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Vector2.Zero;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (! hasAegisReplica) return;

            if (shieldHealth > 0)
            {
                modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
                {
                    int incomingDamage = info. Damage;
                    
                    if (shieldHealth >= incomingDamage)
                    {
                        shieldHealth -= incomingDamage;
                        info.Damage = 0;
                        
                        CombatText.NewText(Player.getRect(), Color.Gold, 
                            $"Shield:  {(int)shieldHealth}/{(int)maxShieldHealth}", false);
                    }
                    else
                    {
                        int remainingDamage = incomingDamage - (int)shieldHealth;
                        shieldHealth = 0;
                        info.Damage = remainingDamage;
                        
                        CombatText.NewText(Player.getRect(), Color.Red, "Shield Broken!", true);
                        
                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                            int dust = Dust.NewDust(Player.Center, Player.width, Player.height, 
                                DustID.GoldCoin, velocity.X, velocity. Y, 100, Color.Orange, 1.3f);
                            Main.dust[dust].noGravity = true;
                        }

                        shieldRegenTimer = 0;
                    }
                };
            }
        }

        public override void Initialize()
        {
            shieldHealth = maxShieldHealth;
            shieldRegenTimer = SHIELD_REGEN_TIME;
        }
    }
}