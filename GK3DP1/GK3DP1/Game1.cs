
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

        private Vector3[] LightPosition = new Vector3[4];
        private Vector3[] LightColor = new Vector3 [4];

        private Vector3[] LightDirectionSpot = new Vector3[4];


        private float timer = 2;         //Initialize a 10 second timer
        private const float TIMER = 2;

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

            LightPosition[0] = new Vector3(10, 10, -25);
            LightPosition[1] = new Vector3(8, 10, 25);
            LightPosition[2] = new Vector3(45, -5, 0);

            LightPosition[3] = new Vector3(-45, 0, -15);

            LightColor[0] = new Vector3(1f, 1, 1f);
            LightColor[1] = new Vector3(0f, 1, 0f);
            LightColor[2] = new Vector3(0f, 0, 1f);

            LightColor[3] = new Vector3(1.0f, 0f,0);

        

            LightDirectionSpot[0] = new Vector3(0, -1f, 0);
            LightDirectionSpot[1] = new Vector3(0, -1f, 0);

            LightDirectionSpot[2] = new Vector3(0, -1f, 0);
            LightDirectionSpot[3] = new Vector3(0,  -1f, 0);
            base.Initialize();
        }

        private void InitializeEffects()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            BasicEffect.TextureEnabled = true;
            BasicEffect.EnableDefaultLighting();
            BasicEffect.LightingEnabled = true; // turn on the lighting subsystem.
            BasicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
            BasicEffect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
            BasicEffect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Model = Content.Load<Model>("robot");
            SampleEffect = Content.Load<Effect>("SampleEffect");

            SetShaderParameters();

            CreateCubes();
        }

        private void SetShaderParameters()
        {
            ProjectionParameter = SampleEffect.Parameters["Projection"];
            WorldParemeter = SampleEffect.Parameters["World"];
            ViewParameter = SampleEffect.Parameters["View"];
            TextureParameter = SampleEffect.Parameters["BasicTexture"];

            SampleEffect.Parameters["LightColor"].SetValue(LightColor);
            SampleEffect.Parameters["LightPosition"].SetValue(LightPosition);

            SampleEffect.Parameters["LightDirectionSpot"].SetValue(LightDirectionSpot);
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

            UpdateLight(gameTime);

            base.Update(gameTime);
        }

        private void UpdateLight(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                //Timer expired, execute action
                   //Reset Timer
                if (LightColor[1].Z == 1)
                {
                    LightColor[1] = new Vector3(0f, 1, 0f);
                    SampleEffect.Parameters["LightColor"].SetValue(LightColor);
                }
                else
                {
                    LightColor[1] = new Vector3(0, 0, 1);
                    SampleEffect.Parameters["LightColor"].SetValue(LightColor);
                }
                timer = TIMER;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawStationWithEffect();

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
            //BasicEffect.Texture = SteelSeamlessTexture;

            //BasicEffect.World = world;
            //BasicEffect.View = Camera.ViewMatrix;
            //BasicEffect.Projection = Camera.ProjectionMatrix;
            //foreach (EffectPass pass in BasicEffect.CurrentTechnique.
            //        Passes)
            //{
            //    pass.Apply();
            //    Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Station,
            //                    0, 12);
            //}
      

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