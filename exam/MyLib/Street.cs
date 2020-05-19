using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    [Serializable]
    public class Street
    {
        public Street() { }


        public string name;
        public int[] houses;

        public Street(string name, int[] houses)
        {
            this.name = name;
            this.houses = houses;
        }

        public static int operator ~(Street street)
        {
            if (street is null)
            {
                throw new ArgumentNullException();
            }

            return street.houses.Length;
        }

        public static bool operator !(Street street) => street.houses.Any(IsContain7);

        public override string ToString() =>
            houses.Aggregate(name + " ", (current, next) => current + " " + next);

        private static bool IsContain7(int number)
        {
            while (number > 0)
            {
                var lastDigit = number % 10;
                number /= 10;

                if (lastDigit % 10 == 7)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
