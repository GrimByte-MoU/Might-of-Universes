using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GreenChaliceDrop : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 1f, 0.5f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 300); // 5 seconds
            target.AddBuff(BuffID.Midas, 120); // 2 seconds
            Player player = Main.player[Projectile.owner];
            player.statLife += (int)(damageDone * 0.1f); // Lifesteal
            player.HealEffect((int)(damageDone * 0.1f), true);
            Main.player[Projectile.owner].GetModPlayer<ReaperPlayer>().AddSoulEnergy(5f, target.Center);
        }
    }
}
