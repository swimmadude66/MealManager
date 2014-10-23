using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory.Tools
{
    public class ToolBox
    {
        static SortedSet<Tuple<double, string>> fractionList = new SortedSet<Tuple<double, string>>();


        public static String findClosestFraction(double dec){
            if (fractionList == null || fractionList.Count < 1)
            {
                fractionList = new SortedSet<Tuple<double, string>>();
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
            fractionList = new SortedSet<Tuple<double, string>>();
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

        public static double FractionToDecimal(string fraction)
        {
            if (Regex.IsMatch(fraction, @"^([0-9]+(\.[0-9]+)?)$"))
            {
                return Double.Parse(fraction);
            }
            else if (Regex.IsMatch(fraction, @"^[0-9]+/[0-9]+$"))
            {
                double a = double.Parse(fraction.Substring(0, fraction.IndexOf('/')));
                double b = double.Parse(fraction.Substring(fraction.IndexOf('/') + 1));
                if (b != 0)
                    return a / b;
            }
            else if (Regex.IsMatch(fraction, @"^[0-9]+\s+[0-9]+/[0-9]+$"))
            {
                string[] parts = Regex.Split(fraction, @"\s+");
                double whole = double.Parse(parts[0]);
                fraction = parts[1];
                double a = double.Parse(fraction.Substring(0, fraction.IndexOf('/')));
                double b = double.Parse(fraction.Substring(fraction.IndexOf('/') + 1));
                if (b != 0)
                    return whole + (a / b);
            }
            return -1;
        }

    }
}
