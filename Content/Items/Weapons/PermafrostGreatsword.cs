using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses. Content.Items.Weapons
{
    public class PermafrostGreatsword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 185;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 10;
            Item. useAnimation = 10;
            Item.useStyle = ItemUseStyleID. Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item. autoReuse = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(
                texture,
                Item.position - Main.screenPosition + new Vector2(Item.width / 2, Item. height / 2),
                null,
                Color.Cyan * 0.5f,
                rotation,
                texture.Size() / 2f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        // Emit light when held
        public override void HoldItem(Player player)
        {
            Lighting.AddLight(player.Center, 0.2f, 0.5f, 1.0f);
            if (Main.rand.NextBool(3))
            {
                Vector2 dustPos = player.Center + new Vector2(Main.rand.NextFloat(-30, 30), Main.rand.NextFloat(-30, 30));
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.IceTorch, Vector2.Zero, 100, Color.LightBlue, 1.0f);
                dust.noGravity = true;
                dust.velocity = new Vector2(0, -1f);
                dust.fadeIn = 1.2f;
            }
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            // Ice trail while swinging
            if (Main.rand.NextBool(2))
            {
                Vector2 swordTip = player.Center + new Vector2(40 * player.direction, -10).RotatedBy(player.itemRotation);
                
                int dustIndex = Dust.NewDust(swordTip, 0, 0, DustID. IceTorch, 0f, 0f, 100, Color. Cyan, 1.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 0.3f;
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent. BuffType<SheerCold>(), 180);
            player. AddBuff(ModContent.BuffType<GlacialArmor>(), 180);
            player.AddBuff(BuffID.Rage, 180);
            player.AddBuff(BuffID.Wrath, 180);
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = new Vector2(Main.rand. NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                int dustIndex = Dust.NewDust(target.Center, 20, 20, DustID. IceTorch, velocity.X, velocity.Y, 100, Color.Cyan, 2.0f);
                Main. dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity = velocity;
            }
            for (int i = 0; i < 8; i++)
            {
                float angle = i / 8f * MathHelper.TwoPi;
                Vector2 velocity = new Vector2(
                    (float)System.Math.Cos(angle) * 3f,
                    (float)System.Math.Sin(angle) * 3f
                );
                Dust iceShard = Dust.NewDustPerfect(target.Center, DustID.Ice, velocity, 100, Color.White, 1.8f);
                iceShard. noGravity = true;
            }
            Lighting.AddLight(target.Center, 0.5f, 0.8f, 1.5f);
            SoundEngine.PlaySound(SoundID.Item27, target.Center);
        }
    }
}