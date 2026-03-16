using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Firewall : ModItem
    {
        public const int DASH_DAMAGE = 50;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FirewallPlayer>().hasFirewall = true;
            player.statDefense += 6;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.noKnockback = true;
            player.fireWalk = true;
        }
    }

    public class FirewallPlayer : ModPlayer
    {
        public const int DashRight = 2;
        public const int DashLeft = 3;
        public const int DashCooldown = 50;
        public const int DashDuration = 35;
        public const float DashVelocity = 10f;

        public bool hasFirewall;
        public int DashDir = -1;
        public int DashDelay = 0;
        public int DashTimer = 0;

        public override void ResetEffects()
        {
            hasFirewall = false;

            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                DashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                DashDir = DashLeft;
            }
            else
            {
                DashDir = -1;
            }
        }

        public override void PreUpdateMovement()
        {
            if (CanUseDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    case DashLeft when Player.velocity.X > -DashVelocity:
                    case DashRight when Player.velocity.X < DashVelocity:
                        float dashDirection = DashDir == DashRight ? 1 : -1;
                        newVelocity.X = dashDirection * DashVelocity;
                        break;
                    default:
                        return;
                }

                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;

                SoundEngine.PlaySound(SoundID.Item74, Player.Center);
            }

            if (DashDelay > 0)
                DashDelay--;

            if (DashTimer > 0)
            {
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;
                Player.immune = true;
                Player.immuneTime = 6;

                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        Player.position,
                        Player.width,
                        Player.height,
                        DustID.GreenTorch,
                        0f,
                        0f,
                        100,
                        default,
                        1.5f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                }

                Rectangle hitbox = Player.Hitbox;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && hitbox.Intersects(npc.Hitbox))
                    {
                        int baseDamage = Firewall.DASH_DAMAGE;
                        float damageMultiplier = Player.GetDamage<PacifistDamageClass>().Additive + Player.GetDamage<PacifistDamageClass>().Multiplicative - 1f;
                        int finalDamage = (int)(baseDamage * damageMultiplier);

                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = finalDamage,
                            Knockback = 8f,
                            HitDirection = DashDir == DashRight ? 1 : -1
                        });

                        SoundEngine.PlaySound(SoundID.Item14, npc.Center);

                        for (int d = 0; d < 20; d++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.GreenTorch,
                                0f,
                                0f,
                                100,
                                default,
                                2f
                            );
                            dust.noGravity = true;
                            dust.velocity *= 2f;
                        }
                    }
                }

                DashTimer--;
            }
        }

        private bool CanUseDash()
        {
            return hasFirewall
                && Player.dashType == 0
                && !Player.setSolar
                && !Player.mount.Active;
        }
    }
}