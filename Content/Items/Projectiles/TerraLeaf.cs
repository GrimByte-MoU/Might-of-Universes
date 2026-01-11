using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Items. Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraLeaf : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            NPC target = FindClosestNPC(400f);
            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 18f, 0.08f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Green, 1.0f);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile.Center, 0.2f, 0.5f, 0.2f);
        }

        private NPC FindClosestNPC(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc. friendly && !npc.dontTakeDamage)
                {
                    float distance = Vector2.Distance(Projectile. Center, npc.Center);
                    if (distance < closestDist)
                    {
                        closestDist = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LimeGreen;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 60);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.Green, 1.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Green, 1.0f);
                dust.noGravity = true;
            }
            return true;
        }
    }
}