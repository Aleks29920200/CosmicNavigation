using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmicNavigation
{
    public interface IPathfinder
    {
        PathResult FindPath(CosmicMap map, Position start);
    }
}
