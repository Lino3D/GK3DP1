using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
namespace GK3DP1
{
    public class Robot
    {
        private Model model;
        private Texture2D texture;

        // new code:
        private float angle;

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("robot");
            texture = contentManager.Load<Texture2D>("robottexture_0");
        }

        public void Update(GameTime gameTime)
        {
            // TotalSeconds is a double so we need to cast to float
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Camera camera, Vector3 position)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (var effect1 in mesh.Effects.Where(x => x.GetType()==typeof(BasicEffect)))
                {
                    var effect = (BasicEffect) effect1;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    var worldMatrix = Matrix.CreateTranslation(position);
                    worldMatrix *= mesh.ParentBone.Transform;

                    effect.World = worldMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }
        public void DrawWithEffect(Camera camera, Vector3 position, Effect effect)
        {
            foreach (var mesh in model.Meshes)
            {
                  
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                      
                        Matrix world = Matrix.CreateTranslation(position);
                        //Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                        //effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                        effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * world);
                        effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        effect.Parameters["BasicTexture"].SetValue(texture);
                     //effect.Parameters["BasicTexture"].SetValue(part.Effect.te);
                }
                    mesh.Draw();
                
              
            }
        }
        public void DrawGlassRobot(Camera camera, Vector3 position, Effect effect)
        {
            foreach (var mesh in model.Meshes)
            {

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix world = Matrix.CreateTranslation(position);
                    effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * world);
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
  
                }
                mesh.Draw();


            }
        }


    }
}