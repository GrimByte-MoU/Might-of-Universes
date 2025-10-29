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

        public override void ResetEffects()
        {
            meleeSizeMultiplier = 1f;
            rangedVelocityMultiplier = 1f;
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

            // Maintain Conjurer sentinel if conjurer set is equipped
            if (IsWearingConjurerSet())
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

            if (IsWearingWizardSet())
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
                armorAbilityCooldown = 300;
                armorAbilityTimer = 0;
                armorAbilityType = 1;
                Main.NewText("Prismatic Wizard Armor Ability used: restored 200 mana.", Color.MediumPurple);
                // Sync in multiplayer
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // placeholder - add packet if you need sync
                }
                return;
            }

            if (IsWearingKnightSet())
            {
                armorAbilityType = 2;
                armorAbilityTimer = 300;
                armorAbilityCooldown = 600;
                SoundEngine.PlaySound(SoundID.Item11, Player.position);
                Main.NewText("Prismatic Knight Armor Ability active.", Color.OrangeRed);
                return;
            }

            if (IsWearingCommandoSet())
            {
                armorAbilityType = 3;
                armorAbilityTimer = 180;
                armorAbilityCooldown = 600;
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
                    Player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
                    meleeSizeMultiplier = 1.25f;
                }
                else if (armorAbilityType == 3)
                {
                    Player.GetAttackSpeed(DamageClass.Ranged) += 2.0f;
                    rangedVelocityMultiplier = 2.0f;
                }
            }
        }

        private bool IsWearingWizardSet()
        {
            return Player.armor[0].type == ModContent.ItemType<PrismaticWizardHood>()
                && Player.armor[1].type == ModContent.ItemType<PrismaticChestplate>()
                && Player.armor[2].type == ModContent.ItemType<PrismaticGreaves>();
        }

        private bool IsWearingKnightSet()
        {
            return Player.armor[0].type == ModContent.ItemType<PrismaticKnightHelmet>()
                && Player.armor[1].type == ModContent.ItemType<PrismaticChestplate>()
                && Player.armor[2].type == ModContent.ItemType<PrismaticGreaves>();
        }

        private bool IsWearingCommandoSet()
        {
            return Player.armor[0].type == ModContent.ItemType<PrismaticCommandoHat>()
                && Player.armor[1].type == ModContent.ItemType<PrismaticChestplate>()
                && Player.armor[2].type == ModContent.ItemType<PrismaticGreaves>();
        }

        private bool IsWearingConjurerSet()
        {
            return Player.armor[0].type == ModContent.ItemType<PrismaticConjurerHood>()
                && Player.armor[1].type == ModContent.ItemType<PrismaticChestplate>()
                && Player.armor[2].type == ModContent.ItemType<PrismaticGreaves>();
        }

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