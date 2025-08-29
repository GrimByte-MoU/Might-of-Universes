using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
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
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Only summon if not already present
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            }
            else
            {
                // Empower the shield
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.owner == player.whoAmI && proj.type == Item.shoot)
                    {
                        proj.localAI[0]++; // Track minion slots consumed
                        int minionSlots = (int)proj.localAI[0];

                        proj.damage = (int)(150 * (1 + 0.15f * minionSlots));
                        proj.localAI[1] = 1 + (minionSlots / 3); // Pierce block scaling
                        proj.netUpdate = true;
                    }
                }
            }
            player.AddBuff(Item.buffType, 2);
            return false;
        }
    }
}
