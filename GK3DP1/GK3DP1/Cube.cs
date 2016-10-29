using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GK3DP1
{
    public class Cube
    {
        public float Width;
        public float Height;
        public float Depth;

        public Vector3 Position ;

        // public float  = 40;

       
        public VertexPositionNormalTexture[] vertexes;

        public Cube(float x, float y, float z)
        {
            Width = x;
            Height = y;
            Depth = z;
            Position = new Vector3(0.0f, 0.0f, 0.0f);
        }
        public Cube(float x, float y, float z, Vector3 startingPosition)
        {
            Width = x;
            Height = y;
            Depth = z;
            Position = startingPosition;
        }


        public VertexPositionNormalTexture[]
                                   MakeCube()
        {
            vertexes = new VertexPositionNormalTexture[36];

            //top face
            Vector3 topLeftFront = Position + new Vector3(-Width, 1.0f, -Depth);
            Vector3 topLeftBack = Position + new Vector3(-Width, 1.0f, Depth);
            Vector3 topRightFront = Position + new Vector3(Width, 1.0f, -Depth);
            Vector3 topRightBack = Position + new Vector3(Width, 1.0f, Depth);

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = Position + new Vector3(-Width, -Height, -Depth);
            Vector3 btmLeftBack = Position + new Vector3(-Width, -Height, Depth);
            Vector3 btmRightFront = Position + new Vector3(Width, -Height, -Depth);
            Vector3 btmRightBack = Position +  new Vector3(Width, -Height, Depth);

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f*Height, 0.0f*Width, 1.0f* Depth);
            Vector3 normalBack = new Vector3(0.0f * Height, 0.0f * Width, -1.0f * Depth);
            Vector3 normalTop = new Vector3(0.0f * Height, 1.0f*Width, 0.0f * Depth);
            Vector3 normalBottom = new Vector3(0.0f * Height, -1.0f * Width, 0.0f * Depth);
            Vector3 normalLeft = new Vector3(-1.0f * Height, 0.0f * Width, 0.0f * Depth);
            Vector3 normalRight = new Vector3(1.0f * Height, 0.0f * Width, 0.0f * Depth);

            // UV texture coordinates
            Vector2 textureTopLeft = new Vector2(1.0f*Height, 0.0f*Width);
            Vector2 textureTopRight = new Vector2(0.0f * Height, 0.0f * Width);
            Vector2 textureBottomLeft = new Vector2(1.0f * Height, 1.0f * Width);
            Vector2 textureBottomRight = new Vector2(0.0f * Height, 1.0f * Width);

            // Add the vertices for the FRONT face.
            vertexes[0] = new VertexPositionNormalTexture(topLeftFront, normalFront, textureTopLeft);
            vertexes[1] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            vertexes[2] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);
            vertexes[3] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            vertexes[4] = new VertexPositionNormalTexture(btmRightFront, normalFront, textureBottomRight);
            vertexes[5] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            // Add the vertices for the BACK face.
            vertexes[6] = new VertexPositionNormalTexture(topLeftBack, normalBack, textureTopRight);
            vertexes[7] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            vertexes[8] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            vertexes[9] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            vertexes[10] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            vertexes[11] = new VertexPositionNormalTexture(btmRightBack, normalBack, textureBottomLeft);

            // Add the vertices for the TOP face.
            vertexes[12] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            vertexes[13] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            vertexes[14] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);
            vertexes[15] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            vertexes[16] = new VertexPositionNormalTexture(topRightFront, normalTop, textureBottomRight);
            vertexes[17] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);

            // Add the vertices for the BOTTOM face.
            vertexes[18] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            vertexes[19] = new VertexPositionNormalTexture(btmLeftBack, normalBottom, textureBottomLeft);
            vertexes[20] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            vertexes[21] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            vertexes[22] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            vertexes[23] = new VertexPositionNormalTexture(btmRightFront, normalBottom, textureTopRight);

            // Add the vertices for the LEFT face.
            vertexes[24] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);
            vertexes[25] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            vertexes[26] = new VertexPositionNormalTexture(btmLeftFront, normalLeft, textureBottomRight);
            vertexes[27] = new VertexPositionNormalTexture(topLeftBack, normalLeft, textureTopLeft);
            vertexes[28] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            vertexes[29] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);

            // Add the vertices for the RIGHT face.
            vertexes[30] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            vertexes[31] = new VertexPositionNormalTexture(btmRightFront, normalRight, textureBottomLeft);
            vertexes[32] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);
            vertexes[33] = new VertexPositionNormalTexture(topRightBack, normalRight, textureTopRight);
            vertexes[34] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            vertexes[35] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);

            return vertexes;
        }
    }
}