using FloorBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorBuilder
{
    public class Room
    {
        public int Top { get { return -1; } }
        public int Bottom { get { return Height; } }
        public int Left { get { return -1; } }
        public int Right { get { return Width; } }

        public int InnerTop { get { return 0; } }
        public int InnerBottom { get { return Height - 1; } }
        public int InnerLeft { get { return 0; } }
        public int InnerRight { get { return Width - 1; } }

        public int Width { get; set; }
        public int Height { get; set; }
        /// <summary>
        /// Not currently used. Need to implement logic to have entrances deal with thickness first.
        /// </summary>
        public int WallThickness { get; set; }
        /// <summary>
        /// Note: Entrace positions are relative to the start point of a room 
        /// (top left corner of the room (not including walls).
        /// </summary>
        public Entrance[] Entrances { get; set; }
        
        /// <summary>
        /// Creates a 5 x 5 room with 2 exits and 1 tile thick walls
        /// </summary>
        public Room() : this(5, 5, 2) {}

        public Room (int width, int height, int numberOfEntrances, int wallThickness = 1)
	    {
            Width = width;
            Height = height;
            WallThickness = wallThickness;
            CreateEntrances(numberOfEntrances);
	    }

        private void CreateEntrances(int numberOfEntrances)
        {
            var r = new Random();
            Entrances = new Entrance[numberOfEntrances];
            var availableDirections = new List<Direction> 
                { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            for (int i = 0; i < numberOfEntrances; i++)
            {
                var direction = availableDirections.PopRandom();
                var entrance = Entrances[i] = new Entrance();
                switch (direction)
                {
                    case Direction.Up:
                        entrance.Direction = Point.Up();
                        entrance.Position = new Point(r.Next(Width), this.Top);
                        break;
                    case Direction.Down:
                        entrance.Direction = Point.Down();
                        entrance.Position = new Point(r.Next(Width), this.Bottom);
                        break;
                    case Direction.Left:
                        entrance.Direction = Point.Left();
                        entrance.Position = new Point(this.Left, r.Next(Height));
                        break;
                    case Direction.Right:
                        entrance.Direction = Point.Right();
                        entrance.Position = new Point(this.Right, r.Next(Height));
                        break;
                }
            }
        }

        public void Create(Point start, Floor TargetFloor)
        {
            // Lay down the floor and wall tiles.
            for (int x = -1; x < Width + 1; x++)
                for (int y = -1; y < Height + 1; y++)
                {
                    var current = new Point(start.X + x, start.Y + y);
                    var tile = (x < 0 || x >= Width || y < 0 || y >= Height) ? Tile.Wall : Tile.Floor;
                    TargetFloor.Place(current.X, current.Y, tile);
                }

            // Place the entrance/exit tiles.
            foreach (var entrance in Entrances)
                TargetFloor.Place(entrance.Position + start, Tile.Entrance);
        }
    }
}
