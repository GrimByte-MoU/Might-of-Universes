using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;
using System;

namespace MightofUniverses.Common.Players
{
    public class TerraiumArmorPlayer : ModPlayer
    {
        public bool knightSetBonus;
        public bool rangerSetBonus;
        public bool wizardSetBonus;
        public bool summonerSetBonus;
        public bool reaperSetBonus;

        private int rangerAttackTimer = 0;
        private int lastUsedItemType = 0;
        private int glaiveSpawnTimer = 0;
        private int worldsoulSpawnTimer = 0;
        private bool lastConsumedSouls = false;

        public override void ResetEffects()
        {
            knightSetBonus = false;
            rangerSetBonus = false;
            wizardSetBonus = false;
            summonerSetBonus = false;
            reaperSetBonus = false;
        }

        public override void PostUpdate()
        {
            bool hasAnySetBonus = knightSetBonus || rangerSetBonus || wizardSetBonus || summonerSetBonus || reaperSetBonus;

            if (hasAnySetBonus)
            {
                ApplyTerraiumVisuals();
            }

            if (knightSetBonus)
            {
                UpdateKnightSetBonus();
            }

            if (rangerSetBonus)
            {
                UpdateRangerSetBonus();
            }

            if (wizardSetBonus)
            {
                UpdateWizardSetBonus();
            }

            if (summonerSetBonus)
            {
                UpdateSummonerSetBonus();
            }

            if (reaperSetBonus)
            {
                UpdateReaperSetBonus();
            }
        }

       private void ApplyTerraiumVisuals()
{
    Player.armorEffectDrawShadow = true;
    Player. armorEffectDrawOutlines = true;
    Player.armorEffectDrawShadowLokis = true;
    Lighting.AddLight(Player.Center, 0.5f, 0.9f, 0.3f);
}

        private void UpdateKnightSetBonus()
        {
            int defenseBonus = Player.statDefense;
            int stacks = defenseBonus / 5;

            Player.GetDamage(DamageClass. Melee) += stacks * 0.01f;
            Player.GetAttackSpeed(DamageClass. Melee) += stacks * 0.01f;
            Player.endurance += stacks * 0.005f;

            glaiveSpawnTimer++;
            if (glaiveSpawnTimer >= 10)
            {
                glaiveSpawnTimer = 0;
                SpawnOrbitingGlaives();
            }
        }

        private void SpawnOrbitingGlaives()
        {
            if (Main.myPlayer != Player.whoAmI)
                return;

            int existingGlaives = 0;
            for (int i = 0; i < Main. maxProjectiles; i++)
            {
                if (Main.projectile[i]. active && 
                    Main.projectile[i].owner == Player.whoAmI && 
                    Main.projectile[i].type == ModContent. ProjectileType<TerraiumKnightGlaive>())
                {
                    existingGlaives++;
                }
            }

            while (existingGlaives < 3)
            {
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<TerraiumKnightGlaive>(),
                    150,
                    4f,
                    Player.whoAmI,
                    existingGlaives
                );
                existingGlaives++;
            }
        }

        private void UpdateRangerSetBonus()
{
    // Check if player is holding a ranged weapon and actively using it
    if (Player.HeldItem. DamageType == DamageClass.Ranged && 
        Player.HeldItem. useTime > 0 && 
        ! Player.noItems && 
        ! Player.CCed)
    {
        if (Player.HeldItem.type == lastUsedItemType)
        {
            if (Player.itemAnimation > 0 || Player.controlUseItem)
            {
                rangerAttackTimer++;
            }
        }
        else
        {
            rangerAttackTimer = 0;
            lastUsedItemType = Player. HeldItem.type;
        }
        int maxTimer = 300;
        rangerAttackTimer = Math.Min(rangerAttackTimer, maxTimer);
        float bonusAttackSpeed = rangerAttackTimer / (float)maxTimer * 0.50f;
        Player.GetAttackSpeed(DamageClass.Ranged) += bonusAttackSpeed;
        if (rangerAttackTimer > 150 && Main.rand.NextBool(8))
        {
            Dust speedDust = Dust.NewDustDirect(
                Player.position,
                Player.width,
                Player.height,
                DustID.TerraBlade,
                Player.velocity. X * 0.5f,
                Player. velocity.Y * 0.5f,
                100,
                Color. Lerp(Color.Orange, Color.Red, (rangerAttackTimer - 150) / 150f),
                1.5f + (rangerAttackTimer / 300f)
            );
            speedDust.noGravity = true;
        }
    }
    else
    {
        if (rangerAttackTimer > 0)
        {
            rangerAttackTimer -= 3;
            if (rangerAttackTimer < 0)
                rangerAttackTimer = 0;
        }
        lastUsedItemType = 0;
    }
}

