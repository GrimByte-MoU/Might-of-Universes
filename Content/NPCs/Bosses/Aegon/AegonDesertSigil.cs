using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonDesertSigil : AegonSigilBase
    {
        protected override Color SigilColor => Color.Yellow;
        protected override int SigilOrder => 3;
        protected override string SigilTexturePath => "MightofUniverses/Content/NPCs/Bosses/Aegon/AegonDesertSigil";
        protected override int ProjectileType => ModContent.ProjectileType<AegonDesertSigilBolt>();
    }
}