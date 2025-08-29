using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class SolunarHelmet : ModItem
    {
        public static readonly float SoulMultiplierBonus = 1f;
        public static readonly float MaxSoulEnergy = 100f;
        public static readonly float ReaperDamageBonus = 0.1f;
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
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ReaperPlayer>().soulGatherMultiplier += SoulMultiplierBonus;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SolunarBreastplate>() && 
                   legs.type == ModContent.ItemType<SolunarLegwraps>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            player.setBonus = SetBonusText.Value;
            
            reaperPlayer.hasReaperArmor = true;
            reaperPlayer.maxSoulEnergy = MaxSoulEnergy;
            reaperPlayer.reaperDamageMultiplier += ReaperDamageBonus;

            SpawnMedallions(player);

            if (reaperPlayer.justConsumedSouls)
            {
                player.Heal(20);
                player.AddBuff(ModContent.BuffType<SolunarEmpowerment>(), 300);
            }
        }

        private void SpawnMedallions(Player player)
        {
            bool[] foundMedallions = new bool[2];

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<SolunarMedallion>() && 
                    proj.owner == player.whoAmI)
                {
                    foundMedallions[(int)proj.ai[0]] = true;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (!foundMedallions[i])
                {
                    Projectile.NewProjectile(
                        player.GetSource_FromThis(),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<SolunarMedallion>(),
                        30,
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
                .AddIngredient(ModContent.ItemType<SolunarToken>(), 10)
                .AddIngredient(ModContent.ItemType<DesertWrappings>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

