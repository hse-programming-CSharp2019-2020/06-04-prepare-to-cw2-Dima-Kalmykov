using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MyLib;

namespace Program_1
{
    class Program
    {
        // Todo check name for char letters.

        private const string PathToInput = "data.txt";
        private const string PathToOutput = @"..\..\..\out.txt";
        private const int MinStreetNameLength = 4;
        private const int MaxStreetNameLength = 10;


        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond);

        private static bool AreCorrectedHouses(IReadOnlyCollection<int> houses) =>
            houses.Count != 0 && houses.All(h => h <= 100 && h > 0);

        private static Street GetStreet(string lineInfo)
        {
            string[] arrInfo = lineInfo.Split();

            var streetName = arrInfo[0];

            int[] streetHouses = arrInfo
                .Skip(1)
                .Select(int.Parse)
                .ToArray();

            if (AreCorrectedHouses(streetHouses))
            {
                return new Street(streetName, streetHouses);
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Get number.
        /// </summary>
        /// <typeparam name="T"> Type of number </typeparam>
        /// <param name="message"> Help message </param>
        /// <param name="conditions"> Conditions for number </param>
        /// <returns> Number </returns>
        private static T GetData<T>(string message, Predicate<T> conditions)
        {
            Console.Write(message);

            while (true)
            {
                try
                {
                    // Attempt to convert input string to required type.
                    var result = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));

                    // Check extra conditions.
                    if (conditions(result))
                        return result;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Number must be > 0: ");
                    Console.ResetColor();
                }
                catch
                {
                    // Print error message.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong format of input data!");
                    Console.ResetColor();

                    Console.Write(message);
                }
            }
        }

        private static string GetRandomStreetName()
        {
            var result = new StringBuilder();

            var length = Rnd.Next(MinStreetNameLength, MaxStreetNameLength + 1);

            for (var i = 0; i < length; i++)
            {
                result.Append((char)Rnd.Next('a', 'z' + 1));
            }

            return result.ToString();
        }

        private static Street GetRandomStreet()
        {
            var houseAmount = Rnd.Next(2, 11);
            var houses = new int[houseAmount];

            for (var i = 0; i < houseAmount; i++)
            {
                houses[i] = Rnd.Next(1, 101);
            }

            return new Street(GetRandomStreetName(), houses);
        }


        /// <summary>
        /// Print color message.
        /// </summary>
        /// <param name="message"> Message </param>
        /// <param name="color"> Message's color </param>
        private static void PrintMessage(string message, ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        private static void SerializeObjects(Street[] streetsArray)
        {
            using (var fs = new FileStream(PathToOutput, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new XmlSerializer(typeof(Street[]));

                formatter.Serialize(fs, streetsArray);
            }

            PrintMessage("Serialize was successful\n", ConsoleColor.Green);
        }

        private static void Serialize(Street[] streetsArray)
        {
            try
            {
                SerializeObjects(streetsArray);
            }
            catch (SerializationException) { }
            catch (IOException) { }
            catch (SecurityException) { }
            catch (Exception)
            {
            }
        }

        private static void Main()
        {
            var streetCounter = 0;
            var n = GetData<int>("Enter amount of houses: ", el => el > 0);
            var streetsArray = new Street[n];
            try
            {
                using (var sr = new StreamReader(new FileStream(PathToInput, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    string lineInfo;

                    while ((lineInfo = sr.ReadLine()) != null && streetCounter < n)
                    {
                        streetsArray[streetCounter++] = GetStreet(lineInfo);
                    }
                }


            }

            catch (ArgumentException) { }
            catch (FormatException) { }
            catch (IOException) { }
            catch (SecurityException) { }
            catch
            {
                Console.WriteLine("adasd");
                for (var i = 0; i < n; i++)
                {
                    streetsArray[i] = GetRandomStreet();
                }
            }

            PrintMessage("\nStreets:\n\n");
            foreach (var street in streetsArray)
            {
                PrintMessage($"{street}\n", ConsoleColor.Yellow);
            }

            Serialize(streetsArray);
            Console.ReadLine();
        }
    }
}
