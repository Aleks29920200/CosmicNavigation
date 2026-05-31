using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmicNavigation
{
    namespace CosmicNavigation
    {
        public class Astronaut
        {
            public string Id { get; }
            public Position StartPosition { get; }
            public PathResult Journey { get; set; }

            public Astronaut(string id, Position startPosition)
            {
                Id = id;
                StartPosition = startPosition;
            }
        }
    }
}
