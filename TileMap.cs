using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject4
{
    public class TileMap
    {
        public List<MineTile> Tiles = new List<MineTile>();
        public int Width = 31;
        public int Height = 130;
        public int TileSize = 32;

        private Dictionary<string, Rectangle> oreTextures = new();

        public void LoadTextures()
        {
            oreTextures.Clear();
            oreTextures["Dirt"] = new Rectangle(0, 0, 32, 32);
            oreTextures["Stone"] = new Rectangle(32, 0, 32, 32);
            oreTextures["Coal"] = new Rectangle(64, 0, 32, 32);
            oreTextures["Copper"] = new Rectangle(96, 0, 32, 32);
            oreTextures["Iron"] = new Rectangle(128, 0, 32, 32);
            oreTextures["Gold"] = new Rectangle(160, 0, 32, 32);
            oreTextures["Diamond"] = new Rectangle(192, 0, 32, 32);
        }

        public string GetOreFromSource(Rectangle source)
        {
            foreach(var o in oreTextures)
            {
                if(o.Value == source)
                {
                    return o.Key;
                }
            }
            return "Dirt";
        }

        public GameSaveData ToSaveData(Player player)
        {
            GameSaveData save = new GameSaveData { PlayerMoney = player.Money, PlayerClickPower = player.ClickPower };

            save.Tiles.Clear();
            foreach (MineTile t in Tiles)
            {
                string oreType = GetOreFromSource(t.Source);

                save.Tiles.Add(new MineTileSaveData
                {
                    X = t.Position.X,
                    Y = t.Position.Y,
                    SourceX = t.Source.X,
                    SourceY = t.Source.Y,
                    IsMined = t.IsMined,
                    RemainingHealth = t.RemainingHealth,
                    Value = t.Value,
                    OreType = oreType
                });
            }

            return save;
        }

        public void GenerateDefaultMine()
        {
            Tiles.Clear();
            for (int y = 0; y < Height; y++)
            {
                string oreType;
                int hardness;
                int value;

                if (y < 1) { oreType = "Dirt"; hardness = 1; value = 0; }
                else if (y < 30) { oreType = "Stone"; hardness = 2; value = 1; }
                else if (y < 60) { oreType = "Coal"; hardness = 5; value = 1; }
                else if (y < 80) { oreType = "Copper"; hardness = 7; value = 5; }
                else if (y < 100) { oreType = "Iron"; hardness = 10; value = 10; }
                else if (y < 120) { oreType = "Gold"; hardness = 15; value = 25; }
                else { oreType = "Diamond"; hardness = 20; value = 100; }

                Rectangle source = oreTextures[oreType];

                for (int x = 0; x < Width; x++)
                {
                    Tiles.Add(new MineTile(new Vector2(x * TileSize, (y * TileSize) + 544), source, hardness, value));
                }
            }
        }

        public void LoadFromFile(GameSaveData save)
        {
            Tiles.Clear();

            foreach(var t in save.Tiles)
            {
                MineTile tile = new MineTile(t.Position, t.Source, t.RemainingHealth, t.Value) { IsMined = t.IsMined };
                Tiles.Add(tile);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Camera camera)
        {
            foreach (var tile in Tiles)
            {
                if (tile.Position.Y > camera.Position.Y + 512 && tile.Position.Y < camera.Position.Y + 1532)
                {
                    tile.Draw(spriteBatch, texture);
                }
            }
        }

        public void TryMineTile(Vector2 mouseWorld, Player player)
        {
            foreach (var tile in Tiles)
            {
                Rectangle rect = new((int)tile.Position.X, (int)tile.Position.Y, TileSize, TileSize);
                if (rect.Contains(mouseWorld) && tile.IsMined == false)
                {
                    tile.RemainingHealth -= player.ClickPower;
                    if (tile.RemainingHealth <= 0)
                    {
                        tile.IsMined = true;
                        player.Money += tile.Value;
                    }
                    break;
                }
            }
        }
    }
}
