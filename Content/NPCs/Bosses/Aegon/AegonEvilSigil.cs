using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonEvilSigil : AegonSigilBase
    {
        protected override Color SigilColor => Color.Lerp(Color.Red, Color.Purple, 0.5f);
        protected override int SigilOrder => 2;
        protected override string SigilTexturePath => "MightofUniverses/Content/NPCs/Bosses/Aegon/AegonEvilSigil";
        protected override int ProjectileType => ModContent.ProjectileType<AegonEvilSigilBolt>();
    }
}