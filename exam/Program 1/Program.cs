using MyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Xml.Serialization;

namespace exam
{
    internal class Program1
    {
        #region Constants

        private const string PathToInput = "data.txt";
        private const string PathToOutput = @"..\..\..\out.ser";

        private const int MinStreetNameLength = 4;
        private const int MaxStreetNameLength = 10;

        private const int MinHousesAmount = 1;
        private const int MaxHousesAmount = 10;

        #endregion

        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Check houses for correctness.
        /// </summary>
        /// <param name="houses"> Houses </param>
        /// <returns> True, if all houses are corrected,
        /// else - false </returns>
        private static bool AreCorrectedHouses(IReadOnlyCollection<int> houses) =>
            houses.Count != 0 && houses.All(h => h <= 100 && h > 0);

        /// <summary>
        /// Check name for correctness.
        /// </summary>
        /// <param name="name"> Name of street </param>
        /// <returns> True, if name is corrected,
        /// else - false </returns>
        private static bool IsCorrectedName(string name) =>
            name.ToCharArray().All(char.IsLetter);

        /// <summary>
        /// Get street from line.
        /// </summary>
        /// <param name="lineInfo"> Line with info </param>
        /// <returns> new street </returns>
        private static Street GetStreet(string lineInfo)
        {
            string[] arrInfo = lineInfo.Split();

            var streetName = arrInfo[0];

            int[] streetHouses = arrInfo
                .Skip(1)
                .Select(int.Parse)
                .ToArray();

            if (AreCorrectedHouses(streetHouses) && IsCorrectedName(streetName))
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
        private static T GetNumber<T>(string message, Predicate<T> conditions)
        {
            PrintMessage(message);

            while (true)
            {
                try
                {
                    // Attempt to convert input string to required type.
                    var result = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));

                    // Check extra conditions.
                    if (conditions(result))
                        return result;

                    PrintMessage("NUmber must be > 0: ", ConsoleColor.Yellow);
                }
                catch
                {
                    // Print error message.
                    PrintMessage("Wrong format of input data!\n", ConsoleColor.Red);

                    PrintMessage(message);
                }
            }
        }

        /// <summary>
        /// Get random name of street.
        /// </summary>
        /// <returns> random string </returns>
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

        /// <summary>
        /// Get random street.
        /// </summary>
        /// <returns> New street </returns>
        private static Street GetRandomStreet()
        {
            var houseAmount = Rnd.Next(MinHousesAmount + 1, MaxHousesAmount + 1);
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

        /// <summary>
        /// Serialize objects.
        /// </summary>
        /// <param name="streetsArray"> Array of streets </param>
        private static void SerializeStreets(IEnumerable streetsArray)
        {
            using (var fs = new FileStream(PathToOutput, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new XmlSerializer(typeof(Street[]));

                formatter.Serialize(fs, streetsArray);
            }

            PrintMessage("\nSerialize was successful\n", ConsoleColor.Green);
        }

        /// <summary>
        /// Serialize.
        /// </summary>
        /// <param name="streetsArray"> Array of streets </param>
        private static void Serialize(IEnumerable<Street> streetsArray)
        {
            try
            {
                SerializeStreets(streetsArray);
            }
            catch (SerializationException)
            {
                PrintMessage("Deserialize error!\n", ConsoleColor.Red);
            }
            catch (IOException)
            {
                PrintMessage("Some problems with file!", ConsoleColor.Red);
            }
            catch (SecurityException)
            {
                PrintMessage("Access error!", ConsoleColor.Red);
            }
            catch (Exception)
            {
                PrintMessage("Unexpected error!", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Get array of random streets.
        /// </summary>
        /// <param name="streets"> Array of streets </param>
        /// <param name="n"> Amount of streets </param>
        private static void GetRandomStreets(ref Street[] streets, int n)
        {
            for (var i = 0; i < n; i++)
            {
                streets[i] = GetRandomStreet();
            }
        }

        /// <summary>
        /// Fill array of streets.
        /// </summary>
        /// <param name="streetsArray"> Array of streets </param>
        /// <param name="n"> Amount of streets </param>
        private static void FillStreets(ref Street[] streetsArray, int n)
        {
            try
            {
                ReadFromFile(ref streetsArray, n);
            }
            catch (IOException)
            {
                GetRandomStreets(ref streetsArray, n);
            }
            catch (SecurityException)
            {
                GetRandomStreets(ref streetsArray, n);
            }
            catch (Exception)
            {
                GetRandomStreets(ref streetsArray, n);
            }
        }

        /// <summary>
        /// Read info from file.
        /// </summary>
        /// <param name="streetsArray"> Array of streets </param>
        /// <param name="n"> Amount of streets </param>
        private static void ReadFromFile(ref Street[] streetsArray, int n)
        {
            var streetCounter = 0;
            using (var sr =
                new StreamReader(new FileStream(PathToInput, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                string lineInfo;

                while ((lineInfo = sr.ReadLine()) != null && streetCounter < n)
                {
                    streetsArray[streetCounter++] = GetStreet(lineInfo);
                }
            }
        }

        private static void Main()
        {
            var n = GetNumber<int>("Enter amount of houses: ", el => el > 0);
            var streetsArray = new Street[n];

            FillStreets(ref streetsArray, n);

            PrintMessage("\nStreets:\n\n");

            foreach (var street in streetsArray)
            {
                PrintMessage($"{street}\n", ConsoleColor.Yellow);
            }

            Serialize(streetsArray);

            PrintMessage("\nPress ESC to exit...", ConsoleColor.Green);
            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }
    }
}
