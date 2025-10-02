using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class MartianTally : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 100f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 125;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MartianTallyBolt>();
            Item.shootSpeed = 14f;
            Item.scale = 1.5f;
        }

        public override void HoldItem(Player player)
        {
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: effectiveCost,
                    durationTicks: 180,
                    configure: vals =>
                    {
                        vals.ArmorPenetration += 9999;
                    }
                );
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(7f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f)));
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}