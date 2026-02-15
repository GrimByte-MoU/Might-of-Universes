using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonHellSigil : AegonSigilBase
    {
        protected override Color SigilColor => Color.Orange;
        protected override int SigilOrder => 1;
        protected override string SigilTexturePath => "MightofUniverses/Content/NPCs/Bosses/Aegon/AegonHellSigil";
        protected override int ProjectileType => ModContent.ProjectileType<AegonHellSigilBolt>();
    }
}