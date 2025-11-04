using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameProject4
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0);
        }
    }
}
