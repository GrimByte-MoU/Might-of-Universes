using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Weapons
{
    public class FrozenEnforcerProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool FromFrozenEnforcer = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource)
            {
                if (itemSource.Item.type == ModContent.ItemType<FrozenEnforcer>())
                {
                    FromFrozenEnforcer = true;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (FromFrozenEnforcer)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }
    }
}