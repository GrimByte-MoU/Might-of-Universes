using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class HellsMark : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 80;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Torch, 0f, 0f, 100, Color.OrangeRed, 1.0f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                
                if (Main.rand.NextBool(5))
                {
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 20f;
                    Dust.NewDustPerfect(npc.Center + offset, DustID.Torch, Vector2.Zero, 100, Color.Red, 1.5f).noGravity = true;
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 40;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch, 0f, 0f, 100, Color.OrangeRed, 1.0f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                
                if (Main.rand.NextBool(5))
                {
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 20f;
                    Dust.NewDustPerfect(player.Center + offset, DustID.Torch, Vector2.Zero, 100, Color.Red, 1.5f).noGravity = true;
                }
            }
        }
    }

    public class HellsMarkGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.DamageType == DamageClass.Magic && target.HasBuff(ModContent.BuffType<HellsMark>()))
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                    Dust.NewDust(target.Center, 10, 10, DustID.Torch, velocity.X, velocity.Y, 0, Color.OrangeRed, 1.5f);
                }
                
                target.StrikeNPC(new NPC.HitInfo { Damage = damageDone * 2, Knockback = 0 });
                
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, target.Center);
            }
        }
    }
}