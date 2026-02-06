using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PermafrostNecklace : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 2;
            player.GetModPlayer<PermafrostNecklacePlayer>().hasPermafrostNecklace = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PygmyNecklace)
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class PermafrostNecklacePlayer : ModPlayer
    {
        public bool hasPermafrostNecklace = false;

        public override void ResetEffects()
        {
            hasPermafrostNecklace = false;
        }
    }
    public class PermafrostNecklaceProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<PermafrostNecklacePlayer>().hasPermafrostNecklace)
                return;
            if (!IsValidProjectile(projectile))
                return;
            target.AddBuff(ModContent.BuffType<SheerCold>(), 180);
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.IceTorch,
                    0f, 0f, 100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }
            if (Main.rand.NextBool(3))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust ice = Dust.NewDustDirect(
                        target.position,
                        target.width,
                        target.height,
                        DustID.Ice,
                        0f, 0f, 100,
                        default,
                        2.0f
                    );
                    ice.noGravity = true;
                    ice.velocity = Main.rand.NextVector2Circular(4f, 4f);
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<PermafrostNecklacePlayer>().hasPermafrostNecklace)
                return;

            if (!IsValidProjectile(projectile))
                return;
            modifiers.ArmorPenetration += 20;
        }

        private bool IsValidProjectile(Projectile projectile)
        {
            if (projectile.minion || projectile.sentry)
                return true;
            if (ProjectileID.Sets.IsAWhip[projectile.type])
                return true;
            if (projectile.DamageType == DamageClass.Summon || projectile.DamageType == DamageClass.SummonMeleeSpeed)
                return true;

            return false;
        }
    }
}