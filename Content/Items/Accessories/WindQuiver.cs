using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Rarities;
using System;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WindQuiver : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WindQuiverPlayer>().hasWindQuiver = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MoltenQuiver)
                .AddIngredient(ItemID.StalkersQuiver)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class WindQuiverPlayer : ModPlayer
    {
        public bool hasWindQuiver = false;
        private Dictionary<int, ProjectileBehaviorData> cachedBehaviors = new Dictionary<int, ProjectileBehaviorData>();
        private int lastProjectileType = -1;

        public override void ResetEffects()
        {
            hasWindQuiver = false;
        }

        public override void PostUpdate()
        {
            if (!hasWindQuiver)
                return;

            if (Player.whoAmI != Main.myPlayer)
                return;

            // Only draw while aiming with a bow
            if (Player.HeldItem.useAmmo == AmmoID.Arrow && Player.itemAnimation > 0)
            {
                DrawTrajectoryPreview();
            }
        }

        private void DrawTrajectoryPreview()
        {
            // Get shoot position and velocity
            Vector2 shootPos = Player.RotatedRelativePoint(Player.MountedCenter);
            Vector2 toMouse = Main.MouseWorld - shootPos;
            float shootSpeed = Player.HeldItem.shootSpeed;

            if (toMouse.Length() < 0.01f)
                return;

            Vector2 velocity = toMouse;
            velocity.Normalize();
            velocity *= shootSpeed;

            // Determine projectile type
            int projType = GetArrowProjectileType();

            // Get or measure projectile behavior
            if (!cachedBehaviors.ContainsKey(projType) || lastProjectileType != projType)
            {
                cachedBehaviors[projType] = MeasureProjectileBehavior(shootPos, velocity, projType);
                lastProjectileType = projType;
            }

            ProjectileBehaviorData behavior = cachedBehaviors[projType];

            // Simulate trajectory and find impact point
            SimulateAndDrawImpact(shootPos, velocity, behavior);
        }

        private void SimulateAndDrawImpact(Vector2 shootPos, Vector2 velocity, ProjectileBehaviorData behavior)
        {
            Vector2 currentPos = shootPos;
            Vector2 currentVel = velocity;
            int steps = 0;
            int maxSteps = 2000;
            int projectileWidth = 10;
            int projectileHeight = 10;

            while (steps < maxSteps)
            {
                steps++;

                // Apply gravity after delay
                if (behavior.hasGravity && steps > behavior.gravityDelay)
                {
                    currentVel.Y += behavior.gravity;
                }

                // Apply air resistance
                currentVel *= behavior.airResistance;

                // Move projectile
                currentPos += currentVel;

                // Check for NPC collision
                Rectangle projHitbox = new Rectangle((int)currentPos.X - projectileWidth / 2, (int)currentPos.Y - projectileHeight / 2, projectileWidth, projectileHeight);
                
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy())
                    {
                        Rectangle npcHitbox = npc.Hitbox;
                        if (projHitbox.Intersects(npcHitbox))
                        {
                            // Hit an enemy!
                            DrawImpactRing(npc.Center, true);
                            return;
                        }
                    }
                }

                // Check tile collision (IGNORE PLATFORMS)
                Point tilePos = currentPos.ToTileCoordinates();
                if (WorldGen.InWorld(tilePos.X, tilePos.Y))
                {
                    Tile tile = Main.tile[tilePos.X, tilePos.Y];
                    if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType])
                    {
                        // Skip platforms (tileSolidTop = true for platforms)
                        if (!Main.tileSolidTop[tile.TileType])
                        {
                            // Hit a solid block!
                            DrawImpactRing(currentPos, false);
                            return;
                        }
                    }
                }

                // Stop if velocity is too low
                if (steps > 100 && currentVel.Length() < 0.5f)
                {
                    DrawImpactRing(currentPos, false);
                    return;
                }

                // Stop if too far
                if (Vector2.Distance(shootPos, currentPos) > 6000f)
                {
                    DrawImpactRing(currentPos, false);
                    return;
                }
            }
        }

        private void DrawImpactRing(Vector2 position, bool isEnemy)
        {
            // Clean, simple ring - no electric dust
            int ringSegments = 32; // Smooth circle
            float ringRadius = 25f;

            // Color based on target type
            Color ringColor = isEnemy ? Color.Red : Color.Cyan;

            for (int i = 0; i < ringSegments; i++)
            {
                float angle = (i / (float)ringSegments) * MathHelper.TwoPi;
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * ringRadius,
                    (float)Math.Sin(angle) * ringRadius
                );

                Vector2 dustPos = position + offset;

                // Use simple dust (ID 6 = clean dot)
                Dust dust = Dust.NewDustPerfect(
                    dustPos,
                    DustID.Torch,
                    Vector2.Zero,
                    0,
                    ringColor,
                    1.0f
                );
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            // Center dot
            Dust center = Dust.NewDustPerfect(
                position,
                DustID.Torch,
                Vector2.Zero,
                0,
                ringColor,
                1.3f
            );
            center.noGravity = true;
            center.velocity = Vector2.Zero;
        }

        private ProjectileBehaviorData MeasureProjectileBehavior(Vector2 startPos, Vector2 startVel, int projType)
        {
            // Create a dummy projectile OFF-SCREEN to measure its behavior
            Vector2 measurePos = new Vector2(-10000, -10000);
            
            Projectile dummy = new Projectile();
            dummy.SetDefaults(projType);
            dummy.position = measurePos;
            dummy.velocity = startVel;
            dummy.owner = Player.whoAmI;
            dummy.active = true;
            dummy.timeLeft = 300;

            // Record velocity changes over multiple ticks
            List<Vector2> velocities = new List<Vector2>();
            List<Vector2> accelerations = new List<Vector2>();

            int measureTicks = 60; // Measure for 1 second

            for (int i = 0; i < measureTicks; i++)
            {
                Vector2 oldVel = dummy.velocity;
                
                // Run the projectile's AI
                if (ProjectileLoader.GetProjectile(projType) is ModProjectile modProj)
                {
                    // Modded projectile
                    Projectile testProj = new Projectile();
                    testProj.SetDefaults(projType);
                    testProj.position = measurePos;
                    testProj.velocity = oldVel;
                    testProj.owner = Player.whoAmI;
                    testProj.timeLeft = 300 - i;
                    testProj.AI();
                    
                    velocities.Add(testProj.velocity);
                    if (i > 0)
                    {
                        accelerations.Add(testProj.velocity - oldVel);
                    }
                }
                else
                {
                    // Vanilla projectile
                    dummy.AI();
                    velocities.Add(dummy.velocity);
                    if (i > 0)
                    {
                        accelerations.Add(dummy.velocity - oldVel);
                    }
                }

                dummy.velocity = velocities[velocities.Count - 1];
                measurePos += dummy.velocity;
                dummy.position = measurePos;
            }

            // Analyze the recorded data
            return AnalyzeBehavior(velocities, accelerations);
        }

        private ProjectileBehaviorData AnalyzeBehavior(List<Vector2> velocities, List<Vector2> accelerations)
        {
            ProjectileBehaviorData data = new ProjectileBehaviorData();

            if (accelerations.Count == 0)
            {
                data.hasGravity = false;
                data.gravity = 0f;
                data.gravityDelay = 0;
                data.airResistance = 1.0f;
                return data;
            }

            // Detect gravity delay (when Y acceleration starts)
            data.gravityDelay = 0;
            for (int i = 0; i < accelerations.Count; i++)
            {
                if (Math.Abs(accelerations[i].Y) > 0.01f)
                {
                    data.gravityDelay = i;
                    break;
                }
            }

            // Calculate average Y acceleration (gravity)
            float totalGravity = 0f;
            int gravityCount = 0;
            for (int i = data.gravityDelay; i < accelerations.Count; i++)
            {
                if (Math.Abs(accelerations[i].Y) > 0.01f)
                {
                    totalGravity += accelerations[i].Y;
                    gravityCount++;
                }
            }

            if (gravityCount > 0)
            {
                data.hasGravity = true;
                data.gravity = totalGravity / gravityCount;
            }
            else
            {
                data.hasGravity = false;
                data.gravity = 0f;
            }

            // Calculate air resistance (velocity decay)
            if (velocities.Count > 1)
            {
                float totalResistance = 0f;
                int resistCount = 0;

                for (int i = 1; i < Math.Min(20, velocities.Count); i++)
                {
                    float velRatio = velocities[i].Length() / velocities[i - 1].Length();
                    if (velRatio > 0.5f && velRatio < 1.1f)
                    {
                        totalResistance += velRatio;
                        resistCount++;
                    }
                }

                data.airResistance = resistCount > 0 ? totalResistance / resistCount : 0.99f;
            }
            else
            {
                data.airResistance = 0.99f;
            }

            return data;
        }

        private int GetArrowProjectileType()
        {
            int projType = Player.HeldItem.shoot;

            // Check inventory for arrow ammo
            if (Player.HeldItem.useAmmo == AmmoID.Arrow)
            {
                for (int i = 0; i < Player.inventory.Length; i++)
                {
                    Item ammoItem = Player.inventory[i];
                    if (ammoItem.ammo == AmmoID.Arrow && ammoItem.shoot > ProjectileID.None && ammoItem.stack > 0)
                    {
                        projType = ammoItem.shoot;
                        break;
                    }
                }
            }

            return projType;
        }
    }

    public struct ProjectileBehaviorData
    {
        public bool hasGravity;
        public float gravity;
        public int gravityDelay;
        public float airResistance;
    }

    // Global projectile for damage scaling and debuff application
    public class WindQuiverProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public float flightTime = 0f;

        public override void AI(Projectile projectile)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<WindQuiverPlayer>().hasWindQuiver)
                return;

            if (!projectile.arrow && !IsArrowProjectile(projectile.type))
                return;

            // Track flight time
            flightTime += 1f / 60f;

            // Visual effect: electric trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    DustID.Electric,
                    0f, 0f, 100,
                    Color.Cyan,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(projectile.Center, 0.2f, 0.5f, 1.0f);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<WindQuiverPlayer>().hasWindQuiver)
                return;

            if (!projectile.arrow && !IsArrowProjectile(projectile.type))
                return;

            // Calculate damage bonus (+5% per second, capped at +50%)
            float damageBonus = Math.Min(flightTime * 0.05f, 0.50f);
            modifiers.SourceDamage *= (1f + damageBonus);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<WindQuiverPlayer>().hasWindQuiver)
                return;

            if (!projectile.arrow && !IsArrowProjectile(projectile.type))
                return;

            // Apply Delta Shock for 1-3 seconds based on flight time
            int debuffDuration = 60;
            
            if (flightTime >= 2f)
                debuffDuration = 180;
            else if (flightTime >= 1f)
                debuffDuration = 120;

            target.AddBuff(ModContent.BuffType<DeltaShock>(), debuffDuration);

            // Electric hit effect
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.Electric,
                    0f, 0f, 100,
                    Color.Yellow,
                    1.8f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }

        private bool IsArrowProjectile(int type)
        {
            return ProjectileID.Sets.IsAWhip[type] == false && 
                   (type >= ProjectileID.WoodenArrowFriendly && type <= ProjectileID.BoneArrow ||
                    type == ProjectileID.ChlorophyteArrow ||
                    type == ProjectileID.IchorArrow ||
                    type == ProjectileID.CursedArrow ||
                    type == ProjectileID.HolyArrow ||
                    type == ProjectileID.HellfireArrow ||
                    type == ProjectileID.JestersArrow ||
                    type == ProjectileID.UnholyArrow ||
                    type == ProjectileID.FrostburnArrow ||
                    type == ProjectileID.BoneArrowFromMerchant ||
                    type == ProjectileID.VenomArrow ||
                    type == ProjectileID.MoonlordArrow ||
                    type == ProjectileID.DD2PhoenixBowShot ||
                    ProjectileLoader.GetProjectile(type)?.Projectile.arrow == true);
        }
    }
}