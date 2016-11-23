using FloorBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorBuilder
{
    class Crawler
    {
        /// <summary>
        /// All Crawler's should use this RNG to determine their randomized outcomes.
        /// </summary>
        private static Random rand = new Random();
        private static int nextID = 0;

        public int ID { get; set; }
        public bool Alive { get; set; }
        /// <summary>
        /// The total number of steps this crawler will take within it's lifetime.
        /// </summary>
        public int StepsTotal { get; set; }
        /// <summary>
        /// The number of steps it will take before dying.
        /// </summary>
        public int StepsRemaining 
        { 
            get { return stepsRemaining; }
            set { 
                stepsRemaining = value;
                if (stepsRemaining <= 0)
                    Kill();
            }
        }

        private int stepsRemaining;
        /// <summary>
        /// When true the Crawler will kill itself when it lands on a position which already contains a tile.<para/>
        /// If false the Crawler will continue moving around until it dies in the hopes of finding a free tile before death.
        /// </summary>
        public bool DiesOnExistingTile { get; set; }
        /// <summary>
        /// When true the Crawler will kill itself when it leaves the boundary of whatever bounds it.<para/>
        /// Currently this is always true.
        /// </summary>
        public bool DiesOnOutOfBounds { get { return true; } }

        /// <summary>
        /// The floor that the Crawler places tiles upon.
        /// </summary>
        public Floor TargetFloor { get { return Controller.TargetFloor; } }

        public CrawlerController Controller { get; set; }

        /// <summary>
        /// The position of the Crawler.
        /// </summary>
        public Point CurrentPosition 
        {
            get { return currentPosition; }
            set
            {
                if (TargetFloor.IsOutOfBounds(value)
                    || !Controller.OverwritableTiles.Contains(TargetFloor.Model[value.X, value.Y]))// && this.DiesOnOutOfBounds)
                {
                    // Kill();

                    Debug.Print("Crawler [{3}] at position {0} depth {1} is turning around. {2} steps still remain.", value, Depth, StepsRemaining, ID);
                    // Turn around.
                    CurrentHeading *= -1;
                }
                else
                {
                    StepsRemaining--;
                    currentPosition = value;
                }
            }
        }
        public Point currentPosition;

        /// <summary>
        /// The heading of the Crawler, basically the way it's facing.
        /// </summary>
        public Point CurrentHeading { get; set; }

        /// <summary>
        /// Creates a new Crawler Object.
        /// </summary>
        /// <param name="controller">The Controller which sets behaviour for the Crawler.</param>
        /// <param name="position">The position at which the Crawler will spawn.</param>
        /// <param name="heading">THe starting heading/facing of the Crawler.</param>
        /// <param name="depth">How many ancestors this Crawler has had.</param>
        public Crawler(CrawlerController controller, Point position, Point heading, int depth)
        {
            ID = nextID++;
            Debug.Print("Creating Crawler [{4}] ({0}, {1}, {2}, {3})", controller, position, heading, depth, ID);
            this.Controller = controller;

            // Register with Controller.
            Controller.RegisterCrawler(this);

            // Determine how many steps this Crawler will take before offing itself.
            StepsRemaining = StepsTotal = Controller.GetStepsRandom();

            this.Depth = depth;
            currentPosition = position;
            this.CurrentHeading = heading;

            Alive = true;
        }

        /// <summary>
        /// Makes the Crawler move around and place tiles until it runs out of steps.
        /// </summary>
        public virtual void Go()
        {
            Move(StepsTotal);
        }

        /// <summary>
        /// Moves the crawler forward one space and place a floor tile.
        /// </summary>
        public void Move()
        {
            if (Alive)
            {
                // Move forward.
                CurrentPosition += CurrentHeading;
                // Place a tile.
                TargetFloor.Place(CurrentPosition, Tile.Floor); 
            }
        }

        private void Move(int distance)
        {
            while (distance > 0 && Alive)
            {
                Move();
                //Console.WriteLine();
                //TargetFloor.PrintModel(currentPosition);
                //Console.ReadKey();
            }
        }

        /// <summary>
        /// Turns the crawler left, relative to it's current heading.
        /// </summary>
        public void TurnLeft()
        {
            // Are we heading up or down?
            if (Math.Abs(CurrentHeading.Y) > 0)
                // Make it so we head left or right.
                CurrentHeading = new Point(-CurrentHeading.Y, 0);
            // Must be heading left or right then.
            else
                // So head up or down.
                CurrentHeading = new Point(0, CurrentHeading.X);
        }

        /// <summary>
        /// Turns the crawler right, relative to it's current heading.
        /// </summary>
        public void TurnRight()
        {
            // Are we heading up or down?
            if (Math.Abs(CurrentHeading.Y) > 0)
                // Make it so we head left or right.
                CurrentHeading = new Point(CurrentHeading.Y, 0);
            // Must be heading left or right then.
            else
                // So head up or down.
                CurrentHeading = new Point(0, -CurrentHeading.X);
        }

        /// <summary>
        /// Destroys the Crawler and spawn new crawlers if possible.
        /// </summary>
        private void Kill()
        {
            Alive = false;
            // Check that we haven't gone too deep.
            if (this.Depth < Controller.DepthMax)
            {
                SpawnNewCrawlers();
            }
            // Unregister with COntroller in the hopes of dropping memory.
            Controller.UnregisterCrawler(this);
        }

        private void SpawnNewCrawlers()
        {
            int spawned = 0;
            // Say our chance to spawn in a given direction is 30% (0.3) then there's a 30% chance that 
            //  the next random double (between 0.0 and 1.0) will be less than this.
            // Hence why we check if our random number is less than the chance to spawn in a direction.

            // CHeck if we should spawn one left.
            if (rand.NextDouble() < Controller.ChanceToSpawnLeft)
            {
                Debug.Print("Crawler [{0}] Spawning a Crawler to go left.", ID);
                var l = new Crawler(Controller, this.CurrentPosition, this.CurrentHeading, Depth + 1);
                ++spawned;
                l.TurnLeft();
                l.Go();
            }

            // Check if we should spawn one right.
            if (rand.NextDouble() < Controller.ChanceToSpawnRight)
            {
                Debug.Print("Crawler [{0}] Spawning a Crawler to go right.", ID);
                var r = new Crawler(Controller, this.CurrentPosition, this.CurrentHeading, Depth + 1);
                ++spawned;
                r.TurnRight();
                r.Go();
            }

            // Check to spawn one forward
            if (rand.NextDouble() < Controller.ChanceToSpawnForward)
            {
                Debug.Print("Crawler [{0}] Spawning a Crawler continue forward.", ID);
                var f = new Crawler(Controller, this.CurrentPosition, this.CurrentHeading, Depth + 1);
                ++spawned;
                f.Go();
            }

            // If we didn't spawn any and we haven't met the minimum depth for our controller then try again.
            if (spawned == 0 && Depth < Controller.DepthMin)
            {
                SpawnNewCrawlers();
            }
        }

        /// <summary>
        /// The depth of a crawler describes how many ancestors the crawler has had.
        /// </summary>
        public int Depth { get; set; }
    }
}
