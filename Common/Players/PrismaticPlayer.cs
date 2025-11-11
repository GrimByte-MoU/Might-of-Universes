using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using System.Linq;
using MightofUniverses.Common.Input;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Armors;

namespace MightofUniverses.Common.Players
{
    public class PrismaticPlayer : ModPlayer
    {
        public int armorAbilityCooldown = 0;
        public int armorAbilityTimer = 0;
        public int armorAbilityType = 0; // 0 = none, 1 = Wizard, 2 = Knight, 3 = Commando
        public float meleeSizeMultiplier = 1f;
        public float rangedVelocityMultiplier = 1f;
        public float ammoConserveChance = 0f;

        // Sentinel tracking
        private int sentinelProjectileId = -1;

        // Flags set by head.UpdateArmorSet when a full prismatic set is equipped.
        public bool prismaticWizardSet = false;
        public bool prismaticKnightSet = false;
        public bool prismaticCommandoSet = false;
        public bool prismaticConjurerSet = false;

        public override void ResetEffects()
        {
            prismaticWizardSet = false;
            prismaticKnightSet = false;
            prismaticCommandoSet = false;
            prismaticConjurerSet = false;

            meleeSizeMultiplier = 1f;
            rangedVelocityMultiplier = 1f;
            ammoConserveChance = 0f;
        }

        public override void PostUpdate()
        {
            if (armorAbilityCooldown > 0)
                armorAbilityCooldown--;
            if (armorAbilityTimer > 0)
            {
                armorAbilityTimer--;
                if (armorAbilityTimer == 0)
                {
                    armorAbilityType = 0;
                }
            }
            if (prismaticConjurerSet)
            {
                EnsureSentinelExists();
            }
            else
            {
                if (sentinelProjectileId != -1)
                {
                    if (Main.projectile.IndexInRange(sentinelProjectileId))
                        Main.projectile[sentinelProjectileId].Kill();
                    sentinelProjectileId = -1;
                }
            }

            if (!Main.dedServ && ModKeybindManager.ArmorAbility != null && ModKeybindManager.ArmorAbility.JustPressed)
            {
                TryActivateArmorAbility();
            }

            if (prismaticWizardSet || prismaticKnightSet || prismaticCommandoSet || prismaticConjurerSet)
            {
                Lighting.AddLight(Player.Center, Main.DiscoR / 255f * 2f, Main.DiscoG / 255f * 2f, Main.DiscoB / 255f * 2f);

                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.RainbowTorch, 0f, 0f, 100, default, 1f);
                    dust.noGravity = true;
                    dust.fadeIn = 0.3f;
                }
            }
        }

        private void TryActivateArmorAbility()
        {
            if (armorAbilityCooldown > 0) return;
            if (prismaticWizardSet)
            {
                int restore = 200;
                Player.statMana += restore;
                if (Player.statMana > Player.statManaMax2) Player.statMana = Player.statManaMax2;
                for (int i = Player.buffType.Length - 1; i >= 0; i--)
                {
                    if (Player.buffType[i] == BuffID.ManaSickness)
                        Player.DelBuff(i);
                }

                SoundEngine.PlaySound(SoundID.Item4, Player.position);
                armorAbilityCooldown = 300; // 5s
                armorAbilityTimer = 0;
                armorAbilityType = 1;
                Main.NewText("Prismatic Wizard Armor Ability used: restored 200 mana.", Color.MediumPurple);
                return;
            }

            if (prismaticKnightSet)
            {
                armorAbilityType = 2;
                armorAbilityTimer = 300; // 5s
                armorAbilityCooldown = 600; // 10s
                SoundEngine.PlaySound(SoundID.Item11, Player.position);
                Main.NewText("Prismatic Knight Armor Ability active.", Color.OrangeRed);
                return;
            }

            if (prismaticCommandoSet)
            {
                armorAbilityType = 3;
                armorAbilityTimer = 180; // 3s
                armorAbilityCooldown = 600; // 10s
                SoundEngine.PlaySound(SoundID.Item11, Player.position);
                Main.NewText("Prismatic Commando Armor Ability active.", Color.LightSkyBlue);
                return;
            }
        }

        public override void UpdateDead()
        {
            armorAbilityTimer = 0;
            armorAbilityType = 0;
            armorAbilityCooldown = 0;
        }

        public override void UpdateEquips()
        {
            if (armorAbilityTimer > 0)
            {
                if (armorAbilityType == 2)
                {
                    Player.GetCritChance(DamageClass.Melee) += 10;
                    Player.GetAttackSpeed(DamageClass.Melee) += 1f;
                }
                else if (armorAbilityType == 3)
                {
                    Player.GetAttackSpeed(DamageClass.Ranged) += 2.0f;
                    rangedVelocityMultiplier = 2.0f;
                }
            }
        }
        private bool IsWearingWizardSet() => prismaticWizardSet;
        private bool IsWearingKnightSet() => prismaticKnightSet;
        private bool IsWearingCommandoSet() => prismaticCommandoSet;
        private bool IsWearingConjurerSet() => prismaticConjurerSet;

        private void EnsureSentinelExists()
        {
            if (sentinelProjectileId != -1 && Main.projectile.IndexInRange(sentinelProjectileId))
            {
                Projectile p = Main.projectile[sentinelProjectileId];
                if (p.active && p.owner == Player.whoAmI) return;
            }
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == Player.whoAmI && p.type == ModContent.ProjectileType<PrismaticSentinel>())
                {
                    sentinelProjectileId = i;
                    return;
                }
            }
            int proj = Projectile.NewProjectile(Player.GetSource_Misc("PrismaticSentinel"), Player.Center, Vector2.Zero, ModContent.ProjectileType<PrismaticSentinel>(), 0, 0f, Player.whoAmI);
            if (proj >= 0)
                sentinelProjectileId = proj;
        }
    }
}