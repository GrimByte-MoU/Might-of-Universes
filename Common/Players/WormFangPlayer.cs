using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class WormFangPlayer : ModPlayer
    {
        public bool hasWormFang;
        public bool hasWormFamiliar;
        public bool hasHellwyrmFamiliar;
        public bool hasPrismaticFamiliar;
        private int retaliationCooldown = 0;

        public override void ResetEffects()
        {
            hasWormFang = false;
            hasWormFamiliar = false;
            hasHellwyrmFamiliar = false;
            hasPrismaticFamiliar = false;
        }

        public override void PostUpdate()
        {
            if (retaliationCooldown > 0)
                retaliationCooldown--;

            if (hasPrismaticFamiliar && Player.statLife <= Player.statLifeMax2 * 0.5f && retaliationCooldown <= 0)
            {
                retaliationCooldown = 1800;
                Vector2 spawnPosition = Player.Center - new Vector2(0, 200);
                
                for (int i = 0; i < 5; i++)
                {
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(12f, 15f));
                    int proj = Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        spawnPosition,
                        velocity,
                        ModContent.ProjectileType<PrismaticSpitballProjectile>(),
                        100,
                        0f,
                        Player.whoAmI
                    );
                }
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            NPC attackingNPC = null;
            if (info.DamageSource.SourceNPCIndex >= 0)
            {
                attackingNPC = Main.npc[info.DamageSource.SourceNPCIndex];
            }

            if (attackingNPC != null)
            {
                if (hasWormFang)
                {
                    attackingNPC.StrikeNPC(new NPC.HitInfo { Damage = info.Damage });
                    attackingNPC.AddBuff(ModContent.BuffType<Corrupted>(), 180);
                }
                else if (hasWormFamiliar)
                {
                    attackingNPC.StrikeNPC(new NPC.HitInfo { Damage = (int)(info.Damage * 1.5f) });
                    attackingNPC.AddBuff(BuffID.CursedInferno, 180);
                    attackingNPC.AddBuff(ModContent.BuffType<Corrupted>(), 180);
                }
                else if (hasHellwyrmFamiliar)
                {
                    attackingNPC.StrikeNPC(new NPC.HitInfo { Damage = (int)(info.Damage * 2f) });
                    attackingNPC.AddBuff(BuffID.CursedInferno, 180);
                    attackingNPC.AddBuff(ModContent.BuffType<Demonfire>(), 180);
                }
                else if (hasPrismaticFamiliar)
                {
                    attackingNPC.StrikeNPC(new NPC.HitInfo { Damage = (int)(info.Damage * 2.5f) });
                    attackingNPC.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
                    attackingNPC.AddBuff(ModContent.BuffType<Demonfire>(), 180);
                }
            }
        }
    }
}



