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
            // Reset booleans and multipliers each tick
            prismaticWizardSet = false;
            prismaticKnightSet = false;
            prismaticCommandoSet = false;
            prismaticConjurerSet = false;

            meleeSizeMultiplier = 1f;
            rangedVelocityMultiplier = 1f;
            // ammoConserveChance is additive from gear and not reset here so UpdateEquip may add to it directly.
            ammoConserveChance = 0f;
        }

        public override void PostUpdate()
        {
            // Cooldown decrement
            if (armorAbilityCooldown > 0)
                armorAbilityCooldown--;

            // Timer decrement
            if (armorAbilityTimer > 0)
            {
                armorAbilityTimer--;
                if (armorAbilityTimer == 0)
                {
                    armorAbilityType = 0;
                }
            }

            // Maintain Conjurer sentinel if conjurer set is active
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
        }

        private void TryActivateArmorAbility()
        {
            if (armorAbilityCooldown > 0) return;

            // Use the boolean flags set by UpdateArmorSet; this is more reliable than inspecting Player.armor[] repeatedly.
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
                // TODO: sync packet if multiplayer behavior needs announcement
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

            // Conjurer set is passive (sentinel) — no armor key action here
        }

        public override void UpdateDead()
        {
            armorAbilityTimer = 0;
            armorAbilityType = 0;
            armorAbilityCooldown = 0;
        }

        public override void UpdateEquips()
        {
            // Apply active ability effects while armorAbilityTimer > 0
            if (armorAbilityTimer > 0)
            {
                if (armorAbilityType == 2)
                {
                    Player.GetCritChance(DamageClass.Melee) += 10;
                    Player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
                    meleeSizeMultiplier = 1.25f;
                }
                else if (armorAbilityType == 3)
                {
                    // Add attack speed to approximate tripled firing speed while active
                    Player.GetAttackSpeed(DamageClass.Ranged) += 2.0f;
                    rangedVelocityMultiplier = 2.0f;
                }
            }

            // ammoConserveChance should be accumulated from UpdateEquip of head if Commando hat is equipped
            // (UpdateEquip on commando hat must set player.GetModPlayer<PrismaticPlayer>().ammoConserveChance += 0.25f;)
        }

        // Optional helper checks kept for compatibility; they now simply read the flags
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