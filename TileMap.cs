using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

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

            Random r = new Random();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    string oreType;
                    if (y < 1)
                    {
                        oreType = "Dirt";
                    }
                    else
                    {
                        oreType = GetRandomOre(y, r);
                    }
                    
                    Rectangle source = oreTextures[oreType];
                    int hardness = GetHardness(oreType);
                    int value = GetValue(oreType);

                    Tiles.Add(new MineTile(new Vector2(x * TileSize, (y * TileSize) + 544), source, hardness, value));
                }
            }
        }

        private string GetRandomOre(int depth, Random r)
        {
            double odds = r.NextDouble();

            if(depth < 10)
            {
                if(odds < 0.98)
                {
                    return "Stone";
                }
                else
                {
                    return "Coal";
                }
            }
            else if (depth < 30)
            {
                if (odds < 0.85)
                {
                    return "Stone";
                }
                else if (odds < 0.97)
                {
                    return "Coal";
                }
                else
                {
                    return "Copper";
                }
            }
            else if (depth < 60)
            {
                if (odds < 0.75)
                {
                    return "Stone";
                }
                else if (odds < 0.9)
                {
                    return "Coal";
                }
                else if (odds < 0.98)
                {
                    return "Copper";
                }
                else
                {
                    return "Iron";
                }
            }
            else if (depth < 90)
            {
                if (odds < 0.65)
                {
                    return "Stone";
                }
                else if (odds < 0.83)
                {
                    return "Coal";
                }
                else if (odds < 0.91)
                {
                    return "Iron";
                }
                else
                {
                    return "Gold";
                }
            }
            else
            {
                if (odds < 0.6)
                {
                    return "Stone";
                }
                else if (odds < 0.77)
                {
                    return "Iron";
                }
                else if (odds < 0.9)
                {
                    return "Gold";
                }
                else
                {
                    return "Diamond";
                }
            }
        }

        private int GetHardness(string oreType)
        {
            int hardness = oreType switch
            {
                "Stone" => 2,
                "Coal" => 5,
                "Copper" => 7,
                "Iron" => 10,
                "Gold" => 15,
                "Diamond" => 20,
                _ => 1
            };
            return hardness;
        }

        private int GetValue(string oreType)
        {
            int value = oreType switch
            {
                "Stone" => 1,
                "Coal" => 3,
                "Copper" => 8,
                "Iron" => 15,
                "Gold" => 30,
                "Diamond" => 100,
                _ => 0
            };
            return value;
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
