using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Slaughterspray : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.PsychoKnife);

            Item.damage = 140;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 15);
            Item.scale = 1.25f;
            Item.maxStack = 1;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float healthPercent = player.statLife / (float)player.statLifeMax2;
            float damageBonus = healthPercent / 0.02f * 0.01f;
            
            if (damageBonus > 0.5f)
                damageBonus = 0.5f;
            
            damage += damageBonus;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<EnemyBleeding>(), 180);
            target.AddBuff(ModContent.BuffType<MortalWound>(), 180);
            
            if (target.HasBuff(ModContent.BuffType<EnemyBleeding>()))
            {
                int healAmount = (int)(damageDone * 0.005f);
                if (healAmount < 1)
                    healAmount = 1;
                    
                player.Heal(healAmount);
                
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0f, 0f, 100, default, 1.2f);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PsychoKnife)
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 12)
                .AddIngredient(ItemID.LunarBar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}