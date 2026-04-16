using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class DebuffDeathMessagePlayer : ModPlayer
    {
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound,
            ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player.HasBuff(ModContent.BuffType<CoreHeat>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} was melted into slag."));

            else if (Player.HasBuff(ModContent.BuffType<SheerCold>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} froze solid."));

            else if (Player.HasBuff(ModContent.BuffType<DeltaShock>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} was fried by thousands of volts."));

            else if (Player.HasBuff(ModContent.BuffType<NaturesToxin>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} succumbed to a unique toxin."));

            else if (Player.HasBuff(ModContent.BuffType<RebukingLight>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} died a luminous death."));

            else if (Player.HasBuff(ModContent.BuffType<PrismaticRend>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} burst into an array of color."));

            else if (Player.HasBuff(ModContent.BuffType<CodeDestabilized>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} became too unstable to remain in the world."));

            else if (Player.HasBuff(ModContent.BuffType<Corrupted>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name}'s body and soul was burned out."));

            else if (Player.HasBuff(ModContent.BuffType<DeadlyCorrupt>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name}'s body and soul was burned out."));

            else if (Player.HasBuff(ModContent.BuffType<Demonfire>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} was burned by demonic fires."));

            else if (Player.HasBuff(ModContent.BuffType<Drowning>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} drowned."));

            else if (Player.HasBuff(ModContent.BuffType<ElementsHarmony>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} was utterly eradicated by a combination of elements."));

            else if (Player.HasBuff(ModContent.BuffType<GoblinsCurse>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"The curse caught up to {Player.name}."));

            else if (Player.HasBuff(ModContent.BuffType<HellsMark>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"The mark of the underworld decimated its target."));

            else if (Player.HasBuff(ModContent.BuffType<LordsVenom>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} succumbed to the Gambit Tactician's venom."));

            else if (Player.HasBuff(ModContent.BuffType<OceanPressure>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} was crushed by the pressure of the ocean."));

            else if (Player.HasBuff(ModContent.BuffType<OminousPrice>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name} could not pay the price."));

            else if (Player.HasBuff(ModContent.BuffType<SugarCrash>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name}'s body shut down due to sugar."));

            else if (Player.HasBuff(ModContent.BuffType<Sunfire>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"The fires of the sun disintegrated {Player.name}."));

            else if (Player.HasBuff(ModContent.BuffType<TerrasRend>()))
                damageSource = PlayerDeathReason.ByCustomReason(
                    NetworkText.FromLiteral($"{Player.name}'s essence was torn asunder."));

            return true;
        }
    }
}