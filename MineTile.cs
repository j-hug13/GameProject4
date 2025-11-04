using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameProject4
{
    public class MineTile
    {
        public string OreType;
        public bool IsMined;
        public Vector2 Position;
        public int RemainingHealth;
        public int Value;
        public Rectangle Source;

        public MineTile(Vector2 position, Rectangle sourceRect, int health, int value)
        {
            Source = sourceRect;
            RemainingHealth = health;
            Position = position;
            Value = value;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if(IsMined == true)
            {
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), new Rectangle(0, 0, 32, 32), Color.Black * 0.5f);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), Source, Color.White);
            }
        }
    }
}
