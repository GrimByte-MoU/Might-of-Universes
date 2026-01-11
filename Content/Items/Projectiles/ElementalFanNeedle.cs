using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses. Content.Items.Projectiles
{
    public class ElementalFanNeedle : MoUProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CrystalShard;

        private enum Element
        {
            Fire = 0,
            Ice = 1,
            Water = 2,
            Earth = 3,
            Nature = 4,
            Air = 5
        }

        private Element CurrentElement => (Element)Projectile.ai[0];

        public override void SafeSetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(2))
            {
                SpawnElementalDust();
            }

            EmitElementalLight();
        }

        private void SpawnElementalDust()
        {
            int dustType;
            Color dustColor;

            switch (CurrentElement)
            {
                case Element. Fire:
                    dustType = DustID.Torch;
                    dustColor = Color.Orange;
                    break;
                case Element.Ice:
                    dustType = DustID.Ice;
                    dustColor = Color.LightBlue;
                    break;
                case Element. Water:
                    dustType = DustID.Water;
                    dustColor = new Color(0, 100, 200);
                    break;
                case Element.Earth:
                    dustType = DustID.Stone;
                    dustColor = Color.Brown;
                    break;
                case Element.Nature:
                    dustType = DustID.TerraBlade;
                    dustColor = Color.Green;
                    break;
                case Element. Air:
                    dustType = Main.rand.NextBool() ? DustID.Cloud : DustID.Electric;
                    dustColor = new Color(150, 180, 200);
                    break;
                default: 
                    dustType = DustID.Torch;
                    dustColor = Color.White;
                    break;
            }

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, dustColor, 1.2f);
            dust.noGravity = true;
            dust.velocity *= 0.3f;
        }

        private void EmitElementalLight()
        {
            Vector3 lightColor;

            switch (CurrentElement)
            {
                case Element.Fire:
                    lightColor = new Vector3(1f, 0.5f, 0f);
                    break;
                case Element.Ice:
                    lightColor = new Vector3(0.5f, 0.8f, 1f);
                    break;
                case Element. Water:
                    lightColor = new Vector3(0f, 0.4f, 0.8f);
                    break;
                case Element.Earth:
                    lightColor = new Vector3(0.6f, 0.4f, 0.2f);
                    break;
                case Element.Nature:
                    lightColor = new Vector3(0.2f, 0.8f, 0.2f);
                    break;
                case Element.Air:
                    lightColor = new Vector3(0.8f, 0.9f, 1f);
                    break;
                default: 
                    lightColor = new Vector3(1f, 1f, 1f);
                    break;
            }

            Lighting.AddLight(Projectile. Center, lightColor.X * 0.5f, lightColor. Y * 0.5f, lightColor.Z * 0.5f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            switch (CurrentElement)
            {
                case Element.Fire:
                    return Color.Orange;
                case Element. Ice:
                    return Color. LightBlue;
                case Element.Water:
                    return new Color(0, 150, 255);
                case Element. Earth:
                    return Color. Brown;
                case Element.Nature:
                    return Color.Green;
                case Element.Air:
                    return new Color(180, 200, 220);
                default:
                    return Color.White;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 180);

            for (int i = 0; i < 8; i++)
            {
                SpawnElementalDust();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnElementalDust();
            }
            return true;
        }
    }
}