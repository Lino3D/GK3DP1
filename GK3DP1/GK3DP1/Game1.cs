
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

        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private BasicEffect BasicEffect;

        private Effect SampleEffect;

        private Model Model;

        //Geometric info
        // private VertexBuffer vertexBuffer;

        private VertexPositionNormalTexture[] Station;
        private VertexPositionNormalTexture[] RailWay;
        private VertexPositionNormalTexture[] someCube;
        private VertexPositionNormalTexture[] Bench;

        private Robot Robot;
        private Robot Robot2;
        private Camera Camera;
        private Texture2D ConcreteTexture;
        private Texture2D SteelnetTexture;
        private Texture2D SteelSeamlessTexture;

        private EffectParameter WorldParemeter;
        private EffectParameter ProjectionParameter;
        private EffectParameter ViewParameter;
        private EffectParameter TextureParameter;

        private Vector3[] LightPosition = new Vector3[2];
        private Vector3[] LightColor = new Vector3 [2];
    



        #endregion Globals

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Robot = new Robot();
            Robot.Initialize(Content);
            InitializeEffects();
            Camera = new Camera(Graphics.GraphicsDevice);

            LightPosition[0] = new Vector3(5, -10, -25);
            LightPosition[1] = new Vector3(5, -10, 25);

            LightColor[0] = new Vector3(0f, 0, 1f);
            LightColor[1] = new Vector3(1f, 0, 0f);


           


            base.Initialize();
        }

        private void InitializeEffects()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            BasicEffect.TextureEnabled = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Model = Content.Load<Model>("robot");
            SampleEffect = Content.Load<Effect>("SampleEffect");

            ProjectionParameter = SampleEffect.Parameters["Projection"];
            WorldParemeter = SampleEffect.Parameters["World"];
            ViewParameter = SampleEffect.Parameters["View"];
            TextureParameter = SampleEffect.Parameters["BasicTexture"];

            SampleEffect.Parameters["LightColor"].SetValue(LightColor);
            SampleEffect.Parameters["LightPosition"].SetValue(LightPosition);
            CreateCubes();
        }

        private void CreateCubes()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            var stationCube = new Cube(20, 40, 60);
            stationCube.isStation = true;
            Station = stationCube.MakeCube();

            var railwayCube = new Cube(5, 5, 60, new Vector3(0.0f, -40.0f, 0.0f));
            RailWay = railwayCube.MakeCube();

            var someSmallCube = new Cube(3, 0.5f,1, new Vector3(0, 0, 0));
            someCube = someSmallCube.MakeCube();
            Bench = new Bench(someSmallCube).Vertexes;

            LoadTextures();
        }

        private void LoadTextures()
        {
            ConcreteTexture = Content.Load<Texture2D>("tex");
            SteelnetTexture = Content.Load<Texture2D>("steelnet");
            SteelSeamlessTexture = Content.Load<Texture2D>("steel");
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
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawStationWithEffect();
            // DrawBench(new Vector3(15, -25, 17),Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(90)));
            //  DrawBench(new Vector3(-10, -25, 17),Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(270)));
            Robot.DrawWithEffect(Camera, new Vector3(15, -22, -35), SampleEffect);
            Robot.DrawWithEffect(Camera, new Vector3(-10, -22, 20), SampleEffect);
            // DrawStationWithEffect();
            DrawBenchSampleEffect(new Vector3(-10, -24, 58.5f));
            DrawBenchSampleEffect(new Vector3(10, -24, 58.5f));

            //DrawCubeSampleEffect(new Vector3(10, -10, 0));

            base.Draw(gameTime);
        }


        private void DrawStationWithEffect()
        {
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            SampleEffect.Parameters["BasicTexture"].SetValue(SteelSeamlessTexture);

            ViewParameter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            Matrix world = Matrix.CreateTranslation(new Vector3(0.0f, 15f, 0.0f));

            WorldParemeter.SetValue(world);
            foreach (EffectPass pass in SampleEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Station,
                                0, 12);
            }
            BasicEffect.Texture = SteelSeamlessTexture;

            foreach (EffectPass pass in SampleEffect.CurrentTechnique.
                   Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, RailWay,
                               0, 12);
            }
        }




    
        private void DrawBenchSampleEffect(Vector3 position, params Matrix[] matrices)
        {
            ViewParameter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            Matrix world = Matrix.CreateTranslation(position);

            TextureParameter.SetValue(SteelSeamlessTexture);
            WorldParemeter.SetValue( world);
            foreach (var pass in SampleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Bench,
                                               0, 24);
            }
        }
        private void DrawCubeSampleEffect(Vector3 position, params Matrix[] matrices)
        {
            ViewParameter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            Matrix world = Matrix.CreateTranslation(position);

              SampleEffect.Parameters["BasicTexture"].SetValue(SteelSeamlessTexture);

            WorldParemeter.SetValue(world);
            foreach (var pass in SampleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, someCube,
                                               0, 12);
            }
        }
    }
}