        private void UpdateWizardSetBonus()
        {
            int manaStacks = Player.statManaMax2 / 10;
            float bonusDamage = manaStacks * 0.01f;
            Player.GetDamage(DamageClass.Magic) += bonusDamage;
        }

        private void UpdateSummonerSetBonus()
        {
            worldsoulSpawnTimer++;
            if (worldsoulSpawnTimer >= 30)
            {
                worldsoulSpawnTimer = 0;
                SpawnMiniatureWorldsoul();
            }
        }

        private void SpawnMiniatureWorldsoul()
        {
            if (Main.myPlayer != Player.whoAmI)
                return;

            bool hasWorldsoul = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && 
                    Main.projectile[i].owner == Player.whoAmI && 
                    Main.projectile[i].type == ModContent.ProjectileType<MiniatureWorldsoul>())
                {
                    hasWorldsoul = true;
                    break;
                }
            }

            if (!hasWorldsoul)
            {
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<MiniatureWorldsoul>(),
                    150,
                    4f,
                    Player.whoAmI
                );
            }
        }

        private void UpdateReaperSetBonus()
        {
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();

            if (reaperPlayer. justConsumedSouls && ! lastConsumedSouls)
            {
                FireTerraMissiles();
                reaperPlayer.AddDeathMarks(1);
            }

            lastConsumedSouls = reaperPlayer.justConsumedSouls;
        }

        private void FireTerraMissiles()
        {
            if (Main.myPlayer != Player.whoAmI)
                return;

            for (int i = 0; i < 3; i++)
            {
                Vector2 spawnPos = Player.Center + new Vector2(0, -50 - (i * 20));
                Vector2 velocity = new Vector2(0, -10);

                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    spawnPos,
                    velocity,
                    ModContent.ProjectileType<TerraMissile>(),
                    300,
                    5f,
                    Player.whoAmI
                );
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID. Item92, Player.Center);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyTerraRend(item. DamageType, target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC. HitInfo hit, int damageDone)
        {
            ApplyTerraRend(proj.DamageType, target);
        }

        private void ApplyTerraRend(DamageClass damageClass, NPC target)
        {
            if (knightSetBonus && damageClass == DamageClass. Melee)
            {
                target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            }

            if (rangerSetBonus && damageClass == DamageClass.Ranged)
            {
                target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            }

            if (wizardSetBonus && damageClass == DamageClass.Magic)
            {
                target. AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            }

            if (summonerSetBonus && (damageClass == DamageClass.Summon || damageClass == DamageClass.SummonMeleeSpeed))
            {
                target. AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            }

            if (reaperSetBonus && damageClass == ModContent.GetInstance<ReaperDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (wizardSetBonus && proj.DamageType == DamageClass.Magic)
            {
                Item heldItem = Player.HeldItem;
                if (heldItem != null && heldItem.useTime > 0)
                {
                    int useTime = heldItem.useTime;
                    int armorPen = 100 - (useTime * 2);
                    
                    armorPen = Math.Max(0, armorPen);
                    
                    modifiers.FlatBonusDamage += armorPen;
                }
            }
        }
    }
}