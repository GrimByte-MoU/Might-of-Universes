using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Accessories
{
    public class AnkhFirewallProgram : ModItem
    {
        public const int DASH_DAMAGE = 100;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AnkhFirewallPlayer>().hasAnkhFirewall = true;
            player.statDefense += 12;
            player.endurance += 0.10f;
            player.noKnockback = true;

            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Burning] = true;
            player.buffImmune[BuffID.Suffocation] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Blackout] = true;
            player.buffImmune[ModContent.BuffType<PrismaticRend>()] = true;
            player.buffImmune[ModContent.BuffType<HellsMark>()] = true;
            player.buffImmune[ModContent.BuffType<Drowning>()] = true;
            player.buffImmune[ModContent.BuffType<DeadlyCorrupt>()] = true;
            player.buffImmune[ModContent.BuffType<Subjugated>()] = true;
            player.buffImmune[ModContent.BuffType<RebukingLight>()] = true;
            player.buffImmune[ModContent.BuffType<LunarReap>()] = true;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AnkhShield)
                .AddIngredient<Firewall>()
                .AddIngredient(ItemID.LunarBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class AnkhFirewallPlayer : ModPlayer
    {
        public const int DashRight = 2;
        public const int DashLeft = 3;
        public const int DashCooldown = 50;
        public const int DashDuration = 35;
        public const float DashVelocity = 12f;

        public bool hasAnkhFirewall;
        public int DashDir = -1;
        public int DashDelay = 0;
        public int DashTimer = 0;

        public override void ResetEffects()
        {
            hasAnkhFirewall = false;

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
                        DustID.Electric,
                        0f,
                        0f,
                        100,
                        Color.Cyan,
                        1.8f
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
                        int baseDamage = AnkhFirewallProgram.DASH_DAMAGE;
                        float damageMultiplier = Player.GetDamage<PacifistDamageClass>().Additive + Player.GetDamage<PacifistDamageClass>().Multiplicative - 1f;
                        int finalDamage = (int)(baseDamage * damageMultiplier);

                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = finalDamage,
                            Knockback = 10f,
                            HitDirection = DashDir == DashRight ? 1 : -1
                        });

                        npc.AddBuff(ModContent.BuffType<CodeDestabilized>(), 300);

                        SoundEngine.PlaySound(SoundID.Item14, npc.Center);

                        for (int d = 0; d < 25; d++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.Electric,
                                0f,
                                0f,
                                100,
                                Color.Cyan,
                                2.5f
                            );
                            dust.noGravity = true;
                            dust.velocity *= 2.5f;
                        }
                    }
                }

                DashTimer--;
            }
        }

        private bool CanUseDash()
        {
            return hasAnkhFirewall
                && Player.dashType == 0
                && !Player.setSolar
                && !Player.mount.Active;
        }
    }
}