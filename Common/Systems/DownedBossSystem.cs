using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Common.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool downedObeSadee = false;

        public override void ClearWorld()
        {
            downedObeSadee = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedObeSadee)
            {
                tag["downedObeSadee"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedObeSadee = tag.ContainsKey("downedObeSadee");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedObeSadee;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedObeSadee = flags[0];
        }
    }
}