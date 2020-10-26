using Loki2D.Core.Scene;
using Loki2D.Core.Utilities;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TermProject1.World;

namespace TermProject1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // creating new instance of scene
            SceneManagement.Instance = new SceneManagement(graphics.GraphicsDevice, Content);


            // option for deferred lighting 
            // SceneManagement.Instance.CurrentScene.DeferredDraw = true;

            // creating new instance of texture manager
            TextureManager.Instance = new TextureManager(graphics.GraphicsDevice);
            TextureManager.Instance.LoadFolder("Textures/");

            // setting up new scene
            var scene = new Scene("Untitled", new Point(10, 10));
            SceneManagement.Instance.LoadScene(scene);
            SceneManagement.Instance.CurrentScene.Camera.CamControl = true;

            GameManager.Instance = new GameManager();
            Camera.Instance.SetMoveSpeed(10);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.StartCapture();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SceneManagement.Instance.Update(gameTime);
            //SceneManagement.Instance.CurrentScene.Camera.WSADMovement();

            GameManager.Instance.Update(gameTime);

            InputManager.EndCapture();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            SceneManagement.Instance.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
