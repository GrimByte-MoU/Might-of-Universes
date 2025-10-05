using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common; // ReaperDamageClass
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class PrimalSavageryPlayer : ModPlayer
    {
        public bool FullSetEquipped;
        public bool MaskEquipped;
        public bool ChestEquipped;
        public bool BootsEquipped;
        public bool ChestSoulGenActive;

        private int _tarredBuffType = -2; // -2 = unknown/unresolved, -1 = not found, >=0 = valid buff type

        public override void ResetEffects()
        {
            FullSetEquipped = false;
            MaskEquipped = false;
            ChestEquipped = false;
            BootsEquipped = false;
            ChestSoulGenActive = false;
        }

        public override void PostUpdate()
        {
            // +4 souls/sec from chest
            if (ChestSoulGenActive)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(4f / 60f);
            }

            // Grant Savage (+30 Reaper AP, 5s) when souls are consumed
            if (FullSetEquipped && Player.GetModPlayer<ReaperPlayer>().justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<Savage>(), 60 * 5);
            }
        }

        public override void PostUpdateEquips()
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();

            // Set bonus main stats
            if (FullSetEquipped)
            {
                Player.GetDamage(reaperClass) += 0.15f;
                Player.GetCritChance(reaperClass) += 10f;
            }

            // Savage buff effect while active: +30 Reaper armor penetration
            if (Player.HasBuff(ModContent.BuffType<Savage>()))
            {
                Player.GetArmorPenetration(reaperClass) += 30;
            }
        }

        // Apply Tarred + Oiled on Reaper hits (3s)
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (FullSetEquipped && item.DamageType == reaperClass)
                ApplyTarOil(target);

            // Chest effect: crits ignite Oiled enemies (Hellfire 3s)
            if (ChestEquipped && hit.Crit && target.HasBuff(BuffID.Oiled))
                target.AddBuff(BuffID.OnFire3, 180);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (FullSetEquipped && proj.DamageType == reaperClass)
                ApplyTarOil(target);

            if (ChestEquipped && hit.Crit && target.HasBuff(BuffID.Oiled))
                target.AddBuff(BuffID.OnFire3, 180);
        }

        private void ApplyTarOil(NPC target)
        {
            int tarredType = GetTarredBuffType();
            if (tarredType != -1)
                target.AddBuff(tarredType, 180); // 3s Tarred (if your buff exists)
            target.AddBuff(BuffID.Oiled, 180);     // 3s Oiled
        }

        // Conditional +15 AP vs Oiled/Tarred from each piece
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (item.DamageType != reaperClass) return;

            int pieces = (MaskEquipped ? 1 : 0) + (ChestEquipped ? 1 : 0) + (BootsEquipped ? 1 : 0);
            if (pieces <= 0) return;

            bool hasTarred = false;
            int tarredType = GetTarredBuffType();
            if (tarredType != -1 && target.HasBuff(tarredType))
                hasTarred = true;

            if (target.HasBuff(BuffID.Oiled) || hasTarred)
                modifiers.ArmorPenetration += 15 * pieces;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (proj.DamageType != reaperClass) return;

            int pieces = (MaskEquipped ? 1 : 0) + (ChestEquipped ? 1 : 0) + (BootsEquipped ? 1 : 0);
            if (pieces <= 0) return;

            bool hasTarred = false;
            int tarredType = GetTarredBuffType();
            if (tarredType != -1 && target.HasBuff(tarredType))
                hasTarred = true;

            if (target.HasBuff(BuffID.Oiled) || hasTarred)
                modifiers.ArmorPenetration += 15 * pieces;
        }

        private int GetTarredBuffType()
        {
            // Cached
            if (_tarredBuffType != -2)
                return _tarredBuffType;

            // First try "MightofUniverses/Tarred"
            if (ModContent.TryFind<ModBuff>("MightofUniverses/Tarred", out var tarred))
            {
                _tarredBuffType = tarred.Type;
                return _tarredBuffType;
            }

            // Fallback: try local mod with just "Tarred"
            if (ModContent.TryFind<ModBuff>("Tarred", out tarred))
            {
                _tarredBuffType = tarred.Type;
                return _tarredBuffType;
            }

            // Not found
            _tarredBuffType = -1;
            return _tarredBuffType;
        }
    }
}