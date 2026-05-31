using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmicNavigation
{
    internal class IPathFinderImplementation:IPathfinder
    {
        public PathResult FindPath(CosmicMap map, Position start)
        {
            var queue = new PriorityQueue<Position, int>();
            var cameFrom = new Dictionary<Position, Position>();
            var costSoFar = new Dictionary<Position, int>();

            queue.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == map.Target) break;

                for (int i = 0; i < 4; i++)
                {
                    var next = new Position(current.Row + dRow[i], current.Col + dCol[i]);

                    if (next.Row < 0 || next.Row >= map.Rows || next.Col < 0 || next.Col >= map.Cols)
                        continue;

                    string cell = map.Grid[next.Row, next.Col];
                    if (cell == "X") continue;

                    int stepCost = (cell == "D") ? 2 : 1;
                    int newCost = costSoFar[current] + stepCost;

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        cameFrom[next] = current;
                        queue.Enqueue(next, newCost);
                    }
                }
            }

            if (!cameFrom.ContainsKey(map.Target))
                return new PathResult(false, 0, new List<Position>());

            var path = new List<Position>();
            var curr = map.Target;
            while (curr != start)
            {
                path.Add(curr);
                curr = cameFrom[curr];
            }
            path.Add(start);
            path.Reverse();

            return new PathResult(true, costSoFar[map.Target], path);
        }
    
}
}
