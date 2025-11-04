using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject4
{
    public class GameScene
    {
        private MainController game;
        private Camera camera;
        private TileMap mine;
        private Player player;
        private SpriteFont spriteFont;
        private Texture2D tileset;
        private Texture2D backgroundTexture;

        private Texture2D saveButton;
        private Rectangle saveButtonBounds;
        private Texture2D plusButton;
        private Rectangle plusButtonBounds;
        private Texture2D minusButton;
        private Rectangle minusButtonBounds;

        private MouseState currentMouse;
        private MouseState previousMouse;

        private string saveFile => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "save.json");

        public void LoadContent(MainController game, ContentManager content)
        {
            this.game = game;
            this.camera = new Camera();
            this.player = new Player();

            tileset = content.Load<Texture2D>("GameScene/Ores");

            spriteFont = content.Load<SpriteFont>("bangers");

            backgroundTexture = content.Load<Texture2D>("GameScene/gameBackground");

            saveButton = content.Load<Texture2D>("MenuScreen/button");
            saveButtonBounds = new Rectangle(546, 325, saveButton.Width, saveButton.Height);
            plusButton = content.Load<Texture2D>("GameScene/smallButton");
            plusButtonBounds = new Rectangle(50, 325, plusButton.Width, plusButton.Height);
            minusButton = content.Load<Texture2D>("GameScene/smallButton");
            minusButtonBounds = new Rectangle(225, 325, minusButton.Width, minusButton.Height);

            mine = new TileMap();
            mine.LoadTextures();
        }

        public void Save()
        {
            var save = mine.ToSaveData(player);
            SaveGame.Save(save);
        }

        public void CreateNewGame()
        {
            mine.GenerateDefaultMine();
            player.Money = 0;
            player.ClickPower = 1;
            Save();
        }

        public void LoadExistingGame()
        {
            if (File.Exists(saveFile))
            {
                var save = SaveGame.Load();
                player.Money = save.PlayerMoney;
                player.ClickPower = save.PlayerClickPower;
                mine.LoadFromFile(save);
            }
            else
            {
                CreateNewGame();
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && saveButtonBounds.Contains(currentMouse.Position))
            {
                Save();
            }
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && plusButtonBounds.Contains(currentMouse.Position))
            {
                player.ClickPower += 1;
            }
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && minusButtonBounds.Contains(currentMouse.Position))
            {
                if(player.ClickPower > 1)
                {
                    player.ClickPower -= 1;
                }
            }

            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && camera.Position.Y < 3200)
            {
                camera.Position.Y += 10;
            }
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && camera.Position.Y > 0)
            {
                camera.Position.Y -= 10;
            }

            if (currentMouse.Y > 544 && (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released))
            {
                var mouseWorld = new Vector2(currentMouse.X, currentMouse.Y + camera.Position.Y);
                mine.TryMineTile(mouseWorld, player);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
            mine.Draw(spriteBatch, tileset, camera);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, graphics.Viewport.Bounds, Color.White);

            spriteBatch.Draw(saveButton, new Vector2(546, 325), Color.White);
            Color saveButtonColor;
            if (saveButtonBounds.Contains(currentMouse.Position))
            {
                saveButtonColor = Color.Gold;
            }
            else
            {
                saveButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "Save", new Vector2(650, 360), saveButtonColor);

            spriteBatch.Draw(plusButton, new Vector2(50, 325), Color.White);
            Color plusButtonColor;
            if (plusButtonBounds.Contains(currentMouse.Position))
            {
                plusButtonColor = Color.Gold;
            }
            else
            {
                plusButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "+", new Vector2(115, 360), plusButtonColor);

            spriteBatch.Draw(minusButton, new Vector2(225, 325), Color.White);
            Color minusButtonColor;
            if (minusButtonBounds.Contains(currentMouse.Position))
            {
                minusButtonColor = Color.Gold;
            }
            else
            {
                minusButtonColor = Color.SaddleBrown;
            }
            spriteBatch.DrawString(spriteFont, "-", new Vector2(285, 360), minusButtonColor);

            spriteBatch.DrawString(spriteFont, $"Money: ${player.Money}", new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(spriteFont, $"Click Power: {player.ClickPower}", new Vector2(20, 100), Color.White);
            spriteBatch.End();
        }
    }
}
