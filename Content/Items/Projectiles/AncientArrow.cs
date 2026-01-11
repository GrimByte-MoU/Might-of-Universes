using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AncientArrow : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile. penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.arrow = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 15f)
            {
                Projectile.velocity.Y += 0.25f;
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID. TerraBlade, 0, 0, 100, Color. Gold, 1.4f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.7f, 0.3f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color. Lerp(Color.LimeGreen, Color.Gold, 0.5f);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            int bonusDamage = target.defense * 2;
            modifiers. FlatBonusDamage += bonusDamage;
            modifiers. DisableKnockback();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target. width, target.height, DustID.TerraBlade, 0, 0, 100, Color.Gold, 1.8f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand. NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Gold, 1.3f);
                dust.noGravity = true;
            }
            return true;
        }
    }
}