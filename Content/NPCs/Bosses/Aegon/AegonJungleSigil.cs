using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonJungleSigil : AegonSigilBase
    {
        protected override Color SigilColor => Color.LimeGreen;
        protected override int SigilOrder => 4;
        protected override string SigilTexturePath => "MightofUniverses/Content/NPCs/Bosses/Aegon/AegonJungleSigil";
        protected override int ProjectileType => ModContent.ProjectileType<AegonJungleSigilBolt>();
    }
}