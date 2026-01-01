using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TomeoftheWorldsSoul : ModItem
    {
        public static int Mode = 0; // 0 = World Channel, 1 = World's Punishment

        public override void SetDefaults()
        {
            Item.damage = 225;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Worldbolt>();
            Item.shootSpeed = 10f;
            Item.mana = 10;
            Item.maxStack = 1;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Mode = 1 - Mode;
                if (Main.myPlayer == player.whoAmI)
                {
                    CombatText.NewText(player.getRect(), Color.LimeGreen,
                        Mode == 0 ? "Channel the Worlds" : "Cleanse the Polluted");
                }
                return false;
            }
            else
            {
                if (Mode == 0)
                {
                    Item.useTime = 12;
                    Item.useAnimation = 12;
                    Item.shoot = ModContent.ProjectileType<Worldbolt>();
                }
                else
                {
                    Item.useTime = 15;
                    Item.useAnimation = 15;
                    Item.shoot = ModContent.ProjectileType<TerraBall>();
                }
                return true;
            }
        }
    }
}
