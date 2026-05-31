using System.Collections.Generic;
using CosmicNavigation.CosmicNavigation;

namespace CosmicNavigation
{
    public class CosmicMap
    {
        public string[,] Grid { get; }
        public int Rows => Grid.GetLength(0);
        public int Cols => Grid.GetLength(1);
        public Position Target { get; private set; }
        public List<Astronaut> Astronauts { get; } = new();

        public CosmicMap(string[,] grid)
        {
            Grid = grid;
            LocateEntities();
        }

        private void LocateEntities()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    string cell = Grid[r, c];
                    if (cell == "F") Target = new Position(r, c);
                    else if (cell.StartsWith("S")) Astronauts.Add(new Astronaut(cell, new Position(r, c)));
                }
            }
        }
    }
}
