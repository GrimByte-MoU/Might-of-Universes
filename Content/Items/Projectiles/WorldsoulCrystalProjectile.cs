using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WorldsoulCrystalProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 420;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.TerraBlade, 0, 0, 120, Color.LimeGreen, 1.2f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(7f, target.Center);
        }
    }
}