using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Tools
{
    public class ToolBox
    {
        static List<Tuple<double, string>> fractionList = new List<Tuple<double, string>>();


        public static String findClosestFraction(double dec){
            if (fractionList == null || fractionList.Count < 1)
            {
                fractionList = new List<Tuple<double, string>>();
                generateFractionList();
            }
            int i = 0;
            while (dec > fractionList.ElementAt(i).Item1)
            {
                i++;
            }
            if (i - 1 >= 0)
            {
                double distd = dec - fractionList.ElementAt(i - 1).Item1;
                double distu = fractionList.ElementAt(i).Item1 - dec;
                if (distd <= distu)
                {
                    return fractionList.ElementAt(i - 1).Item2;
                }
                else
                {
                    return fractionList.ElementAt(i).Item2;
                }
            }
            else
            {
                return fractionList.ElementAt(0).Item2;
            }
        }

        public static void generateFractionList()
        {
            fractionList = new List<Tuple<double, string>>();
            List<double> done = new List<double>();
            fractionList.Add(new Tuple<double,string>(0,"0"));
            for (double denom = 1; denom <= 64; denom++)
            {
                for (double num = 1; num <= denom; num++)
                {
                    if (!done.Contains(num/denom))
                    {
                        fractionList.Add(new Tuple<double, string>(num / denom, num + "/" + denom));
                        done.Add(num/denom);
                    }
                }
            }
            fractionList.Add(new Tuple<double, string>(1, "1"));
            fractionList.Sort();
        }


        public static String DecimalToFraction(double dec)
        {
            double decimals = dec % 1;
            int whole = (int)(dec - decimals);
            if (decimals > 0)
            {
                string fraction = findClosestFraction(decimals);
                if (fraction.Equals("1") || fraction.Equals("0"))
                {
                    whole = whole + int.Parse(fraction);
                    return "" + whole;
                }
                if (whole > 0)
                {
                    return whole + " " + fraction;
                }
                else
                {
                    return fraction;
                }
            }
            else
            {
                return "" + whole;
            }
        }

        public static String simplifyFraction(double dec){
            string str = dec.ToString();
            if (str.Contains('.'))
            {
                String[] parts = str.Split('.');
                long whole = long.Parse(parts[0]);
                long numerator = long.Parse(parts[1]);
                long denominator = (long)Math.Pow(10, parts[1].Length);
                long divisor = GCD(numerator, denominator);
                long num = numerator / divisor;
                long den = denominator / divisor;
                
                String fraction = num + "/" + den;
                if (whole > 0)
                {
                    return whole + " " + fraction;
                }
                else
                {
                    return fraction;
                }
            }
            else
            {
                return str;
            }
        }

        public static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

    }
}
