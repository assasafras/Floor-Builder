using FloorBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorBuilder
{
    public class Floor
    {
        private static Random r = new Random();
        bool debug = false;
        private Point previousPosition;
        private Point current;
        public int Height { get; set; }
        public int Width { get; set; }

        public Tile[,] Model { get; set; }

        public Floor(int width, int height)
        {
            Height = height;
            Width = width;
            Model = new Tile[Width, Height];
        }

        public void Init(int iterations)
        {

            // Pick a starting point to lay floor tiles.
            var startX = r.Next(Width);
            var startY = r.Next(Height);
            var start = new Point(startX, startY);

            current = start;
            previousPosition = current;
            for (int i = 0; i < iterations; i++)
            {
                // Create a random direction (left, right, up or down only).
                var direction = Point.RandomDirection(-1, 1, DirectionFlag.NoDiagonals);

                // Move to the new position.
                var newPosition = current + direction;

                // Make sure the new position is out not out of bounds, and if it is then change direction until within bounds
                newPosition = DealWithOutOfBounds(newPosition, direction);

                // If the direction has changed figure out what the new direction is.
                direction = newPosition - previousPosition;

                // Make sure the position we now plan to place a tile on is a free tile.
                newPosition = DealWithOccupiedTile(newPosition, direction);

                current = newPosition;
                previousPosition = current;
                PlaceFloorTile(current);
                //PrintModel();
                //if (debug) Console.WriteLine("-----------------------");
                //if (debug) Console.ReadKey();
            }
        }

        private Point DealWithOccupiedTile(Point position, Point direction, int tries = 100)
        {
            var initPos = position;
            var initDir = direction;
            if (debug) Console.WriteLine("DealWithOccupiedTile({0}, {1}, {2}) called!", position, direction, tries);

            if (tries < 0)
            {
                if (debug) Console.WriteLine("\t No tries remain, returning a random point on the floor and hoping for the best!");
                var x = r.Next(Width);
                var y = r.Next(Height);
                // Try get an empty point on the map.
                return new Point(x, y);
            }
            Point newPosition = position;

            if (Model[position.X, position.Y] == Tile.Floor)
            {
                if (debug) Console.WriteLine("\tPosition: {0} is occupied already, moving by {1}", position, direction);

                // Tile is occupied, move in our current direction.
                newPosition = position + direction;

                if (debug) Console.WriteLine("\tPosition after moving: [{0}]", newPosition);

                // Ensure we're not out of bounds.
                newPosition = DealWithOutOfBounds(newPosition, direction);
                if (debug) Console.WriteLine("\tPosition after dealing with out of bounds: [{0}]", newPosition);

                // If the direction has changed figure out what the new direction is.
                direction = newPosition - initPos;
                if (debug) Console.WriteLine("\tDirection after dealing with out of bounds: [{0}]", direction);

                // Make sure the position we now plan to place a tile on is a free tile.
                newPosition = DealWithOccupiedTile(newPosition, direction, --tries);
            }

            if (debug) Console.WriteLine("\tDealWithOccupiedTile({0}, {1}) returning: {2}", initPos, initDir, newPosition);

            return newPosition;
        }

        private Point DealWithOutOfBounds(Point position, Point direction)
        {
            if (debug) Console.WriteLine("DealWithOutOfBounds({0}, {1}) called!", position, direction);

            var initPos = position;
            var initDir = direction;
            if (IsOutOfBounds(position))
            {
                if (debug) Console.WriteLine("\t Position: {0} is out of bounds!", position);
                // Back out the change.
                position -= direction;
                if (debug) Console.WriteLine("\t Position: {0} after backing out of move!", position);
                 
                // Change direction.
                direction = ChangeDirection(direction);

                if (debug) Console.WriteLine("\t New Direction: {0}", direction);

                // Move again.
                position += direction;
                if (debug) Console.WriteLine("\t Position: {0} after moving again!", position);

                // Make sure we are still within bounds.
                position = DealWithOutOfBounds(position, direction);
            }
            if (debug) Console.WriteLine("\tDealWithOutOfBounds({0}, {1}) returned: {2}!", initPos, initDir, position);
            // Finally return a position which should now be within bounds.
            return position;
        }

        private Point ChangeDirection(Point currentDirection)
        {
            Point pt = new Point();
            // If we were travelling left move upward.
            if (currentDirection.X < 0)
                pt = new Point(0, currentDirection.X);
            // If we were moving right move downward.
            else if (currentDirection.X > 0)
                pt = new Point(0, currentDirection.X);
            // If we were moving up move right.
            else if (currentDirection.Y < 0)
                pt = new Point(-currentDirection.Y, 0);
            // If we were moving down move left.
            else if (currentDirection.Y > 0)
                pt = new Point(-currentDirection.Y, 0);
            return pt;
        }

        public bool IsOutOfBounds(Point pt)
        {
            return (pt.X >= Width || pt.X < 0 || pt.Y >= Height || pt.Y < 0);
        }
        public void PrintModel()
        {
            PrintModel(new Point(-1, -1));
        }
        public void PrintModel(Point currentPosition)
        {
            // Write the top border.
            Console.Write("╔");
            for (int a = 0; a <= Width; a++)
            {
                if (a == Width)
                    Console.WriteLine("══╗");
                else
                    Console.Write("══╦");
            }

            // Write the columns numbers.
            Console.Write("║  ║");
            for (int a = 0; a < Width; a++)
            {
                Console.Write((a < 10) ? " " : "");
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(a);
                
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("║");
            }
            Console.WriteLine();

            // Write each row border and then the row.
            for (int i = 0; i < Height; i++)
            {
                // Write the row border.
                Console.Write("╠");
                for (int b = 0; b < Width; b++)
                {
                    Console.Write("══╬");
                }
                Console.WriteLine("══╣");

                // Write the row number.
                Console.Write((i < 10) ? "║ " : "║"); ;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(i);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("║");


                // Write each entry in the row.
                for (int j = 0; j < Width; j++)
                {
                    var tile = Model[j, i];
                    if ((int) tile != 0)
                    {
                        if(currentPosition.X == j && currentPosition.Y == i)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write((((int) tile < 10) ? " " : "") + (int) tile);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("║");
                    }
                    else
                        Console.Write(" " + (int) tile + "║");
                }
                Console.WriteLine();
            }

            Console.Write("╚");
            for (int c = 0; c < Width; c++)
            {
                Console.Write("══╩");
                if (c == Width - 1)
                    Console.WriteLine("══╝");
            }
        }

        private void PlaceFloorTile(Point pt)
        {
            Place(pt, Tile.Floor);
        }

        public void Place(Point pt, Tile tile)
        {
            // Cast the Tile enum to an integer and carry on.
            Place(pt.X, pt.Y, tile);
        }

        public void Place(int x, int y, Tile tile)
        {
            Model[x, y] = tile;
        }
    }
}
