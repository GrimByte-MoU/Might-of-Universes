// Content/NPCs/Bosses/Aegon/AegonArena.cs

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonArena
    {
        public static AegonArena Current;
        public Vector2 Center { get; private set; }
        public float Radius { get; private set; }
        public bool IsActive { get; private set; }
        private Dictionary<Point, TileData> originalTiles = new Dictionary<Point, TileData>();

        private struct TileData
        {
            public ushort type;
            public ushort wall;
            public bool active;
        }
        private const float ARENA_RADIUS_NORMAL = 65.5f;
        private const float ARENA_RADIUS_EXPERT = 57.5f;
        private const float ARENA_RADIUS_MASTER = 50.5f;

        public AegonArena(Vector2 center)
        {
            Center = center;

            if (Main.masterMode)
                Radius = ARENA_RADIUS_MASTER;
            else if (Main.expertMode)
                Radius = ARENA_RADIUS_EXPERT;
            else
                Radius = ARENA_RADIUS_NORMAL;
        }

        public void Create()
        {
            if (IsActive)
            {
                Main.NewText("[ARENA DEBUG] Arena already active, skipping creation", Color.Yellow);
                return;
            }

            Main.NewText("[ARENA DEBUG] Creating arena...", Color.Green);

            Point centerTile = Center. ToTileCoordinates();
            int radiusTiles = (int)Radius;

            for (int x = -radiusTiles; x <= radiusTiles; x++)
            {
                for (int y = -radiusTiles; y <= radiusTiles; y++)
                {
                    int worldX = centerTile.X + x;
                    int worldY = centerTile.Y + y;

                    if (worldX < 0 || worldX >= Main.maxTilesX || worldY < 0 || worldY >= Main.maxTilesY)
                        continue;

                    float distance = new Vector2(x, y).Length();

                    if (distance >= Radius - 1 && distance <= Radius)
                    {
                        Point tilePos = new Point(worldX, worldY);
                        Tile tile = Main.tile[worldX, worldY];

                        originalTiles[tilePos] = new TileData
                        {
                            type = tile.TileType,
                            wall = tile.WallType,
                            active = tile.HasTile
                        };

                        tile.TileType = TileID.WoodenSpikes;
                        tile.HasTile = true;
                        
                        if (Main. netMode != NetmodeID. SinglePlayer)
                        {
                            NetMessage.SendTileSquare(-1, worldX, worldY, 1);
                        }
                    }
                }
            }

            IsActive = true;
            Main. NewText($"[ARENA DEBUG] Arena created!  {originalTiles.Count} tiles stored", Color.Green);

            CreateDustRing();
        }

        public void Remove()
        {
            Main.NewText($"[ARENA DEBUG] Remove() called.  IsActive={IsActive}, Tiles stored={originalTiles.Count}", Color.Orange);

            int restoredCount = 0;
            foreach (var kvp in originalTiles)
            {
                Point pos = kvp.Key;
                TileData data = kvp.Value;

                if (pos.X < 0 || pos.X >= Main.maxTilesX || pos.Y < 0 || pos.Y >= Main.maxTilesY)
                    continue;

                Tile tile = Main.tile[pos.X, pos.Y];
                tile.TileType = data.type;
                tile.WallType = data.wall;
                tile.HasTile = data.active;

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendTileSquare(-1, pos.X, pos.Y, 1);
                }
                
                restoredCount++;
            }

            Main.NewText($"[ARENA DEBUG] Restored {restoredCount} tiles", Color.Green);

            originalTiles.Clear();
            IsActive = false;

            CreateDustExplosion();
            
            Main.NewText("[ARENA DEBUG] Arena removed successfully!", Color.Green);
        }

        public bool IsInsideArena(Vector2 position)
        {
            return Vector2.Distance(position, Center) <= (Radius * 16f); // Convert tiles to pixels
        }

        public bool IsOutsideArena(Vector2 position)
        {
            return ! IsInsideArena(position);
        }

        public Vector2 ClampToArena(Vector2 position)
        {
            Vector2 direction = position - Center;
            float distance = direction.Length();
            float maxDistance = (Radius - 2) * 16f;

            if (distance > maxDistance)
            {
                direction. Normalize();
                return Center + direction * maxDistance;
            }

            return position;
        }

        public bool CanBreakTile(int x, int y)
        {
            if (!IsActive) return true;

            Point tilePos = new Point(x, y);
            
            if (originalTiles.ContainsKey(tilePos))
            {
                Tile tile = Main.tile[x, y];
                if (tile.TileType == TileID.WoodenSpikes)
                    return false;
            }

            return true;
        }

        private void CreateDustRing()
        {
            int segments = 200;
            for (int i = 0; i < segments; i++)
            {
                float angle = i / (float)segments * MathHelper.TwoPi;
                Vector2 position = Center + new Vector2(
                    (float)Math.Cos(angle) * Radius * 16f,
                    (float)Math.Sin(angle) * Radius * 16f
                );

                int dust = Dust.NewDust(position, 0, 0, DustID. Stone, 0f, 0f, 100, Color.Brown, 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Vector2.Zero;
            }
        }

        private void CreateDustExplosion()
        {
            int segments = 100;
            for (int i = 0; i < segments; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(Radius * 16f);
                Vector2 position = Center + new Vector2(
                    (float)Math.Cos(angle) * distance,
                    (float)Math.Sin(angle) * distance
                );

                int dust = Dust. NewDust(position, 0, 0, DustID. Stone, 0f, 0f, 100, Color.Brown, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = (position - Center).SafeNormalize(Vector2.Zero) * 3f;
            }
        }
    }
}