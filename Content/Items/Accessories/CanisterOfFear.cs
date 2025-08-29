using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CanisterOfFear : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FearPlayer>().hasCanisterOfFear = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PureTerror>(), 5)
                .AddIngredient(ItemID.Bottle, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class FearPlayer : ModPlayer
    {
        public bool hasCanisterOfFear;

        public override void ResetEffects()
        {
            hasCanisterOfFear = false;
        }

        public override void PostUpdate()
        {
            if (!hasCanisterOfFear) return;

            Lighting.AddLight(Player.Center, Color.Orange.ToVector3() * 1.5f);

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Player.Center) < 13 * 16f)
                {
                    npc.AddBuff(ModContent.BuffType<Terrified>(), 180);
                    Player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(2f / 60f, npc.Center);
                }
            }
        }
    }
}

