using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MightofUniverses.Content.NPCs.Bosses.Minions
{
    public class ObeSadeeHealerMinion : ModNPC
    {
        private int healTimer = 0;
        private NPC boss;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 20;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.aiStyle = -1;
        }

        public override void AI()
        {
            if (boss == null || !boss.active || boss.type != ModContent.NPCType<ObeSadee>())
            {
                NPC.active = false;
                return;
            }

            boss = Main.npc[(int)NPC.ai[0]];
            float distance = Main.expertMode ? 240f : 320f; // 15 or 20 tiles
            float speed = Main.expertMode ? 0.1f : 0.075f;

            NPC.Center = boss.Center + Vector2.One.RotatedBy(NPC.ai[1]) * distance;
            NPC.ai[1] += speed;

            healTimer++;
            if (healTimer >= (Main.expertMode ? 60 : 120))
            {
                healTimer = 0;
                if (boss.life < boss.lifeMax)
                {
                    boss.life += 200;
                    boss.HealEffect(200);
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, boss.whoAmI);
                }
            }
        }
        public override void FindFrame(int frameHeight)
{
    NPC.frameCounter += 0.2;
    if (NPC.frameCounter >= Main.npcFrameCount[NPC.type])
        NPC.frameCounter = 0;

    NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
}

    }
}