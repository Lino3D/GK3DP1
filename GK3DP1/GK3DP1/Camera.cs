using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GK3DP1
{
    public class Camera
    {
        // We need this to calculate the aspectRatio
        // in the ProjectionMatrix property.
        private GraphicsDevice graphicsDevice;

        private Vector3 cameraPosition = new Vector3(4, 8, 5);
        private Vector3 cameraUp = Vector3.UnitY;
        private Vector3 cameraDirection;

        private float leftrightRot = MathHelper.PiOver2;
        private float updownRot = -MathHelper.Pi / 10.0f;

        private MouseState prevMouseState;

        private const float MoveSpeed = 30f;
        private const float MouseSensitivity = 0.006f;
        public Matrix ViewMatrix { get; set; }

        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            cameraDirection = Vector3.Zero - this.cameraPosition;
            this.ViewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);

            Mouse.SetPosition(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            this.prevMouseState = Mouse.GetState();
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
            Vector3 moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector.Z += -1;
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector.Z += 1;
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector.X += 1;
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector.X += -1;
            if (keyState.IsKeyDown(Keys.Q))
                moveVector.Y += 1;
            if (keyState.IsKeyDown(Keys.Z))
                moveVector.Y += -1;
            AddToCameraPosition(moveVector * MouseSensitivity);

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != prevMouseState)
            {
                float xDifference = currentMouseState.X - prevMouseState.X;
                float yDifference = currentMouseState.Y - prevMouseState.Y;
                leftrightRot -= xDifference * MouseSensitivity;
                updownRot -= yDifference * MouseSensitivity;
                Mouse.SetPosition(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
                UpdateViewMatrix();
            }
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += MoveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -2);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = Vector3.Up;
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            this.ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}