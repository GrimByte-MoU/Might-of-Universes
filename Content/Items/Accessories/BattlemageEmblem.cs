using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BattlemageEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass. Melee) += 0.12f;
            player.GetDamage(DamageClass. Magic) += 0.12f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.GetCritChance(DamageClass.Magic) += 10;
            player.GetModPlayer<BattlemagePlayer>().battlemageEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddIngredient(ItemID.FragmentNebula, 10)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class BattlemagePlayer : ModPlayer
    {
        public bool battlemageEffect;
        private int manaRegenTimer = 0;

        public override void ResetEffects()
        {
            battlemageEffect = false;
        }

        public override void PostUpdateEquips()
        {
            if (!battlemageEffect) return;

            if (Player.statMana > Player.statManaMax2 * 0.5f)
            {
                Player. statDefense += 10;
            }

            if (Player.HeldItem.CountsAsClass(DamageClass.Melee))
            {
                Player.manaRegen += 10;
            }

            if (manaRegenTimer > 0)
            {
                Player.manaRegen += 10;
                manaRegenTimer--;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (battlemageEffect && item. DamageType == DamageClass.Melee)
            {
                manaRegenTimer = 180;
                
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, 
                        DustID.MagicMirror, 0, 0, 100, Color. Cyan, 1.2f);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (battlemageEffect && proj.DamageType == DamageClass. Melee && proj.owner == Player.whoAmI)
            {
                manaRegenTimer = 180;
                
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, 
                        DustID.MagicMirror, 0, 0, 100, Color.Cyan, 1.2f);
                }
            }
        }
    }
}