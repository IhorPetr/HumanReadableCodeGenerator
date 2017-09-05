using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUMANREADABLECODE.Algoritm
{
    public class Generator
    {
        private static char[] soglas =
            "BCDFGHKLMNPQRSTVWXZ"
                .ToCharArray();
        private static char[] glas =
            "AEIJOUY"
                .ToCharArray();

        private static Random _random = new Random();

        public static string GetBase36()
        {
            var sb = new StringBuilder(6);
            var u = _random.Next(2);
            for (int i = 0; i < 6; i++)
            {
                sb.Append(u%2==0 ? soglas[_random.Next(19)] : glas[_random.Next(7)]);
                u++;
            }

            return sb.ToString();
        }
    }
}
