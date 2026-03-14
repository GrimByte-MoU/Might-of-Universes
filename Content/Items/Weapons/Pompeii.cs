using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;


namespace MightofUniverses.Content.Items.Weapons
{
    public class Pompeii : ModItem, IHasSoulCost, IScytheWeapon
    {
        public float BaseSoulCost => 300f;

        public override void SetDefaults()
        {
            Item.damage = 145;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item74;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PompeiiFireball>();
            Item.shootSpeed = 18f;
            Item.maxStack = 1;
        }

       public override void HoldItem(Player player)
{
    var reaper = player.GetModPlayer<ReaperPlayer>();

    if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
    {        
        if (reaper.ConsumeSoulEnergy(BaseSoulCost))
        {
            Vector2 meteorSpawn = player.Center + new Vector2(0, -800f);
            
            NPC target = FindNearestEnemy(player);
            if (target != null)
            {
                Vector2 meteorVelocity = (target.Center - meteorSpawn).SafeNormalize(Vector2.UnitY) * 20f;
                
                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);

                Projectile.NewProjectile(
                    src,
                    meteorSpawn,
                    meteorVelocity,
                    ModContent.ProjectileType<PompeiiMeteor>(),
                    (int)(damage * 3f),
                    kb * 2f,
                    player.whoAmI
                );
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, player.Center);
            }
        }
    }
}

        private NPC FindNearestEnemy(Player player)
        {
            NPC nearest = null;
            float minDist = 2000f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(player.Center, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = npc;
                    }
                }
            }

            return nearest;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(6f, target.Center);
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 120);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MeteoriteHarvester>(), 1)
                .AddIngredient(ModContent.ItemType<HellsTalon>(), 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}