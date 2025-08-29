using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TungstenScythe : ModItem
    {
        private int regenTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 17;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(1f, target.Center);
        }

       public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    
if (ReaperPlayer.SoulReleaseKey.JustPressed)



    {
        if (reaper.ConsumeSoulEnergy(40f))
        {
            player.lifeRegen += 7;
            player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.1f;
            regenTimer = 180; // 3 seconds
            Main.NewText("40 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }
    return true;
}


        public override void UpdateInventory(Player player)
        {
            if (regenTimer > 0)
            {
                regenTimer--;
                if (regenTimer <= 0)
                {
                    player.lifeRegen -= 7;
                    player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) -= 0.1f;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}