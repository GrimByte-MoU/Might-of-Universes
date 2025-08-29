using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class EclipseHood : ModItem
    {
        public static readonly float SoulGatherBonus = 4f;
        public static readonly float ReaperDamageBonus = 0.10f;
        public static readonly float AttackSpeedBonus = 0.10f;
        public static readonly float SetBonusReaperDamage = 0.15f;
        public static readonly float SoulConsumeAttackSpeed = 0.30f;
        public static readonly float SoulConsumeReaperDamage = 0.20f;
        public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonusText = this.GetLocalization("SetBonus");
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.soulGatherMultiplier += SoulGatherBonus;
            reaperPlayer.reaperDamageMultiplier += ReaperDamageBonus;
            player.GetAttackSpeed(DamageClass.Generic) += AttackSpeedBonus;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EclipseChestplate>() && 
                   legs.type == ModContent.ItemType<EclipseLegwraps>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            player.setBonus = SetBonusText.Value;
            
            reaperPlayer.hasReaperArmor = true;
            reaperPlayer.reaperDamageMultiplier += SetBonusReaperDamage;
            reaperPlayer.maxSoulEnergy = 400f;

            SpawnEclipses(player);

            if (reaperPlayer.justConsumedSouls)
            {
                player.GetAttackSpeed(DamageClass.Generic) += SoulConsumeAttackSpeed;
                reaperPlayer.reaperDamageMultiplier += SoulConsumeReaperDamage;
            }
        }

        private void SpawnEclipses(Player player)
        {
            bool[] foundEclipses = new bool[4];
            
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<EclipseSphere>() && 
                    proj.owner == player.whoAmI)
                {
                    foundEclipses[(int)proj.ai[0]] = true;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (!foundEclipses[i])
                {
                    Projectile.NewProjectile(
                        player.GetSource_FromThis(),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<EclipseSphere>(),
                        70,
                        0f,
                        player.whoAmI,
                        ai0: i
                    );
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SolunarHelmet>())
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

