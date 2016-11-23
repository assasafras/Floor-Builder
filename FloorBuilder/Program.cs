using FloorBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorBuilder
{
    class Program
    {
        private static bool i;
        static void Main(string[] args)
        {

            while (true)
            {
                var defaultWidth = 50;
                Console.WriteLine("Floor Width({0}): ", defaultWidth);
                int width;
                var b = int.TryParse(Console.ReadLine(), out width);
                width = b ? width : defaultWidth;

                //var width = int.TryParse(Console.ReadLine());

                var defaultHeight = 18;
                Console.WriteLine("Floor Height({0}): ", defaultHeight);
                int height;
                b = int.TryParse(Console.ReadLine(), out height);
                height = b ? height : defaultHeight;

                var defaultDepthMin = 10;
                Console.WriteLine("Depth Min({0}): ", defaultDepthMin);
                int depthMin;
                b = int.TryParse(Console.ReadLine(), out depthMin);
                depthMin = b ? depthMin : 10;

                var defaultDepthMax = 10;
                Console.WriteLine("Depth Max({0}): ", defaultDepthMax);
                int depthMax;
                b = int.TryParse(Console.ReadLine(), out depthMax);
                depthMax = b ? depthMax : defaultDepthMax;

                var f = new Floor(width, height);
                //f.Init(iterations);
                var controller = new CrawlerController()
                    {
                        StepsMax = 10,
                        StepsMin = 2,
                        TargetFloor = f,
                        CanChangeDirection = false,
                        ChanceToSpawnForward = 0.5,
                        ChanceToSpawnLeft = 0.3,
                        ChanceToSpawnRight = 0.3,
                        DepthMax = depthMax,
                        DepthMin = depthMin,
                        OverwritableTiles = new List<Tile>()
                        {
                            Tile.Empty,
                            Tile.Floor
                        }
                    };

                var startPoint = Point.RandomPoint(f.Width, f.Height);
                var startDirection = Point.RandomDirection(-1, 1, DirectionFlag.NoDiagonals);
                var startDepth = 0;
                var c = new Crawler(controller, startPoint, startDirection, startDepth);

                // Kick the bastard off!

                // Add a room.
                var room = new Room();
                var r = Extensions.rand;
                room.Create(new Point(r.Next(1, f.Width - room.Right), r.Next(1, f.Height - room.Bottom)), f);
                room.Create(new Point(r.Next(1, f.Width - room.Right), r.Next(1, f.Height - room.Bottom)), f);
                f.PrintModel();


                Console.WriteLine("Press anything to add corridors!");
                Console.ReadKey();

                f.Model.Fill(Tile.Floor);

                //c.Go();
                f.PrintModel();

                Console.ReadKey();
            }
        }
    }
}
