using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GK3DP1
{
    public class Cube
    {
        private float width;
        private float height;
        private float depth;
        private Vector3 position;
        private VertexPositionNormalTexture[] vertexes;

        #region Properties

        public float Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public float Depth
        {
            get
            {
                return depth;
            }

            set
            {
                depth = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public VertexPositionNormalTexture[] Vertexes
        {
            get
            {
                return vertexes;
            }

            set
            {
                vertexes = value;
            }
        }

        #endregion Properties

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
            Vertexes = new VertexPositionNormalTexture[36];

            //top face
            Vector3 topLeftFront = Position + new Vector3(-Width, 1.0f, -Depth);
            Vector3 topLeftBack = Position + new Vector3(-Width, 1.0f, Depth);
            Vector3 topRightFront = Position + new Vector3(Width, 1.0f, -Depth);
            Vector3 topRightBack = Position + new Vector3(Width, 1.0f, Depth);

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = Position + new Vector3(-Width, -Height, -Depth);
            Vector3 btmLeftBack = Position + new Vector3(-Width, -Height, Depth);
            Vector3 btmRightFront = Position + new Vector3(Width, -Height, -Depth);
            Vector3 btmRightBack = Position + new Vector3(Width, -Height, Depth);

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f * Height, 0.0f * Width, 1.0f * Depth);
            Vector3 normalBack = new Vector3(0.0f * Height, 0.0f * Width, -1.0f * Depth);
            Vector3 normalTop = new Vector3(0.0f * Height, 1.0f * Width, 0.0f * Depth);
            Vector3 normalBottom = new Vector3(0.0f * Height, -1.0f * Width, 0.0f * Depth);
            Vector3 normalLeft = new Vector3(-1.0f * Height, 0.0f * Width, 0.0f * Depth);
            Vector3 normalRight = new Vector3(1.0f * Height, 0.0f * Width, 0.0f * Depth);

            // UV texture coordinates
            Vector2 textureTopLeft = new Vector2(1.0f * Height, 0.0f * Width);
            Vector2 textureTopRight = new Vector2(0.0f * Height, 0.0f * Width);
            Vector2 textureBottomLeft = new Vector2(1.0f * Height, 1.0f * Width);
            Vector2 textureBottomRight = new Vector2(0.0f * Height, 1.0f * Width);

            // Add the vertices for the FRONT face.
            Vertexes[0] = new VertexPositionNormalTexture(topLeftFront, normalFront, textureTopLeft);
            Vertexes[1] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            Vertexes[2] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);
            Vertexes[3] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            Vertexes[4] = new VertexPositionNormalTexture(btmRightFront, normalFront, textureBottomRight);
            Vertexes[5] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            // Add the vertices for the BACK face.
            Vertexes[6] = new VertexPositionNormalTexture(topLeftBack, normalBack, textureTopRight);
            Vertexes[7] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            Vertexes[8] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            Vertexes[9] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            Vertexes[10] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            Vertexes[11] = new VertexPositionNormalTexture(btmRightBack, normalBack, textureBottomLeft);

            // Add the vertices for the TOP face.
            Vertexes[12] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            Vertexes[13] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            Vertexes[14] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);
            Vertexes[15] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            Vertexes[16] = new VertexPositionNormalTexture(topRightFront, normalTop, textureBottomRight);
            Vertexes[17] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);

            // Add the vertices for the BOTTOM face.
            Vertexes[18] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            Vertexes[19] = new VertexPositionNormalTexture(btmLeftBack, normalBottom, textureBottomLeft);
            Vertexes[20] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            Vertexes[21] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            Vertexes[22] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            Vertexes[23] = new VertexPositionNormalTexture(btmRightFront, normalBottom, textureTopRight);

            // Add the vertices for the LEFT face.
            Vertexes[24] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);
            Vertexes[25] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            Vertexes[26] = new VertexPositionNormalTexture(btmLeftFront, normalLeft, textureBottomRight);
            Vertexes[27] = new VertexPositionNormalTexture(topLeftBack, normalLeft, textureTopLeft);
            Vertexes[28] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            Vertexes[29] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);

            // Add the vertices for the RIGHT face.
            Vertexes[30] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            Vertexes[31] = new VertexPositionNormalTexture(btmRightFront, normalRight, textureBottomLeft);
            Vertexes[32] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);
            Vertexes[33] = new VertexPositionNormalTexture(topRightBack, normalRight, textureTopRight);
            Vertexes[34] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            Vertexes[35] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);

            return Vertexes;
        }
    }
}