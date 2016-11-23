using FloorBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloorBuilder
{
    class CrawlerController
    {
        public static Random rand = new Random();
        public int DepthMax { get; set; }
        public int DepthMin { get; set; }
        public int StepsMax { get; set; }
        public int StepsMin { get; set; }
        public bool CanChangeDirection { get; set; }
        public double ChanceToSpawnLeft { get; set; }
        public double ChanceToSpawnRight { get; set; }
        public double ChanceToSpawnForward { get; set; }
        public Floor TargetFloor { get; set; }
        public List<Tile> OverwritableTiles { get; set; }

        private List<Crawler> Crawlers = new List<Crawler>();

        /// <summary>
        /// Returns a random integer between the Controller's StepsMin and StepsMax.
        /// </summary>
        /// <returns></returns>
        internal int GetStepsRandom()
        {
            return rand.Next(StepsMin, StepsMax);
        }

        public void UnregisterCrawler(Crawler crawler)
        {
            Crawlers.Remove(crawler);
        }

        public void RegisterCrawler(Crawler crawler)
        {
            Crawlers.Add(crawler);
        }
    }
}
