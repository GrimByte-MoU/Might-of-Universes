using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses. Content.Items.Buffs
{
    public class HellsMark : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 80;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc. height, DustID. Torch, 0f, 0f, 100, Color.OrangeRed, 1.0f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                
                if (Main.rand.NextBool(5))
                {
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 20f;
                    Dust. NewDustPerfect(npc.Center + offset, DustID. Torch, Vector2.Zero, 100, Color.Red, 1.5f).noGravity = true;
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 40;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch, 0f, 0f, 100, Color.OrangeRed, 1.0f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                
                if (Main.rand.NextBool(5))
                {
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 20f;
                    Dust.NewDustPerfect(player.Center + offset, DustID.Torch, Vector2.Zero, 100, Color. Red, 1.5f).noGravity = true;
                }
            }
        }
    }

    public class HellsMarkGlobalNPC : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            CheckAndTriggerExplosion(npc, damageDone);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            CheckAndTriggerExplosion(npc, damageDone);
        }

        private void CheckAndTriggerExplosion(NPC target, int damageDone)
        {
            if (target. HasBuff(ModContent.BuffType<HellsMark>()))
            {
                TriggerHellsMarkExplosion(target, damageDone);
                
                int buffIndex = target.FindBuffIndex(ModContent.BuffType<HellsMark>());
                if (buffIndex >= 0)
                {
                    target.DelBuff(buffIndex);
                }
            }
        }

        private void TriggerHellsMarkExplosion(NPC target, int damageDone)
        {
            float explosionRadius = 160f;
            
            float damagePercent;
            int minimumDamage;
            
            if (NPC.downedMoonlord)
            {
                damagePercent = 0.5f;
                minimumDamage = 75;
            }
            else if (NPC.downedGolemBoss)
            {
                damagePercent = 0.40f;
                minimumDamage = 50;
            }
            else if (NPC.downedPlantBoss)
            {
                damagePercent = 0.35f;
                minimumDamage = 40;
            }
            else
            {
                damagePercent = 0.30f;
                minimumDamage = 30;
            }
            
            int calculatedDamage = (int)(damageDone * damagePercent);
            int explosionDamage = Math.Max(calculatedDamage, minimumDamage);
            
            int rings = 5;
            for (int ring = 0; ring < rings; ring++)
            {
                float currentRadius = (explosionRadius / rings) * (ring + 1);
                int segments = 20 + (ring * 10);
                
                for (int i = 0; i < segments; i++)
                {
                    float angle = (i / (float)segments) * MathHelper.TwoPi;
                    Vector2 position = target.Center + new Vector2(
                        (float)Math.Cos(angle) * currentRadius,
                        (float)Math.Sin(angle) * currentRadius
                    );
                    
                    int dustIndex = Dust.NewDust(position, 0, 0, DustID.Torch, 0, 0, 100, Color. OrangeRed, 2.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity = new Vector2(
                        (float)Math.Cos(angle) * 2f,
                        (float)Math.Sin(angle) * 2f
                    );
                    Main.dust[dustIndex].scale = 2.0f + (ring * 0.3f);
                }
            }
            
            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-8f, 8f),
                    Main.rand.NextFloat(-8f, 8f)
                );
                int dustIndex = Dust.NewDust(target.Center, 20, 20, DustID. Torch, velocity.X, velocity.Y, 0, Color.Red, 3.0f);
                Main. dust[dustIndex].noGravity = true;
            }
            
            Lighting.AddLight(target.Center, 2f, 1f, 0f);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, target.Center);
            
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player owner = Main.player[Main.myPlayer];
                
                int savedDefense = target.defense;
                target.defense = 0;
                target.SimpleStrikeNPC(explosionDamage, 0, false, 0, null, false, 0, true);
                target.defense = savedDefense;
                
                target.AddBuff(ModContent. BuffType<Demonfire>(), 120);
                
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC otherNPC = Main.npc[i];
                    if (otherNPC. active && i != target.whoAmI && !otherNPC.friendly && otherNPC.type != NPCID. TargetDummy)
                    {
                        float distance = Vector2.Distance(target.Center, otherNPC. Center);
                        
                        if (distance < explosionRadius)
                        {
                            int otherSavedDefense = otherNPC.defense;
                            otherNPC.defense = 0;
                            otherNPC. SimpleStrikeNPC(explosionDamage, 0, false, 0, null, false, 0, true);
                            otherNPC.defense = otherSavedDefense;
                            
                            otherNPC.AddBuff(ModContent.BuffType<Demonfire>(), 120);
                            
                            if (Main.rand.NextBool(3))
                            {
                                otherNPC.AddBuff(ModContent.BuffType<HellsMark>(), 180);
                                
                                for (int d = 0; d < 8; d++)
                                {
                                    Dust chainDust = Dust.NewDustDirect(otherNPC.position, otherNPC.width, otherNPC.height, 
                                        DustID. Torch, 0, 0, 100, Color. DarkRed, 2.0f);
                                    chainDust.noGravity = true;
                                }
                            }
                            
                            Vector2 direction = (otherNPC.Center - target.Center).SafeNormalize(Vector2.UnitX);
                            for (int d = 0; d < 10; d++)
                            {
                                Vector2 dustPos = target.Center + (direction * distance * (d / 10f));
                                Dust. NewDust(dustPos, 4, 4, DustID. Torch, 0, 0, 100, Color.Orange, 1.5f);
                            }
                        }
                    }
                }
            }
        }
    }
}