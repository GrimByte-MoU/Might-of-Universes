using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using  MightofUniverses.Common.Players;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class BulletPiercingProjectile : GlobalProjectile
    {
public override void OnSpawn(Projectile projectile, IEntitySource source)
{
    if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Player != null && projectile.type == ProjectileID.Bullet)
    {
        projectile.penetrate += itemSource.Player.GetModPlayer<BulletPiercingPlayer>().bulletPiercing;
    }
}



        }
    }
