using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonHallowedSigil : AegonSigilBase
    {
        protected override Color SigilColor => new Color(173, 216, 230);
        protected override int SigilOrder => 0;
        protected override string SigilTexturePath => "MightofUniverses/Content/NPCs/Bosses/Aegon/AegonHallowedSigil";
        protected override int ProjectileType => ModContent.ProjectileType<AegonHallowedSigilBolt>();
    }
}