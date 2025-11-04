using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameProject4
{
    public class GameSaveData
    {
        public int PlayerMoney { get; set; }
        public int PlayerClickPower {  get; set; }
        public List<MineTileSaveData> Tiles { get; set; } = new List<MineTileSaveData>();
    }

    public class MineTileSaveData
    {
        public float X { get; set; }
        public float Y { get; set; }

        public int SourceX { get; set; }
        public int SourceY { get; set; }

        public bool IsMined { get; set; }
        public int RemainingHealth { get; set; }
        public int Value { get; set; }
        public string OreType { get; set; }

        public Vector2 Position => new Vector2(X, Y);
        public Rectangle Source => new Rectangle(SourceX, SourceY, 32, 32);
    }
}
