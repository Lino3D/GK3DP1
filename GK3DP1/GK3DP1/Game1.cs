using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GK3DP1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Globals

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        private Model model;

        //Geometric info
        // private VertexBuffer vertexBuffer;

        private VertexPositionNormalTexture[] Station;
        private VertexPositionNormalTexture[] RailWay;
        private VertexPositionNormalTexture[] someCube;
        private VertexPositionNormalTexture[] Bench;

        private Robot robot;
        private Camera camera;
        private Texture2D concreteTexture;
        private Texture2D steelnetTexture;
        private Texture2D steelSeamlessTexture;

        #endregion Globals

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            robot = new Robot();
            robot.Initialize(Content);
            InitializeEffects();
            camera = new Camera(graphics.GraphicsDevice);
            //this.IsMouseVisible = true;
            base.Initialize();
        }

        private void InitializeEffects()
        {
            basicEffect = new BasicEffect(GraphicsDevice);

            //basicEffect.AmbientLightColor = Vector3.One;
            //basicEffect.DirectionalLight0.Enabled = true;
            //basicEffect.DirectionalLight0.DiffuseColor =
            //                            Vector3.One;
            //basicEffect.DirectionalLight0.Direction =
            //         Vector3.Normalize(Vector3.One);
            //basicEffect.LightingEnabled = true;

            //basicEffect.TextureEnabled = true;
            //basicEffect.Texture = stationTexture;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            model = Content.Load<Model>("robot");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var myCube = new Cube(20, 40, 60);
            Station = myCube.MakeCube();

            var mySecondCube = new Cube(5, 5, 60, new Vector3(0.0f, -40.0f, 0.0f));
            RailWay = mySecondCube.MakeCube();

            var someSmallCube = new Cube(4, 2, 2, new Vector3(0, 0, 0));
            someCube = someSmallCube.MakeCube();
            Bench = new Bench(someSmallCube).Vertexes;

            LoadTextures();
        }

        private void LoadTextures()
        {
            concreteTexture = Content.Load<Texture2D>("tex.jpg");
            steelnetTexture = Content.Load<Texture2D>("steelnet.jpg");
            steelSeamlessTexture = Content.Load<Texture2D>("steel.jpg");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawStation();
            DrawBench(new Vector3(15, -25, 17),Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(90)));
            DrawBench(new Vector3(-10, -25, 17),Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(270)));
            robot.Draw(camera, new Vector3(9, 0, 0));

            base.Draw(gameTime);
        }

        private void DrawStation()
        {
            basicEffect.View = camera.ViewMatrix;
            basicEffect.Projection = camera.ProjectionMatrix;
            basicEffect.World = Matrix.CreateTranslation(new Vector3(0.0f,15f,0.0f));
            basicEffect.Texture = concreteTexture;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Station,
                                0, 12);
            }
            basicEffect.Texture = steelSeamlessTexture;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.
                   Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, RailWay,
                               0, 12);
            }
        }

        private void DrawBench(Vector3 position, params Matrix[] matrices )
        {
            basicEffect.View = camera.ViewMatrix;
            basicEffect.Projection = camera.ProjectionMatrix;

            basicEffect.TextureEnabled = true;
            basicEffect.Texture = steelnetTexture;

            basicEffect.World = Matrix.CreateTranslation(position);
            foreach (Matrix m in matrices)
            {
                basicEffect.World *= m;
            }
            

            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Bench,
                                               0, 24);
            }
        }
    }
}