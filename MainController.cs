using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject4
{
    public enum GameState
    {
        Menu,
        LoadGame,
        PlayingNew,
        PlayingLoad,
        Settings,
        Quit
    }

    public class MainController : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameState currentState = GameState.Menu;

        private MenuScreen menuScreen;
        private LoadGame loadGame;
        private GameScene gameScene;

        public static int ScreenWidth = 992;
        public static int ScreenHeight = 1500;

        private bool ignoreNextClick = false;
        private bool gameInitialized = false;

        public MainController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuScreen = new MenuScreen();
            menuScreen.LoadContent(Content);

            loadGame = new LoadGame();
            loadGame.LoadContent(Content);

            gameScene = new GameScene();
            gameScene.LoadContent(this, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.Menu:
                    GameState? state = menuScreen.Update(gameTime);
                    if (state == GameState.LoadGame)
                    {
                        currentState = GameState.LoadGame;
                        ignoreNextClick = true;
                    }
                    break;

                case GameState.LoadGame:
                    if(ignoreNextClick == true)
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Released)
                        {
                            ignoreNextClick = false;
                        }
                    }
                    else
                    {
                        GameState? loadState = loadGame.Update(gameTime);
                        if(loadState != null)
                        {
                            currentState = loadState.Value;
                        }
                    }
                    break;

                case GameState.PlayingNew:
                    if(gameInitialized == false)
                    {
                        gameScene.CreateNewGame();
                        gameInitialized = true;
                    }
                    gameScene.Update(gameTime);
                    break;

                case GameState.PlayingLoad:
                    if (gameInitialized == false)
                    {
                        gameScene.LoadExistingGame();
                        gameInitialized = true;
                    }
                    gameScene.Update(gameTime);
                    break;

                case GameState.Quit:
                    Exit();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (currentState)
            {
                case GameState.Menu:
                    menuScreen.Draw(gameTime, spriteBatch, GraphicsDevice);
                    break;

                case GameState.LoadGame:
                    loadGame.Draw(gameTime, spriteBatch, GraphicsDevice);
                    break;

                case GameState.PlayingNew:
                    gameScene.Draw(gameTime, spriteBatch, GraphicsDevice);
                    break;

                case GameState.PlayingLoad:
                    gameScene.Draw(gameTime, spriteBatch, GraphicsDevice);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
