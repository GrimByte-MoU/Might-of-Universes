using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Consumables
{
    public class HeartOfTheCrawlingChaos : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item2;
            Item.rare = ModContent.RarityType<EldritchRarity>();
            Item.consumable = true;
            Item.maxStack = 1;
        }

        public override bool? UseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<HeartOfTheCrawlingChaosPlayer>();
            if (!modPlayer.usedHeartOfTheCrawlingChaos)
            {
                modPlayer.usedHeartOfTheCrawlingChaos = true;
                return true;
            }
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.GetModPlayer<HeartOfTheCrawlingChaosPlayer>().usedHeartOfTheCrawlingChaos;
        }
    }

    public class HeartOfTheCrawlingChaosPlayer : ModPlayer
    {
        public bool usedHeartOfTheCrawlingChaos;

        public override void PostUpdate()
        {
            if (usedHeartOfTheCrawlingChaos)
            {
                Player.statManaMax2 += 50;
                Player.manaRegenBonus += 3;
                Player.manaCost -= 0.10f;
                Player.statLifeMax2 += 25;
                Player.lifeRegen += 5;

                if (Main.rand.NextBool(60))
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, DustID.Shadowflame, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), 150, default, 1.2f);
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["usedHeartOfTheCrawlingChaos"] = usedHeartOfTheCrawlingChaos;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("usedHeartOfTheCrawlingChaos"))
                usedHeartOfTheCrawlingChaos = tag.GetBool("usedHeartOfTheCrawlingChaos");
        }
    }
}
