using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraBall : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.5f, 1f, 0.5f);

            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            int debuffCount = 0;
            for (int i = 0; i < target.buffType.Length; i++)
            {
                if (target.buffType[i] > 0) debuffCount++;
            }
            if (debuffCount > 0)
                modifiers.SourceDamage += 0.20f * debuffCount;

            // Reduce all debuff durations by 1s
            for (int i = 0; i < target.buffType.Length; i++)
            {
                if (target.buffType[i] > 0 && target.buffTime[i] > 30)
                    target.buffTime[i] -= 30;
            }
        }
    }
}
