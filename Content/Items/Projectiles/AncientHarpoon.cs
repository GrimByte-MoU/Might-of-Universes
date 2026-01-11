using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses. Content.Items.Projectiles
{
    public class AncientHarpoon : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile. penetrate = 7;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile. ignoreWater = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Gold, 2.0f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.8f, 0.7f, 0.3f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Gold;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust. NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.Gold, 2.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand. NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, target.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Gold, 1.5f);
                dust. noGravity = true;
            }
            return true;
        }
    }
}