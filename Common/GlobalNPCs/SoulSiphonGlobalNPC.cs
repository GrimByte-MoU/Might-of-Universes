using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class SoulSiphonGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.SpawnedFromStatue) return;
            if (npc.friendly || npc.lifeMax <= 1) return;

            // Determine the player who killed it (owner of last hit)
            Player killer = Main.player[npc.lastInteraction];
            if (killer is null || !killer.active) return;

            var acc = killer.GetModPlayer<ReaperAccessoryPlayer>();

            // Highest tier supersedes lower tiers
            if (acc.accSpectercageArtifact)
            {
                Vector2 spawn = npc.Center + new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-20f, 20f));
                Projectile.NewProjectile(
                    npc.GetSource_Death(),
                    spawn,
                    Main.rand.NextVector2Circular(2f, 2f),
                    ModContent.ProjectileType<SpecterOrbProj>(),
                    0,
                    0f,
                    killer.whoAmI
                );
                return;
            }

            if (acc.accSoulEnslavementArtifact)
            {
                int count = Main.rand.Next(2, 4); // 2–3 wisps
                for (int i = 0; i < count; i++)
                {
                    Vector2 spawn = npc.Center + new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-20f, 20f));
                    int p = Projectile.NewProjectile(
                        npc.GetSource_Death(),
                        spawn,
                        new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, -2f)),
                        ModContent.ProjectileType<SoulWispOrb>(),
                        0,
                        0f,
                        killer.whoAmI
                    );
                    // Small stagger so they don't all reach at once
                    if (p >= 0) Main.projectile[p].timeLeft += i * 10;
                }
                return;
            }

            if (acc.accShackledArtifact)
            {
                int count = Main.rand.Next(1, 4); // 1–3 wisps
                for (int i = 0; i < count; i++)
                {
                    Vector2 spawn = npc.Center + new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-20f, 20f));
                    int p = Projectile.NewProjectile(
                        npc.GetSource_Death(),
                        spawn,
                        new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, -2f)),
                        ModContent.ProjectileType<SoulWispOrb>(),
                        0,
                        0f,
                        killer.whoAmI
                    );
                    if (p >= 0) Main.projectile[p].timeLeft += i * 10;
                }
                return;
            }

            // Fallback: original Soul Siphoning Artifact behavior (your existing first form)
            if (acc.HasSoulSiphoningArtifact)
            {
                int count = Main.rand.Next(1, 3); // 1–2 wisps
                for (int i = 0; i < count; i++)
                {
                    Vector2 spawn = npc.Center + new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-20f, 20f));
                    int p = Projectile.NewProjectile(
                        npc.GetSource_Death(),
                        spawn,
                        new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, -2f)),
                        ModContent.ProjectileType<SoulWispOrb>(),
                        0,
                        0f,
                        killer.whoAmI
                    );
                    if (p >= 0) Main.projectile[p].timeLeft += i * 10;
                }
            }
        }
    }
}