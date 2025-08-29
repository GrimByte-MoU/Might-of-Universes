using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ArcaneGladius : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 72;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Pink;
            Item.mana = 12;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.UseSound = SoundID.Item43;
        }

        public override void HoldItem(Player player)
        {
            player.GetModPlayer<ArcaneGladiusPlayer>().holdingGladius = true;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<ArcaneGladiusProjectile>()] < 3)
            {
                Vector2 spawnPos = player.Center;
                Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), spawnPos, Vector2.Zero, ModContent.ProjectileType<ArcaneGladiusProjectile>(), Item.damage, Item.knockBack, player.whoAmI);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MortalWound>(), 180);
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SkyFracture)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class ArcaneGladiusPlayer : ModPlayer
    {
        public bool holdingGladius = false;

        public override void ResetEffects()
        {
            holdingGladius = false;
        }

        public override void PostUpdate()
        {
            if (holdingGladius)
            {
                Rectangle hitbox = Player.Hitbox;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Hitbox.Intersects(hitbox))
                    {
                        int damage = (int)(Player.HeldItem.damage * 1.25f);
                        npc.StrikeNPC(new NPC.HitInfo()
                        {
                            Damage = damage,
                            DamageType = DamageClass.Melee
                        }, false);
                    }
                }
            }
        }
    }
}
