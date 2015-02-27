using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
         const float cameraSpeed = 10.0f;
         float rotation = 0;
        Texture2D backGround;
        Texture2D belmont;
        Texture2D rain;
        Point belmontSheetSize = new Point(8, 1);
        Point belmontFrameSize = new Point(40, 46);
        Point belmontSheetPosition = new Point(0, 0);
        Point belmontFirstFrame = new Point(0, 0);
        int belmontNumFrames = 8;
        int belmontCurrentFrame = 0;
        int belmontMillisecPerFrame = 100;
        int belmontTimeSinceLastFrame = 0;
        Vector2 cameraPosition = Vector2.Zero;
        float layerRainScroll = 5f;
        float layer0Scroll = 1.0f;
        float layer1Scroll = 0.5f;
        float layer2Scroll = 0.1f;
        float rainSpeed = 500.0f;
        float rainYPos = 0;
        float backGroundScale = 4.0f;
        Point frameSize = new Point(2048, 224);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 896;
            graphics.PreferredBackBufferWidth = 1024;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backGround = Content.Load<Texture2D>(@"Images/background");
            belmont = Content.Load<Texture2D>(@"Images/belmont");
            rain = Content.Load<Texture2D>(@"Images/rain");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            rainYPos -= rainSpeed * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
           
            rotation += 0.1f;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // Move threerings based on keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
                cameraPosition.X -= cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Right))
                cameraPosition.X += cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Up))
                cameraPosition.Y -= cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                cameraPosition.Y += cameraSpeed;
            // TODO: Add your update logic here

            belmontTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (belmontTimeSinceLastFrame > belmontMillisecPerFrame)
            {
                belmontCurrentFrame++;
                belmontTimeSinceLastFrame -= belmontMillisecPerFrame;

                if (belmontCurrentFrame >= belmontNumFrames)
                {
                    belmontSheetPosition = belmontFirstFrame;
                    belmontCurrentFrame = 0;
                }
                else
                {
                    ++belmontSheetPosition.X;
                    if (belmontSheetPosition.X >= belmontSheetSize.X)
                    {
                        belmontSheetSize.X = 0;
                        ++belmontSheetSize.Y;
                    }
                }

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Matrix screenMatrix = Matrix.CreateTranslation(new Vector3(-cameraPosition.X,0, 0));

            BlendState blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.SourceAlpha;
            blendState.AlphaSourceBlend = Blend.SourceAlpha;
            blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            blendState.ColorBlendFunction = BlendFunction.Add;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, 
                BlendState.AlphaBlend, 
                SamplerState.PointWrap, 
                DepthStencilState.None, 
                RasterizerState.CullCounterClockwise,
                null,
                screenMatrix);

                      
            //third layer
           spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0), new Rectangle((int)Math.Round(cameraPosition.X * layer2Scroll / backGroundScale), 448, 1024 / (int)backGroundScale, 224), Color.White, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0.9f);
            //second layer
            spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0), new Rectangle((int)Math.Round(cameraPosition.X * layer1Scroll / backGroundScale), 224, 1024 / (int)backGroundScale, 224), Color.White, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0.8f);
            //first layer
            spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0), 
                new Rectangle((int)Math.Round(cameraPosition.X  / backGroundScale), 
                    0,  1024 / (int)backGroundScale, 224), 
                Color.White, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0.7f);


            //belmont 
            spriteBatch.Draw(belmont, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2) + cameraPosition, new Rectangle(belmontSheetPosition.X * belmontFrameSize.X, belmontSheetPosition.Y * belmontFrameSize.Y, belmontFrameSize.X, belmontFrameSize.Y), Color.White, 0, new Vector2(belmontFrameSize.X / 2, belmontFrameSize.Y / 2), 4.0f, SpriteEffects.None, 0.1f);
            
            
            //rain layer
            spriteBatch.Draw(rain, new Vector2(cameraPosition.X, 0), new Rectangle((int)Math.Round(cameraPosition.X * layerRainScroll / backGroundScale) - (int)rainYPos, (int)rainYPos, 1024, 224), Color.White, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0.0f);
            
            //Rectangle layer0Rectangle = new Rectangle(
            //    (int)Math.Round(cameraPosition.X * layer0Scroll / backGroundScale), 
            //    0, Window.ClientBounds.Width, frameSize.Y);

            //spriteBatch.Draw(backGround, 
            //    new Vector2(cameraPosition.X, 0), 
            //    layer0Rectangle, Color.White, 0.0f,
            //    Vector2.Zero, backGroundScale, 
            //    SpriteEffects.None, 0);
           

            //int positionOnFrame = (int)Math.Round(cameraPosition.X * layer0Scroll *4)%2048; //position on frame in texture space
            //spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0), 
            //    new Rectangle(positionOnFrame, 0, 
            //        Math.Min(frameSize.X-positionOnFrame,1024/(int)backGroundScale), frameSize.Y), 
            //        Color.White, 0.0f, Vector2.Zero, backGroundScale, SpriteEffects.None, 0);
            //spriteBatch.Draw(backGround, 
            //    new Vector2(cameraPosition.X + Math.Min(frameSize.X - positionOnFrame, 1024/(int)backGroundScale) * backGroundScale, 0),
            //    new Rectangle(0, 0, 1024/(int)backGroundScale - Math.Min(frameSize.X - positionOnFrame, 1024/(int)backGroundScale), frameSize.Y), 
            //    Color.White, 0.0f, Vector2.Zero, backGroundScale, SpriteEffects.None, 0);

            

            //spriteBatch.Draw(link,
            //    new Rectangle(0, 0, 100, 500),
            //    new Rectangle(13, 13, 16, 20),
            //    new Color(0.5f, 0.5f, 0.5f, 0.5f));


            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
