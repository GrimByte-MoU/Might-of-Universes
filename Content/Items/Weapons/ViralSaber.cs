using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ViralSaber : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextFloat() < 0.05f)
            {
                modifiers.FinalDamage *= 0;
                target.life = Utils.Clamp(target.life + 50, 0, target.lifeMax);
                target.HealEffect(50);
            }
            else if (Main.rand.NextFloat() < 0.30f)
            {
                modifiers.FinalDamage *= 1.1f;
                player.Heal((int)(modifiers.FinalDamage.Base * 0.05f));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SyntheticumBar>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

