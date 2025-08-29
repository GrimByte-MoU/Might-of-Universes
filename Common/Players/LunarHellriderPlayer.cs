using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using Terraria.GameContent;
using System;

namespace MightofUniverses.Common.Players
{

public class LunarHellriderPlayer : ModPlayer
{
    public bool hasLunarHellrider;
    private float dodgeChance = 0.20f;
    private int timeSinceLastDodge = 0;

    public override void ResetEffects()
    {
        hasLunarHellrider = false;
    }

    public override void PostUpdate()
    {
        if (hasLunarHellrider && !Player.HasBuff(ModContent.BuffType<LunarVision>()) && 
            !Player.HasBuff(ModContent.BuffType<AlteredPerception>()))
        {
            timeSinceLastDodge++;
            dodgeChance = Math.Min(1f, 0.20f + (timeSinceLastDodge / 60f) * 0.05f);
        }
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (!hasLunarHellrider) return;

        if (Player.HasBuff(ModContent.BuffType<LunarVision>()) || 
            Player.HasBuff(ModContent.BuffType<AlteredPerception>()))
            return;

        if (Main.rand.NextFloat() < dodgeChance)
        {
            modifiers.FinalDamage *= 0;
            Player.AddBuff(ModContent.BuffType<LunarVision>(), 480);
            timeSinceLastDodge = 0;
            dodgeChance = 0.20f;

            // Spawn Lunar Blasts
            SpawnLunarBlasts(5);
            
            // Affect nearby enemies
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) <= 1200f)
                {
                    npc.AddBuff(ModContent.BuffType<LunarReap>(), 180);
                    npc.AddBuff(BuffID.Ichor, 180);
                    npc.AddBuff(ModContent.BuffType<Demonfire>(), 180);
                }
            }
        }
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (!hasLunarHellrider) return;

        if (info.DamageSource.SourceNPCIndex >= 0)
        {
            NPC attackingNPC = Main.npc[info.DamageSource.SourceNPCIndex];
            if (attackingNPC != null)
            {
                attackingNPC.StrikeNPC(new NPC.HitInfo { Damage = (int)(info.Damage * 3f) });
                SpawnLunarBlasts(2);
            }
        }
    }

    private void SpawnLunarBlasts(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition = Player.Center - new Vector2(Main.rand.Next(-200, 201), 400);
            int proj = Projectile.NewProjectile(
                Player.GetSource_FromThis(),
                spawnPosition,
                Vector2.Zero,
                ModContent.ProjectileType<LunarBlastProjectile>(),
                125,
                0f,
                Player.whoAmI
            );
        }
    
    }
     public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Player.HasBuff(ModContent.BuffType<LunarVision>()))
        {
            if (hasLunarHellrider)
                modifiers.CritDamage += 1f;
        }
    }
}
}
