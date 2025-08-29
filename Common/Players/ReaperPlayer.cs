using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameInput;
using System;

public class ReaperPlayer : ModPlayer
{
    public float soulEnergy;
    public float maxSoulEnergy = 100f;
    public float soulGatherMultiplier = 1f;
    public bool hasReaperArmor;
    public int deathMarks;
    public const int MAX_DEATH_MARKS = 5;
    public static ModKeybind SoulReleaseKey;
    public float reaperDamageMultiplier = 1f;
    public float reaperCritChance = 0f;
    public bool justConsumedSouls;


    public int TempleBuffTimer;

    public override void Load()
    {
        SoulReleaseKey = KeybindLoader.RegisterKeybind(Mod, "Release Soul Energy", "R");
    }

    public override void Unload()
    {
        SoulReleaseKey = null;
    }

    public override void Initialize()
    {
        soulEnergy = 0f;
        soulGatherMultiplier = 1f;
        hasReaperArmor = false;
        reaperDamageMultiplier = 1f;
        reaperCritChance = 0f;
        justConsumedSouls = false;
        TempleBuffTimer = 0;
    }

    public override void ResetEffects()
    {
        maxSoulEnergy = 100f;
        soulGatherMultiplier = 1f;
        hasReaperArmor = false;
        reaperDamageMultiplier = 1f;
        reaperCritChance = 0f;
        justConsumedSouls = false;

        if (TempleBuffTimer > 0)
        {
            TempleBuffTimer--;
        }
    }

    public void AddSoulEnergy(float amount, Vector2 sourcePosition)
    {
        float adjustedAmount = amount * soulGatherMultiplier;
        if (soulEnergy < maxSoulEnergy)
        {
            soulEnergy = MathHelper.Clamp(soulEnergy + adjustedAmount, 0f, maxSoulEnergy);

            // Create soul-gathering dust trail effect
            Vector2 vectorToPlayer = Player.Center - sourcePosition;
            float distance = vectorToPlayer.Length();
            vectorToPlayer.Normalize();

            for (int i = 0; i < 10; i++)
            {
                Vector2 dustPosition = sourcePosition + vectorToPlayer * distance * (i / 10f);
                Dust dust = Dust.NewDustPerfect(
                    dustPosition,
                    DustID.WhiteTorch,
                    vectorToPlayer * 5f,
                    0,
                    Color.White,
                    1f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }
        }
    }

    public bool ConsumeSoulEnergy(float amount)
    {
        if (soulEnergy >= amount)
        {
            soulEnergy -= amount;
            justConsumedSouls = true;
            return true;
        }
        return false;
    }

    public bool TryReleaseSouls(float cost, Action<Player> onSuccess, string releaseMessage = null)
    {
        if (SoulReleaseKey.JustPressed)
        {
            if (ConsumeSoulEnergy(cost))
            {
                onSuccess?.Invoke(Player);
                Main.NewText(releaseMessage ?? $"{(int)cost} souls released!", Color.Green);
                return true;
            }
            else
            {
                Main.NewText("Not enough soul energy to activate!", Color.Red);
            }
        }
        return false;
    }

    public void UpdateReaperDamageMultiplier(float amount)
    {
        reaperDamageMultiplier = MathHelper.Clamp(reaperDamageMultiplier + amount, 1f, 10f);
    }

    public void UpdateReaperCritChance(float amount)
    {
        reaperCritChance = MathHelper.Clamp(reaperCritChance + amount, 0f, 100f);
    }
}