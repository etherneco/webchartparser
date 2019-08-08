using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {

        public static string GetWordFromText(string input, int wordNumberToFind)
        {

                if (input == null)
                    throw new ArgumentNullException();
                else
                {

                    List<String> list = new List<string>();
                    string a = "";
                    for (int i = 0; i < input.Length; i++)
                    {
                        if ((input[i] >= 'A' && input[i] <= 'Z') || (input[i] >= 'a' && input[i] <= 'z'))
                        {
                            a += input[i];
                        }
                        else
                        {
                            list.Add(a);
                            a = "";


                        }
                    }
                    if (!a.Equals("")) list.Add(a);

                    if (list.Count() < wordNumberToFind || wordNumberToFind < 1)
                    {
                        throw new ArgumentOutOfRangeException();

                    }
                    else
                    {
                        return list[wordNumberToFind - 1];
                    }


                }

                return string.Empty;
            }


        public string Reverse(string input)
        {

            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);

        }

        static void Main(string[] args)
        {
            Console.WriteLine(GetWordFromText("one two three", 0));
            Console.ReadLine();
        }
    }
}
