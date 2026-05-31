using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CosmicNavigation.CosmicNavigation;
using MimeKit;

namespace CosmicNavigation
{
    public class MissionControl
    {
        private const string MESSAGE_FOR_EMAIL_REPORT_HEADER = "\n--- Mission Control: Live Gmail Transmission ---";
        private const string MESSAGE_FOR_ASKING_IF_THE_USER_WANT_AN_EMAIL_REPORT = "Do you want to send this report via Gmail? (Y/N)";
        private const string MESSAGE_FOR_ACKNOLEDGING_THAT_THE_USER_HAVE_TO_ENTER_AN_EMAIL_OF_THE_SENDING_SIDE = "Enter Sender Email: ";
        private const string MESSAGE_FOR_ACKNOLEDGING_THAT_THE_USER_HAVE_TO_ENTER_THE_SPECIAL_PASSWORD= "Enter App Password (The 16-letter code ONLY(!!!!NB not a regular password.The password is provided only for this app): ";
        private const string MESSAGE_FOR_ACKNOLEDGING_THAT_THE_USER_HAVE_TO_ENTER_THE_RECIEVER_ADDRESS= "Enter Receiver Email: ";
        private const string TYPE_OF_THE_REPORT_IN_THE_EMAIL = "=== AUTOMATED MISSION REPORT ===\n\n";
        private const string NAME_OF_THE_REPORT_IN_THE_EMAIL = " Cosmic Navigation Mission Report";
        private const string NAME_OF_THE_SENDING_EMAIL_CENTER = "Cosmic Navigation System";
        private const string NAME_OF_THE_RECIEVEING_EMAIL_CENTER = "Mission Control";
        private const string NAME_OF_THE_HOST_FOR_THE_EMAIL_REPORT = "smtp.gmail.com";
        private const string SUCCESSFULL_SENDING_OF_THE_EMAIL_BETWEEN_THE_CENTERS = "\n[SUCCESS] Transmission verified. Report received through Gmail!";
        private const string MESSAGE_FOR_ACKNOKNOWLEDGING_THAT_THE_DATA_IS_SENT_AND_IT_IS_EXPECTED_TO_ARRIVE_SOON_THROUGHT_THE_HOST = "Transmitting mission data...";
        private const string MESSAGE_FOR_FAILING_THE_PASSWORD_IT_IS_EITHER_MISSING_OR_COMPLETELY_WRONG = "CRITICAL: You did not use the 16-letter App Password, or you had a typo in it.";
        private const string MESSAGE_ACKNOWLEDGES_THAT_THE_SYSTEM_IS_TRYING_TO_CONNECT_TO_THE_HOST = "\nConnecting to Google orbital relay (smtp.gmail.com)...";
        private const string STAR_SYMBOL = "*";
        private const string WORD_PLAIN = "plain";
        private const string SPACE_SYMBOL = " ";
        private const string STAR_AND_SPACE_SYMBOL = "* ";
        private const string FINAL_DESTINATION_SYMBOL_AND_SPACE = "F ";
        private readonly IPathfinder _pathfinder;

        public MissionControl(IPathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public void ExecuteMission(CosmicMap map)
        {
            var reportBuilder = new StringBuilder();

            foreach (var astronaut in map.Astronauts)
            {
                astronaut.Journey = _pathfinder.FindPath(map, astronaut.StartPosition);
            }

            var sortedAstronauts = map.Astronauts
                .OrderBy(a => a.Journey.Success ? a.Journey.Cost : int.MaxValue)
                .ToList();

           
            var failures = sortedAstronauts.Where(a => !a.Journey.Success).ToList();
            foreach (var fail in failures)
            {
                string msg = $"Mission failed — Astronaut {fail.Id} lost in space!";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ResetColor();
                reportBuilder.AppendLine(msg);
            }

           
            var successes = sortedAstronauts.Where(a => a.Journey.Success).ToList();
            foreach (var success in successes)
            {
                string msg = $"Astronaut {success.Id} - Shortest path: {success.Journey.Cost} steps";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg);
                Console.ResetColor();
                reportBuilder.AppendLine(msg);

                string visualMap = GenerateVisualMap(map, success);
                Console.WriteLine(visualMap);
                reportBuilder.AppendLine(visualMap);
            }

            PromptEmailReport(reportBuilder.ToString());
        }

