using Microsoft.Xna. Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Content. Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses. Content.Items.Weapons
{
    public class MouldoftheMother : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.mana = 12;
            Item.DamageType = DamageClass.Summon;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.buffType = ModContent.BuffType<MiniAegisBuff>();
            Item.shoot = ModContent.ProjectileType<MiniAegis>();
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            
            if (player. ownedProjectileCounts[Item.shoot] == 0)
            {
                var proj = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
                proj.originalDamage = Item.damage;
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
            }
            else
            {
                Projectile existingAegis = null;
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.owner == player.whoAmI && proj.type == Item.shoot)
                    {
                        existingAegis = proj;
                        break;
                    }
                }

                if (existingAegis != null)
                {
                    int currentEmpowerLevel = (int)existingAegis.localAI[0];
                    int newEmpowerLevel = currentEmpowerLevel + 1;
                    
                    // FIXED: Calculate as float, then check
                    float usedSlots = player.slotsMinions;
                    float maxSlots = player.maxMinions;
                    float availableSlots = maxSlots - usedSlots;

                    if (availableSlots >= 1f)
                    {
                        existingAegis.localAI[0]++;
                        int empowerLevel = (int)existingAegis.localAI[0];

                        existingAegis.originalDamage = (int)(Item.damage * (1 + 0.15f * empowerLevel));
                        existingAegis.localAI[1] = 1 + (empowerLevel / 3);
                        existingAegis.netUpdate = true;
                        
                        SoundEngine.PlaySound(SoundID.Item29, existingAegis.Center);
                        
                        for (int i = 0; i < 20; i++)
                        {
                            Dust.NewDust(existingAegis.position, existingAegis.width, existingAegis.height,
                                DustID.RainbowMk2, 0f, 0f, 100, default, 1.5f);
                        }

                        CombatText.NewText(existingAegis.getRect(), Color.Gold, $"Empowered!  Level {empowerLevel + 1}", true);
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.MenuClose, player.Center);
                        CombatText.NewText(player.getRect(), Color.Red, "Not enough minion slots!", true);
                    }
                }
            }
            
            return false;
        }
    }
}