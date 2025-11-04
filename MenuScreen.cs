using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameProject4
{
    public class MenuScreen
    {
        private Texture2D backgroundTexture;
        private SpriteFont spriteFont;
        private Texture2D titleBanner;
        private Texture2D startButton;
        private Texture2D settingsButton;
        private Texture2D quitButton;

        private MouseState currentMouse;
        private MouseState previousMouse;

        private Rectangle startButtonBounds;
        private Rectangle settingsButtonBounds;
        private Rectangle quitButtonBounds;

        /// <summary>
        /// Loads all content required for the menu screen.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("MenuScreen/menuBackground");

            titleBanner = content.Load<Texture2D>("MenuScreen/titleBaner");
            startButton = content.Load<Texture2D>("MenuScreen/button");
            startButtonBounds = new Rectangle(346, 425, startButton.Width, startButton.Height);
            settingsButton = content.Load<Texture2D>("MenuScreen/button");
            settingsButtonBounds = new Rectangle(346, 650, settingsButton.Width, settingsButton.Height);
            quitButton = content.Load<Texture2D>("MenuScreen/button");
            quitButtonBounds = new Rectangle(346, 875, quitButton.Width, quitButton.Height);

            spriteFont = content.Load<SpriteFont>("bangers");
        }

        /// <summary>
        /// Updates menu logic. Returns true if player presses Enter to start the game.
        /// </summary>
        public GameState? Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            if(currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && quitButtonBounds.Contains(currentMouse.Position))
            {
                return GameState.Quit;
            }
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && startButtonBounds.Contains(currentMouse.Position)) 
            {
                return GameState.LoadGame;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Draws the menu screen.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, graphics.Viewport.Bounds, Color.White);

            spriteBatch.Draw(titleBanner, new Vector2(46, 50), Color.White);
            spriteBatch.Draw(startButton, new Vector2(346, 425), Color.White);
            spriteBatch.Draw(settingsButton, new Vector2(346, 650), Color.White);
            spriteBatch.Draw(quitButton, new Vector2(346, 875), Color.White);

            Color startButtonColor;
            if (startButtonBounds.Contains(currentMouse.Position))
            {
                startButtonColor = Color.Gold;
            }
            else
            {
                startButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "Start", new Vector2(436, 460), startButtonColor);

            Color settingsButtonColor;
            if (settingsButtonBounds.Contains(currentMouse.Position))
            {
                settingsButtonColor = Color.Gold;
            }
            else
            {
                settingsButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "Settings", new Vector2(406, 685), settingsButtonColor);

            Color quitButtonColor;
            if (quitButtonBounds.Contains(currentMouse.Position))
            {
                quitButtonColor = Color.Gold;
            }
            else
            {
                quitButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "Quit", new Vector2(456, 910), quitButtonColor);

            spriteBatch.End();
        }
    }
}
