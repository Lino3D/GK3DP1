using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GK3DP1
{
    public class Bench
    {
        public Cube Seat { get; set; }
        public Cube Back { get; set; }
        public Cube Leg { get; set; }

        public VertexPositionNormalTexture[] Vertexes;

        //Other will be related();
        public Bench(Cube seat)
        {
            Seat = seat;
            Back = new Cube(seat.Width, seat.Height * 3, seat.Depth / 3,
                    new Vector3(seat.Position.X, seat.Position.Y + seat.Height * 2, (seat.Position.Z + seat.Depth)));

            var tmp = new List<VertexPositionNormalTexture>();
            tmp.AddRange(Seat.MakeCube());
            tmp.AddRange(Back.MakeCube());
            Vertexes = tmp.ToArray();
        }
    }
}