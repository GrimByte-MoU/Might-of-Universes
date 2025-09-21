using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles; // CoreFlesh_Tooth, CoreFlesh_Laser, CoreFlesh_DemonSickle

namespace MightofUniverses.Content.Items.Weapons
{
    public class CoreOfFlesh : ModItem
    {
        // 0 = Teeth, 1 = Laser Beam, 2 = Sickle of Demons
        private byte _mode;

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 20;        // default; overridden per-mode
            Item.useAnimation = 20;   // default; overridden per-mode
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            // Ensure Shoot() runs
            Item.shoot = ModContent.ProjectileType<CoreFlesh_Tooth>();
        }

        public override void HoldItem(Player player)
        {
            // Dim yellowish light while being used
            if (player.itemAnimation > 0 && player.HeldItem == Item)
            {
                Lighting.AddLight(player.Center, new Vector3(0.9f, 0.8f, 0.35f) * 0.5f);
            }
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Right-click: cycle mode (client -> server sync)
                if (player.whoAmI == Main.myPlayer)
                {
                    _mode = (byte)((_mode + 1) % 3);

                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        ModPacket p = Mod.GetPacket();
                        p.Write((byte)MoUPacketType.SwitchCoreOfFleshMode);
                        p.Write((byte)player.whoAmI);
                        p.Write(_mode);
                        p.Send();
                    }

                    string msg = _mode switch
                    {
                        0 => "Teeth",
                        1 => "Laser Beam",
                        _ => "Sickle of Demons"
                    };
                    CombatText.NewText(player.getRect(), Color.LimeGreen, msg);
                    SoundEngine.PlaySound(SoundID.MenuTick with { PitchVariance = 0.2f }, player.Center);
                }
                return false;
            }

            // Configure per-mode stats and set a valid Item.shoot (so Shoot() runs)
            switch (_mode)
            {
                case 0: // Teeth: 3/s, two fast gravity-affected teeth, 75% dmg, +2 souls on hit (projectile)
                    Item.useTime = 20;
                    Item.useAnimation = 20;
                    Item.reuseDelay = 0;
                    Item.UseSound = SoundID.Item103; // fleshy/whip-like
                    Item.knockBack = 2.0f;
                    Item.shoot = ModContent.ProjectileType<CoreFlesh_Tooth>();
                    break;

                case 1: // Laser Beam: 3-shot burst + ~0.5s pause, 80% dmg, pierce 1, high KB, no souls
                    Item.useTime = 10;
                    Item.useAnimation = 10;
                    Item.reuseDelay = 30; // ~0.5s
                    Item.UseSound = SoundID.Item91;
                    Item.knockBack = 6.5f;
                    Item.shoot = ModContent.ProjectileType<CoreFlesh_Laser>();
                    break;

                case 2: // Sickle of Demons: 3/s, 250% dmg, ignores up to 50 def (projectile), costs 30 souls/sec = 10/shot
                {
                    Item.useTime = 20;
                    Item.useAnimation = 20;
                    Item.reuseDelay = 0;
                    Item.UseSound = SoundID.Item71;
                    Item.knockBack = 3.0f;
                    Item.shoot = ModContent.ProjectileType<CoreFlesh_DemonSickle>();

                    var reaper = player.GetModPlayer<ReaperPlayer>();
                    const int costPerShot = 10; // 3 shots/sec -> 30 souls/sec
                    if (reaper.soulEnergy < costPerShot)
                    {
                        _mode = 1; // auto-switch to Laser
                        if (player.whoAmI == Main.myPlayer)
                        {
                            CombatText.NewText(player.getRect(), Color.OrangeRed, "Out of souls! Switching to Laser");
                        }
                        return false;
                    }
                    break;
                }
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (_mode == 2)
            {
                // Spend 10 per shot (3/s) -> 30/s
                var reaper = player.GetModPlayer<ReaperPlayer>();
                const int costPerShot = 10;
                if (reaper.soulEnergy >= costPerShot)
                {
                    reaper.soulEnergy -= costPerShot;
                }
                else
                {
                    _mode = 1; // fallback safety
                    if (player.whoAmI == Main.myPlayer)
                    {
                        CombatText.NewText(player.getRect(), Color.OrangeRed, "Out of souls! Switching to Laser");
                    }
                }
            }
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (_mode)
            {
                case 0: // Teeth: spawn 2 fast, gravity-affected teeth, slight spread, 75% dmg
                {
                    int scaled = (int)(damage * 0.75f);
                    float spread = MathHelper.ToRadians(4f);
                    float speed = 20f; // fast
                    for (int i = 0; i < 2; i++)
                    {
                        float t = i / 1f; // 0, 1
                        float rot = MathHelper.Lerp(-spread, spread, t);
                        Vector2 v = velocity.RotatedBy(rot).SafeNormalize(Vector2.UnitX) * speed;

                        // ai0 can be used as initial rotation; ai1 flag for "apply gravity" (handled in projectile AI)
                        Projectile.NewProjectile(
                            source,
                            position,
                            v,
                            ModContent.ProjectileType<CoreFlesh_Tooth>(),
                            scaled,
                            knockback,
                            player.whoAmI,
                            ai0: v.ToRotation(),
                            ai1: 1f // tell projectile to use gravity
                        );
                    }
                    break;
                }

                case 1: // Laser Beam: 3 lasers, 5° spread, 80% dmg, pierce 1 (projectile), high KB, no souls
                {
                    int scaled = (int)(damage * 0.80f);
                    float spread = MathHelper.ToRadians(5f);
                    float speed = 16f;
                    for (int i = 0; i < 3; i++)
                    {
                        float t = i / 2f; // 0, 0.5, 1
                        float rot = MathHelper.Lerp(-spread, spread, t);
                        Vector2 v = velocity.RotatedBy(rot).SafeNormalize(Vector2.UnitX) * speed;
                        Projectile.NewProjectile(
                            source,
                            position,
                            v,
                            ModContent.ProjectileType<CoreFlesh_Laser>(),
                            scaled,
                            knockback,
                            player.whoAmI
                        );
                    }
                    break;
                }

                case 2: // Sickle of Demons: 250% dmg, spin 0.5s then fly, ignores up to 50 def (projectile)
                {
                    int scaled = (int)(damage * 2.5f);
                    Vector2 aim = velocity.SafeNormalize(Vector2.UnitX);
                    // ai0 = initial aim rotation; ai1 = spin duration seconds (projectile should read this)
                    Projectile.NewProjectile(
                        source,
                        position,
                        aim * 0.01f, // minimal initial velocity; projectile handles its motion
                        ModContent.ProjectileType<CoreFlesh_DemonSickle>(),
                        scaled,
                        knockback,
                        player.whoAmI,
                        ai0: aim.ToRotation(),
                        ai1: 0.5f // spin time
                    );
                    break;
                }
            }

            // Suppress default shot (we spawned manually)
            return false;
        }

        // Persist/Sync the mode (for MP)
        public override void NetSend(BinaryWriter writer) => writer.Write(_mode);
        public override void NetReceive(BinaryReader reader) => _mode = reader.ReadByte();
    }

    public enum MoUPacketType : byte
    {
        SwitchCoreOfFleshMode = 1
    }
}