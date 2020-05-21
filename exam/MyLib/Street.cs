using System;
using System.Linq;

namespace MyLib
{
    [Serializable]
    public class Street
    {
        public Street() { }

        public string Name { get; set; }
        public int[] Houses { get; set; }

        // Constructor.
        public Street(string name, int[] houses)
        {
            Name = name;
            Houses = houses;
        }

        /// <summary>
        /// Overloading operator ~.
        /// </summary>
        /// <param name="street"> Street </param>
        /// <returns> Amount of houses </returns>
        public static int operator ~(Street street)
        {
            if (street is null)
            {
                throw new ArgumentNullException();
            }

            return street.Houses.Length;
        }

        /// <summary>
        /// Overloading operator !.
        /// </summary>
        /// <param name="street"> Street </param>
        /// <returns> True, if any houses number contain 7,
        /// else - false </returns>
        public static bool operator !(Street street) => street.Houses.Any(IsContain7);

        public override string ToString() =>
            Houses.Aggregate(Name + " ", (current, next) => current + " " + next);

        /// <summary>
        /// Check house number for digit 7.
        /// </summary>
        /// <param name="houseNumber"> House number </param>
        /// <returns> True, if house number contains 7,
        /// else - false </returns> 
        private static bool IsContain7(int houseNumber)
        {
            while (houseNumber > 0)
            {
                var lastDigit = houseNumber % 10;
                houseNumber /= 10;

                if (lastDigit % 10 == 7)
                {
                    return true;
                }
            }

            return false;
        }
    }
}