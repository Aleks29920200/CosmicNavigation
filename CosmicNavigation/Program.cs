using System;

namespace CosmicNavigation
{
    class Program
    {
        private const string NAME_OF_THE_SYSTEM = "WELCOME TO THE COSMIC NAVIGATION SYSTEM";
        private const string FIRST_OPTION_FOR_THE_MAP_THAT_IS_TO_MANUALLY_TO_TYPE_EVERYTHING = "1 option: User manually types parameters for map and the map";
        private const string SECOND_OPTION_FOR_THE_MAP_WHICH_IS_TO_TYPE_ONLY_ROWS_AND_COLS_FOR_THE_MAP_BUT_THE_SYSTEM_WILL_GENERATE_THE_MAP_RANDOMLY = "2 option: The map is generated randomly only parameters are typed manually";
        private const string MESSAGE_THAT_ACKNOWLEDGES_THAT_YOU_HAVE_TO_CHOOSE_BETWEEN_THE_TWO_OPTIONS_WITH_WRITING_THE_NUMBER_OF_OPTION_THAT_YOU_WANT_TO_CHOOSE = "Select Input Method: ";
        private const string MISSION_RESULTS_HEADER = "\n--- MISSION RESULTS ---\n";
        private const string VALIDATION_MESSAGE_IF_THE_ROWS_ARE_BIGGER_OR_SLOWER_THAN_THE_GIVEN_NUMBERS = "Dimensions must be between 2 and 100.";
        private const string MESSAGE_TO_INFORM_WHAT_THE_USER_NEEDS_TO_DO_FOR_THE_ROWS = "Type the number of rows that you want the map to have";
        private const string MESSAGE_TO_INFORM_WHAT_THE_USER_NEEDS_TO_DO_FOR_THE_COLS = "Type the number of columns that you want the map to have";
        private const string MESSAGE_THAT_INFORMS_THAT_YOU_ARE_GOING_TO_TYPE_THE_ROWS_FOR_THE_MAP = "Number rows for the map: ";
        private const string MESSAGE_THAT_INFORMS_THAT_YOU_ARE_GOING_TO_TYPE_THE_COLS_FOR_THE_MAP= "Number columns of the map: ";
        private const string ASTEROID_FOR_THE_MAP_SYMBOL = "X";
        private const string FINAL_DESTINATION_FOR_THE_MAP_SYMBOL = "F";
        private const string ASTRONAUT_FOR_THE_MAP_SYMBOL = "S";
        private const string DEBRIS_FOR_THE_MAP_SYMBOL = "D";
        private const string OPEN_SPACE_FOR_THE_MAP_SYMBOL = "0";
        private const string SUCCESSFUL_CREATION_OF_THE_MAP_MESSAGE = "\nGenerated Map:";
        private const string NUMBER_TWO = "2";

        static void Main(string[] args)
        {
            Console.WriteLine(NAME_OF_THE_SYSTEM);
            Console.WriteLine(FIRST_OPTION_FOR_THE_MAP_THAT_IS_TO_MANUALLY_TO_TYPE_EVERYTHING);
            Console.WriteLine(SECOND_OPTION_FOR_THE_MAP_WHICH_IS_TO_TYPE_ONLY_ROWS_AND_COLS_FOR_THE_MAP_BUT_THE_SYSTEM_WILL_GENERATE_THE_MAP_RANDOMLY);
            Console.Write(MESSAGE_THAT_ACKNOWLEDGES_THAT_YOU_HAVE_TO_CHOOSE_BETWEEN_THE_TWO_OPTIONS_WITH_WRITING_THE_NUMBER_OF_OPTION_THAT_YOU_WANT_TO_CHOOSE);

            string[,] rawGrid = null;

            if (Console.ReadLine()?.Trim() == NUMBER_TWO)
            {
                rawGrid = GenerateRandomMap();
            }
            else
            {
                rawGrid = ReadMapFromConsole();
            }

            if (rawGrid == null) return;

            var map = new CosmicMap(rawGrid);
            var pathfinder = new IPathFinderImplementation();
            var missionControl = new MissionControl(pathfinder);

            Console.WriteLine(MISSION_RESULTS_HEADER);
            missionControl.ExecuteMission(map);
        }

        static string[,] ReadMapFromConsole()
        {
            try
            {
                int m, n;
                InputDataForRowsAndColumns(out m, out n);

                if (m < 2 || m > 100 || n < 2 || n > 100)
                    throw new ArgumentException(VALIDATION_MESSAGE_IF_THE_ROWS_ARE_BIGGER_OR_SLOWER_THAN_THE_GIVEN_NUMBERS);

                var grid = new string[m, n];
                Console.WriteLine($"Enter the cosmic map ({m} lines):");
                for (int i = 0; i < m; i++)
                {
                    var tokens = Console.ReadLine()!.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < n; j++)
                    {
                        grid[i, j] = tokens[j];
                    }
                }
                return grid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading map: {ex.Message}");
                return null;
            }
        }

        private static void InputDataForRowsAndColumns(out int m, out int n)
        {
            Console.WriteLine(MESSAGE_TO_INFORM_WHAT_THE_USER_NEEDS_TO_DO_FOR_THE_ROWS);
            Console.Write(MESSAGE_THAT_INFORMS_THAT_YOU_ARE_GOING_TO_TYPE_THE_ROWS_FOR_THE_MAP);
            m = int.Parse(Console.ReadLine()!);
            Console.WriteLine(MESSAGE_TO_INFORM_WHAT_THE_USER_NEEDS_TO_DO_FOR_THE_COLS);
            Console.Write(MESSAGE_THAT_INFORMS_THAT_YOU_ARE_GOING_TO_TYPE_THE_COLS_FOR_THE_MAP);
            n = int.Parse(Console.ReadLine()!);
        }

        static string[,] GenerateRandomMap()
        {
            InputDataForRowsAndColumns(out int m, out int n);

            var grid = new string[m, n];
            var rand = new Random();

            
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int roll = rand.Next(100);
                    if (roll < 20) grid[i, j] = ASTEROID_FOR_THE_MAP_SYMBOL; 
                    else if (roll < 30) grid[i, j] = DEBRIS_FOR_THE_MAP_SYMBOL; 
                    else grid[i, j] = OPEN_SPACE_FOR_THE_MAP_SYMBOL;
                }
            }

           
            grid[rand.Next(m), rand.Next(n)] = FINAL_DESTINATION_FOR_THE_MAP_SYMBOL;

         
            int astroCount = rand.Next(1, 4);
            for (int a = 1; a <= astroCount; a++)
            {
                int r, c;
                do
                {
                    r = rand.Next(m);
                    c = rand.Next(n);
                } while (grid[r, c] == FINAL_DESTINATION_FOR_THE_MAP_SYMBOL || grid[r, c].StartsWith(ASTRONAUT_FOR_THE_MAP_SYMBOL));

                grid[r, c] = $"S{a}";
            }

            Console.WriteLine(SUCCESSFUL_CREATION_OF_THE_MAP_MESSAGE);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++) Console.Write(grid[i, j] + " ");
                Console.WriteLine();
            }

            return grid;
        }
    }
}