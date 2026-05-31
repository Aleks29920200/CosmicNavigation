using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmicNavigation.PathFinding
{
    public interface IPathfinder
    {
        PathResult FindPath(CosmicMap map, Position start);
    }
}
