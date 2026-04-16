using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class MirrorReflectPlayer : ModPlayer
    {
        public bool hasMirror = false;
        public float reflectPercent = 0f;
        public int bonusIFrames = 0;
        public bool grantSpeedBoost = false;
        
        private int speedBoostTimer = 0;

        public override void ResetEffects()
        {
            hasMirror = false;
            reflectPercent = 0f;
            bonusIFrames = 0;
            grantSpeedBoost = false;
        }

        public override void PostUpdateEquips()
        {
            if (speedBoostTimer > 0)
            {
                Player.moveSpeed += 0.45f;
                Player.runAcceleration *= 1.45f;
            }
        }

        public override void PostUpdate()
        {
            if (speedBoostTimer > 0)
                speedBoostTimer--;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (!hasMirror)
                return;

            var empowerState = Player.GetModPlayer<ReaperEmpowermentState>();
            if (!empowerState.Empowered)
                return;

            if (bonusIFrames > 0)
            {
                Player.immuneTime += bonusIFrames;
            }

            if (grantSpeedBoost)
            {
                speedBoostTimer = 120;
            }

            if (reflectPercent <= 0f || info.Damage <= 0)
                return;

            NPC attacker = null;
            if (info.DamageSource.SourceNPCIndex >= 0 && info.DamageSource.SourceNPCIndex < Main.maxNPCs)
            {
                attacker = Main.npc[info.DamageSource.SourceNPCIndex];
            }

            if (attacker == null || !attacker.active || attacker.friendly || attacker.dontTakeDamage)
                return;

            int reflectDamage = (int)(info.Damage * reflectPercent);

            if (reflectDamage > 0)
            {
                NPC.HitInfo hit = new NPC.HitInfo
                {
                    Damage = reflectDamage,
                    Knockback = 3f,
                    HitDirection = Player.direction,
                    Crit = false
                };

                attacker.StrikeNPC(hit);

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendStrikeNPC(attacker, hit);
                }

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        attacker.position,
                        attacker.width,
                        attacker.height,
                        DustID.Glass,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        100,
                        default,
                        1.5f
                    );
                    dust.noGravity = true;
                }

                CombatText.NewText(attacker.getRect(), Color.Cyan, reflectDamage);
            }
        }
    }
}