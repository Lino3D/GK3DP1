
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

        private SpriteFont spriteFont;



        private Vector3[] LightPosition = new Vector3[4];
        private Vector3[] LightColor = new Vector3 [4];

        private Vector3[] LightDirectionSpot = new Vector3[4];


        private float timer = 2;         //Initialize a 10 second timer
        private const float TIMER = 2;
        private bool MenuModeOn = false;
        private bool UseBasicEffectOn = false;

        float MipMapDepthLevels = 0.0f;
        bool MagFilterOn = false;
        bool MipMapFilterOn = false;
        private bool MultiSamplingOn;

        #endregion Globals

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
           // Graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            Robot = new Robot();
            Robot.Initialize(Content);
            InitializeEffects();
            Camera = new Camera(Graphics.GraphicsDevice);
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            SetSamplerState();

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


            BasicEffect.FogEnabled = true;
            BasicEffect.FogColor = Color.CornflowerBlue.ToVector3(); // For best results, ake this color whatever your background is.
            BasicEffect.FogStart = 9.75f;
            BasicEffect.FogEnd = 10.25f;


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
            //ConcreteTexture = Content.Load<Texture2D>("tex");
            SteelnetTexture = Content.Load<Texture2D>("steelnet");
            SteelSeamlessTexture = Content.Load<Texture2D>("steel");
            spriteFont = Content.Load<SpriteFont>("spriteFont");
        }

  
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!MenuModeOn)
            {
                Camera.Update(gameTime);
            }
            if (keyState.IsKeyDown(Keys.G))
            {
                Graphics.PreferredBackBufferWidth = 1280;
                Graphics.PreferredBackBufferHeight = 720;
                this.Window.Position = new Point(0, 0);
                Graphics.ApplyChanges();
                Camera = new Camera(Graphics.GraphicsDevice);
            }
            if (keyState.IsKeyDown(Keys.K))
            {
                Graphics.PreferredBackBufferHeight = 1680;
                Graphics.PreferredBackBufferWidth = 1050;
                Graphics.ApplyChanges();
                Camera = new Camera(Graphics.GraphicsDevice);
            }

            if (keyState.IsKeyDown(Keys.P))
            {
                UseBasicEffectOn = !UseBasicEffectOn;
            }

            if (keyState.IsKeyDown(Keys.M))
            {
                MultiSamplingOn = !MultiSamplingOn;

            }

            if (!keyState.IsKeyDown(Keys.OemMinus))
                MipMapDepthLevels -= 0.03f;

            if (!keyState.IsKeyDown(Keys.OemPlus))
                MipMapDepthLevels += 0.03f;


            if (keyState.IsKeyDown(Keys.B) )
            {
                MagFilterOn = !MagFilterOn;

            }


            if (Keyboard.GetState().IsKeyDown(Keys.N) )
            {
                MipMapFilterOn = !MipMapFilterOn;

            }



            EnterMenu(keyState);
            UpdateLight(gameTime);

            base.Update(gameTime);
        }

        private void EnterMenu(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.X))
            {
                if (MenuModeOn == false)
                {
                    MenuModeOn = true;
                    this.IsMouseVisible = true;
                }
                else
                {
                    MenuModeOn = false;
                    this.IsMouseVisible = false;
                }

            }
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
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            DrawStationWithEffect();
            SetSamplerState();
            Graphics.PreferMultiSampling = MultiSamplingOn;
           
            Robot.DrawWithEffect(Camera, new Vector3(15, -22, -35), SampleEffect);
           Robot.DrawWithEffect(Camera, new Vector3(-10, -22, 20), SampleEffect);

          //  Robot.Draw(Camera, new Vector3(15, -22, -35));
         //   Robot.Draw(Camera, new Vector3(-10, -22, 20));

            DrawBenchSampleEffect(new Vector3(-10, -24, 58.5f));
           DrawBenchSampleEffect(new Vector3(10, -24, 58.5f));
            DrawCubeSampleEffect(new Vector3(5, -18, -40.5f));

            //  DrawHud();
            base.Draw(gameTime);
        }


        private void DrawHud()
        {
            SpriteBatch.Begin();
           // SpriteBatch.DrawString(spriteFont, "camPos:" + Camera.cameraPosition, new Vector2(10, 10), Color.White);
            SpriteBatch.Draw(ConcreteTexture, Vector2.Zero, Color.White);
            SpriteBatch.End();
            //HudTexture is a transparent texture the size of the window/screen, with hud drawn onto it.
          //  SpriteBatch.Begin();
          
         //   SpriteBatch.End();
            //Let's say that it works well to draw your score at [100,100].

            //   SpriteBatch.DrawString(SpriteFont, "100", new Vector2(100, 100), Color.White);
        }
        private void SetSamplerState()
        {
            // https://github.com/labnation/MonoGame/blob/master/Tools/2MGFX/SamplerStateInfo.cs

            var ss = new SamplerState();

            ss.Filter = TextureFilter.Point;
            ss.MaxMipLevel = 0;
            ss.AddressU = TextureAddressMode.Wrap;
            ss.AddressV = TextureAddressMode.Wrap;
            ss.MipMapLevelOfDetailBias = MipMapDepthLevels;
            if (MagFilterOn && MipMapFilterOn)
                ss.Filter = TextureFilter.Linear;
            else if (!MagFilterOn && MipMapFilterOn)
                ss.Filter = TextureFilter.PointMipLinear;
            else if (MagFilterOn && !MipMapFilterOn)
                ss.Filter = TextureFilter.LinearMipPoint;
            else if (!MagFilterOn && !MipMapFilterOn)
                ss.Filter = TextureFilter.Point;
            GraphicsDevice.SamplerStates[0] = ss;

        }
        private void DrawStationWithEffect()
        {
           

           
            Matrix world = Matrix.CreateTranslation(new Vector3(0.0f, 15f, 0.0f));

            

            //Parametry basic effectu
            BasicEffect.Texture = SteelSeamlessTexture;
            BasicEffect.World = world;
            BasicEffect.View = Camera.ViewMatrix;
            BasicEffect.Projection = Camera.ProjectionMatrix;


            //Parametry Custom Effectu
            ViewParameter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            SampleEffect.Parameters["BasicTexture"].SetValue(SteelSeamlessTexture);

            WorldParemeter.SetValue(world);

            Effect effect = BasicEffect;
            if(!UseBasicEffectOn)
            effect = SampleEffect;


            foreach (EffectPass pass in effect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Station,
                                0, 12);
            }

            foreach (EffectPass pass in effect.CurrentTechnique.
                  Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, RailWay,
                               0, 12);
            }





        }

        private void HandleFog(BasicEffect effect)
        {
           
        }



        private void DrawBenchSampleEffect(Vector3 position, params Matrix[] matrices)
        {
            Matrix world = Matrix.CreateTranslation(position);

            BasicEffect.Texture = SteelSeamlessTexture;
            BasicEffect.World = world;
            BasicEffect.View = Camera.ViewMatrix;
            BasicEffect.Projection = Camera.ProjectionMatrix;

            ViewParameter.SetValue(Camera.ViewMatrix);
            ProjectionParameter.SetValue(Camera.ProjectionMatrix);
            

            TextureParameter.SetValue(SteelSeamlessTexture);
            WorldParemeter.SetValue( world);

            Effect effect = BasicEffect;
            if (!UseBasicEffectOn)
                effect = SampleEffect;

            foreach (var pass in effect.CurrentTechnique.Passes)
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