        private string GenerateVisualMap(CosmicMap map, Astronaut astronaut)
        {
            var sb = new StringBuilder();
            var pathSet = new HashSet<Position>(astronaut.Journey.Path);

            for (int r = 0; r < map.Rows; r++)
            {
                for (int c = 0; c < map.Cols; c++)
                {
                    var pos = new Position(r, c);

                    if (pos == astronaut.StartPosition) sb.Append(astronaut.Id + SPACE_SYMBOL);
                    else if (pos == map.Target) sb.Append(FINAL_DESTINATION_SYMBOL_AND_SPACE);
                    else if (pathSet.Contains(pos)) sb.Append(STAR_AND_SPACE_SYMBOL);
                    else sb.Append(map.Grid[r, c] + SPACE_SYMBOL);
                }
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        private void PromptEmailReport(string report)
        {
            Console.WriteLine(MESSAGE_FOR_EMAIL_REPORT_HEADER);
            Console.WriteLine(MESSAGE_FOR_ASKING_IF_THE_USER_WANT_AN_EMAIL_REPORT);

            if (Console.ReadLine()?.Trim().ToUpper() != "Y") return;

            try
            {
                Console.Write(MESSAGE_FOR_ACKNOLEDGING_THAT_THE_USER_HAVE_TO_ENTER_AN_EMAIL_OF_THE_SENDING_SIDE);
                string senderEmail = Console.ReadLine()!;

                Console.Write(MESSAGE_FOR_ACKNOLEDGING_THAT_THE_USER_HAVE_TO_ENTER_THE_SPECIAL_PASSWORD);
               
                string password = HidePassword();

                Console.Write(MESSAGE_FOR_ACKNOLEDGING_THAT_THE_USER_HAVE_TO_ENTER_THE_RECIEVER_ADDRESS);
                string receiverEmail = Console.ReadLine()!;

              
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(NAME_OF_THE_SENDING_EMAIL_CENTER, senderEmail));
                message.To.Add(new MailboxAddress(NAME_OF_THE_RECIEVEING_EMAIL_CENTER, receiverEmail));
                message.Subject = NAME_OF_THE_REPORT_IN_THE_EMAIL;

                message.Body = new TextPart(WORD_PLAIN)
                {
                    Text = TYPE_OF_THE_REPORT_IN_THE_EMAIL + report
                };

                
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    Console.WriteLine(MESSAGE_ACKNOWLEDGES_THAT_THE_SYSTEM_IS_TRYING_TO_CONNECT_TO_THE_HOST);

                    
                    client.Connect(NAME_OF_THE_HOST_FOR_THE_EMAIL_REPORT, 587, MailKit.Security.SecureSocketOptions.StartTls);

                   
                    client.Authenticate(senderEmail, password);

                    Console.WriteLine(MESSAGE_FOR_ACKNOKNOWLEDGING_THAT_THE_DATA_IS_SENT_AND_IT_IS_EXPECTED_TO_ARRIVE_SOON_THROUGHT_THE_HOST);
                    client.Send(message);

                    client.Disconnect(true);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(SUCCESSFULL_SENDING_OF_THE_EMAIL_BETWEEN_THE_CENTERS);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[TRANSMISSION FAILED] {ex.Message}");
                Console.WriteLine(MESSAGE_FOR_FAILING_THE_PASSWORD_IT_IS_EITHER_MISSING_OR_COMPLETELY_WRONG);
                Console.ResetColor();
            }
        }

      
        private string HidePassword()
        {
            var password = new StringBuilder();
            while (true)
            {
               
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b"); 
                    }
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                   
                    password.Append(keyInfo.KeyChar);
                    Console.Write(STAR_SYMBOL);
                }
            }
            return password.ToString();
        }
    }
}