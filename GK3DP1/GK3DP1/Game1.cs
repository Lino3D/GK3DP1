
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// ReSharper disable All

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
        private SpriteBatch MenuSprite;

        private BasicEffect BasicEffect;

        private Effect SampleEffect;
        private Effect GaussianBlur;


        private Model Model;
        private RenderTarget2D RenderTarget;
        //Geometric info
        // private VertexBuffer vertexBuffer;

        private VertexPositionNormalTexture[] Station;
        private VertexPositionNormalTexture[] RailWay;
        private VertexPositionNormalTexture[] someCube;
        private VertexPositionNormalTexture[] Bench;
        private VertexPositionNormalTexture[] ProjectionCube;

        private Robot Robot;
        private Robot Robot2;
        private Camera Camera;
        private Texture2D ConcreteTexture;
        private Texture2D SteelnetTexture;
        private Texture2D SteelSeamlessTexture;
        private Texture2D RenderedTexture;
        private Texture2D MenuTexture;

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
        private bool GaussianBlurOn;

        private SpriteBatch renderTargetBatch;
        private Rectangle FirstResolution = new Rectangle(5, 10, 40, 12);
        private Rectangle SecondResolution = new Rectangle(5, 25, 40, 15);
        private Rectangle ThirdResolution = new Rectangle(5, 40, 40, 12);
        private Rectangle MultiSamplingRectange = new Rectangle(5,55,100,12);

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


            RenderTarget = new RenderTarget2D(
               GraphicsDevice,
               GraphicsDevice.PresentationParameters.BackBufferWidth,
               GraphicsDevice.PresentationParameters.BackBufferHeight,
               false,
               GraphicsDevice.PresentationParameters.BackBufferFormat,
               DepthFormat.Depth24);
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


            BasicEffect.FogEnabled = false;
            BasicEffect.FogColor = Color.CornflowerBlue.ToVector3(); // For best results, ake this color whatever your background is.
            BasicEffect.FogStart = 9.75f;
            BasicEffect.FogEnd = 10.25f;


        }


        protected override void LoadContent()
        {
            Model = Content.Load<Model>("robot");
            SampleEffect = Content.Load<Effect>("SampleEffect");
            GaussianBlur = Content.Load<Effect>("GaussianBlur");
          
            renderTargetBatch = new SpriteBatch(GraphicsDevice);

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
            var cube2 = new Cube(1.0f, 1.0f, 1.0f, new Vector3(0, 0, 0));
            ProjectionCube = cube2.MakeCube();

            LoadTextures();
        }

        private void LoadTextures()
        {
            //ConcreteTexture = Content.Load<Texture2D>("tex");
            SteelnetTexture = Content.Load<Texture2D>("steelnet");
            SteelSeamlessTexture = Content.Load<Texture2D>("steel");
            spriteFont = Content.Load<SpriteFont>("spriteFont");
            MenuTexture = Content.Load<Texture2D>("Menu");
            MenuSprite = new SpriteBatch(GraphicsDevice);

        }

  
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        private void SetResolution(int Width, int Height)
        {
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            this.Window.Position = new Point(0, 0);
            Graphics.ApplyChanges();
            Camera = new Camera(Graphics.GraphicsDevice);
        }



        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            MouseState prevMouseState = new MouseState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!MenuModeOn)
            {
                Camera.Update(gameTime);
            }

            if (MenuModeOn)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
        
                    Point mousePos = new Point(mouseState.X, mouseState.Y);
                    if (FirstResolution.Contains(mousePos))
                    {
                        SetResolution(1440,900);
                    }
                    else if(SecondResolution.Contains(mousePos))
                        SetResolution(1920,1080);
                    else if(ThirdResolution.Contains(mousePos))
                        SetResolution(1900,1200);
                    else if (MultiSamplingRectange.Contains(mousePos))
                        MultiSamplingOn = !MultiSamplingOn;
                }
                prevMouseState = mouseState;
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
            if (keyState.IsKeyDown(Keys.B))
            {
                GaussianBlurOn = !GaussianBlurOn;
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

         protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DrawToTexture(gameTime);

            SetSamplerState();
            Graphics.PreferMultiSampling = MultiSamplingOn;

            DrawObjects();

            if(GaussianBlurOn)
            DrawGaussianBlur();

            if(MenuModeOn)
             DrawMenu();
            base.Draw(gameTime);
        }

        private void DrawGaussianBlur()
        {
            renderTargetBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone, GaussianBlur);

            renderTargetBatch.Draw(RenderTarget, destinationRectangle: new Rectangle(0, 0, 400, 400), color: Color.White);
            renderTargetBatch.End();
        }

        protected void DrawToTexture(GameTime gameTime)
        {
            
            GraphicsDevice.SetRenderTarget(RenderTarget);
            //GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
          
            //   GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawObjects();
            RenderedTexture = RenderTarget;
            GraphicsDevice.SetRenderTarget(null);
        }


        private void DrawObjects()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawStationWithEffect();
            Robot.DrawWithEffect(Camera, new Vector3(15, -22, -35), SampleEffect);
            Robot.DrawWithEffect(Camera, new Vector3(-10, -22, 20), SampleEffect);

            //  Robot.Draw(Camera, new Vector3(15, -22, -35));
            //   Robot.Draw(Camera, new Vector3(-10, -22, 20));

            DrawBenchSampleEffect(new Vector3(-10, -24, 58.5f));
            DrawBenchSampleEffect(new Vector3(10, -24, 58.5f));
            DrawCubeSampleEffect(new Vector3(2, -10, -20.5f));
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
        private void DrawMenu()
        {
      
            MenuSprite.Begin();
            MenuSprite.Draw(MenuTexture, new Vector2(-2, -47), Color.White * 0.5f);
            MenuSprite.DrawString(spriteFont, "1440x900", new Vector2(5, 10), Color.White);
            MenuSprite.DrawString(spriteFont, "1920x1080", new Vector2(5, 25), Color.White);
            MenuSprite.DrawString(spriteFont, "1900x1200", new Vector2(5, 40), Color.White);
            MenuSprite.DrawString(spriteFont, "MultiSampling: " + MultiSamplingOn.ToString(), new Vector2(5, 55), Color.White);

            MenuSprite.End();
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
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

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
   
            BasicEffect.Texture = SteelSeamlessTexture;
            BasicEffect.World = world;
            BasicEffect.View = Camera.ViewMatrix;
            BasicEffect.Projection = Camera.ProjectionMatrix;
            if (RenderedTexture != null)
            {
                SampleEffect.Parameters["BasicTexture"].SetValue(RenderedTexture);
                BasicEffect.Texture = RenderedTexture;
            }
            else
            {
                BasicEffect.Texture = SteelSeamlessTexture;
            }
          

            WorldParemeter.SetValue(world);
            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, ProjectionCube,
                                               0, 12);
            }
        }
    }
}