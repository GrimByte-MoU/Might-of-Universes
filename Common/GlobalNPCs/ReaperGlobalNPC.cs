using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class ReaperGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        public override void OnKill(NPC npc)
        {
            // Check if the player who killed the NPC is holding the TrickstersDue
            Player player = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
            
            if (player.HeldItem.type == ModContent.ItemType<Content.Items.Weapons.TrickstersDue>())
            {
                var reaper = player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(7f, npc.Center);
            }
        }
    }
}