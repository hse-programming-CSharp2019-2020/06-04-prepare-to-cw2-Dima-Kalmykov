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

namespace Program_2
{
    class Program
    {
        // Path to file.
        private const string Path = @"..\..\..\out.txt";

        /// <summary>
        /// Deserialize objects.
        /// </summary>
        private static void DeserializeObjects()
        {
            Street[] streetArray;

            // Deserialization.
            using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var formatter = new XmlSerializer(typeof(Street[]));

                streetArray = (Street[])formatter.Deserialize(fs);
            }

            PrintMessage("Deserialization was successful\n\n", ConsoleColor.Green);

            // Select suitable streets.
            streetArray = streetArray
                .Where(s => s != null && !s)
                .ToArray();

            // Print info.
            PrintMessage("Fantastic streets:\n\n");
            foreach (var street in streetArray)
            {
                PrintMessage($"{street}\n", ConsoleColor.Yellow);
            }
        }

        /// <summary>
        /// Deserialize.
        /// </summary>
        private static void Deserialize()
        {
            // Attempt to deserialize.
            try
            {
                DeserializeObjects();
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

        private static void Main()
        {
            Deserialize();

            PrintMessage("\nPress ESC to exit...", ConsoleColor.Green);
            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }
    }

}

