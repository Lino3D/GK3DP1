using GK3DP1.Objects;
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
        private EffectParameter ViewParamter;


        private TestCube cubeTest;


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
            Robot2 = new Robot();
            Robot2.Initialize(Content);
            InitializeEffects();
            Camera = new Camera(Graphics.GraphicsDevice);
            //this.IsMouseVisible = true;
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
            ViewParamter = SampleEffect.Parameters["View"];

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

            var someSmallCube = new Cube(4, 2, 2, new Vector3(0, 0, 0));
            someCube = someSmallCube.MakeCube();
            Bench = new Bench(someSmallCube).Vertexes;

            cubeTest = new TestCube(new Vector3(0, -5, 2), new Vector3(3, 3, 3));
            cubeTest.ConstructCube();
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
            // Robot2.Draw(Camera, new Vector3(14, -15, 0));
            // Robot2.Draw(Camera, new Vector3(10, -20, 0));
            Robot.DrawWithEffect(Camera, new Vector3(0, -5, 0), SampleEffect);
           // DrawStationWithEffect();
            // DrawBenchSampleEffect(new Vector3(0, -10, 5));
            DrawCubeSampleEffect(new Vector3(10, -5, 0));

            base.Draw(gameTime);
        }

        private void DrawStation()
        {
            BasicEffect.View = Camera.ViewMatrix;
            BasicEffect.Projection = Camera.ProjectionMatrix;
            BasicEffect.World = Matrix.CreateTranslation(new Vector3(0.0f,15f,0.0f));
            BasicEffect.Texture = ConcreteTexture;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            foreach (EffectPass pass in BasicEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Station,
                                0, 12);
            }
            BasicEffect.Texture = SteelSeamlessTexture;

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.
                   Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, RailWay,
                               0, 12);
            }
        }
        private void DrawStationWithEffect()
        {
            BasicEffect.View = Camera.ViewMatrix;
            BasicEffect.Projection = Camera.ProjectionMatrix;
            BasicEffect.World = Matrix.CreateTranslation(new Vector3(0.0f, 15f, 0.0f));
            BasicEffect.Texture = ConcreteTexture;




            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            SampleEffect.Parameters["BasicTexture"].SetValue(SteelSeamlessTexture);
            ViewParamter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            Matrix world = Matrix.CreateTranslation(new Vector3(0.0f, 15f, 0.0f));

            //Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            //SampleEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
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




        private void DrawBench(Vector3 position, params Matrix[] matrices )
        {
            BasicEffect.View = Camera.ViewMatrix;
            BasicEffect.Projection = Camera.ProjectionMatrix;

            //BasicEffect.TextureEnabled = true;
            BasicEffect.Texture = SteelnetTexture;

            BasicEffect.World = Matrix.CreateTranslation(position);
            foreach (Matrix m in matrices)
            {
                BasicEffect.World *= m;
            }
            

            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Bench,
                                               0, 24);
            }
        }
        private void DrawBenchSampleEffect(Vector3 position, params Matrix[] matrices)
        {
            ViewParamter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            Matrix world = Matrix.CreateTranslation(position);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            SampleEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

         //   SampleEffect.Parameters["BasicTexture"].SetValue(SteelSeamlessTexture);

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
            ViewParamter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            Matrix world = Matrix.CreateTranslation(position);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            SampleEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

            //   SampleEffect.Parameters["BasicTexture"].SetValue(SteelSeamlessTexture);

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