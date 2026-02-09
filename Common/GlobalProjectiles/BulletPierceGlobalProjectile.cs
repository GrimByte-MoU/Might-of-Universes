using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common
{
    public class BulletPierceGlobalProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            var player = Main.player[projectile.owner];
            int extraPierce = player.GetModPlayer<BulletPiercingPlayer>().bulletPiercing;
            if (extraPierce <= 0)
                return;

            bool isBullet = false;
            if (source is EntitySource_ItemUse_WithAmmo withAmmo && withAmmo.AmmoItemIdUsed > 0)
            {

                var ammoItem = ContentSamples.ItemsByType[withAmmo.AmmoItemIdUsed];
                if (ammoItem != null && ammoItem.ammo == AmmoID.Bullet)
                    isBullet = true;
            }

            if (!isBullet)
                return;

            if (projectile.penetrate > 0)
            {
                projectile.penetrate += extraPierce;
                if (!projectile.usesLocalNPCImmunity)
                {
                    projectile.usesLocalNPCImmunity = true;
                }
            }
        }
    }
}