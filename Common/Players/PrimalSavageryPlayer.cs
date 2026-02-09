using MightofUniverses.Content.Items.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Common.Players
{
    public class PrimalSavageryPlayer : ModPlayer
    {
        public bool FullSetEquipped;
        public bool MaskEquipped;
        public bool ChestEquipped;
        public bool BootsEquipped;
        public bool ChestSoulGenActive;

        private int _tarredBuffType = -2;

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
            if (ChestSoulGenActive)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(4f / 60f);
            }
            if (FullSetEquipped && Player.GetModPlayer<ReaperPlayer>().justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<Savage>(), 60 * 5);
            }

            if (!FullSetEquipped) return;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Bone, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(4))
            {
                int dustType = Main.rand.NextBool() ? DustID.AmberBolt : DustID.Shadowflame;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, Color.Orange, 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
            }
        }

        public override void PostUpdateEquips()
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (FullSetEquipped)
            {
                Player.GetDamage(reaperClass) += 0.15f;
                Player.GetCritChance(reaperClass) += 10f;
            }

            if (Player.HasBuff(ModContent.BuffType<Savage>()))
            {
                Player.GetArmorPenetration(reaperClass) += 30;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            if (FullSetEquipped && item.DamageType == reaperClass)
                ApplyTarOil(target);

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
                target.AddBuff(tarredType, 180);
            target.AddBuff(BuffID.Oiled, 180);
        }
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
            if (_tarredBuffType != -2)
                return _tarredBuffType;
            if (ModContent.TryFind<ModBuff>("MightofUniverses/Tarred", out var tarred))
            {
                _tarredBuffType = tarred.Type;
                return _tarredBuffType;
            }

            if (ModContent.TryFind("Tarred", out tarred))
            {
                _tarredBuffType = tarred.Type;
                return _tarredBuffType;
            }
            _tarredBuffType = -1;
            return _tarredBuffType;
        }
    }
}