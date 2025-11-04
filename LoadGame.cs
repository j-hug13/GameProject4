using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject4
{
    public class LoadGame
    {
        private MainController game;
        private SpriteFont spriteFont;

        private MouseState currentMouse;
        private MouseState previousMouse;

        private Texture2D backgroundTexture;
        private Texture2D titleBanner;
        private Texture2D newButton;
        private Texture2D loadButton;

        private Rectangle newButtonBounds;
        private Rectangle loadButtonBounds;

        public void LoadContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("MenuScreen/menuBackground");

            titleBanner = content.Load<Texture2D>("MenuScreen/titleBaner");
            newButton = content.Load<Texture2D>("MenuScreen/button");
            newButtonBounds = new Rectangle(346, 425, newButton.Width, newButton.Height);
            loadButton = content.Load<Texture2D>("MenuScreen/button");
            loadButtonBounds = new Rectangle(346, 650, loadButton.Width, loadButton.Height);

            spriteFont = content.Load<SpriteFont>("bangers");
        }

        public GameState? Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && newButtonBounds.Contains(currentMouse.Position))
            {
                return GameState.PlayingNew;
            }
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && loadButtonBounds.Contains(currentMouse.Position))
            {
                return GameState.PlayingLoad;
            }
            else
            {
                return null;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, graphics.Viewport.Bounds, Color.White);

            spriteBatch.Draw(titleBanner, new Vector2(46, 50), Color.White);
            spriteBatch.Draw(newButton, new Vector2(346, 425), Color.White);
            spriteBatch.Draw(loadButton, new Vector2(346, 650), Color.White);

            Color newButtonColor;
            if (newButtonBounds.Contains(currentMouse.Position))
            {
                newButtonColor = Color.Gold;
            }
            else
            {
                newButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "New", new Vector2(460, 460), newButtonColor);

            Color loadButtonColor;
            if (loadButtonBounds.Contains(currentMouse.Position))
            {
                loadButtonColor = Color.Gold;
            }
            else
            {
                loadButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "Load", new Vector2(450, 685), loadButtonColor);

            spriteBatch.End();
        }
    }
}
