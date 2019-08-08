using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChartParse.Models
{
    class Parser
    {

        public void wr(string p)
        {
        }

        public double resolveFun(string name, string param = "")
        {

            if (name.Equals("pi"))
                return Math.PI;
            else if (name.Equals("e"))
                return Math.E;
            else if (name.Equals("abs"))
                return Math.Abs(Double.Parse(param));
            else if (name.Equals("sqrt"))
            {
                if (Double.Parse(param) < 0) return 0;
                return Math.Sqrt(Double.Parse(param));
            }
            else if (name.Equals("sin"))
                return Math.Sin(Double.Parse(param));
            else if (name.Equals("cos"))
                return Math.Cos(Double.Parse(param));
            else if (name.Equals("tg"))
                return Math.Tan(Double.Parse(param));
            else if (name.Equals("ctg"))
            {
                if (Math.Tan(Double.Parse(param)) == 0) return 0;
                return 1 / Math.Tan(Double.Parse(param));
            }

            return 0;
        }

        public double parse(string exp)
        {

            exp = exp.ToLower();
            exp.Replace('.', ',');

            double result1 = 0, result2 = 0, result3 = 0;
            int prior = 0;
            char lastOperation1 = '0', lastOperation2 = '0', lastOperation3 = '0';
            string subexp = "";
            string subfun = "";
            string bracketExp = "";

            int bracketCount = 0;
            bool isBracket = false;

            for (int i = 0; i < exp.Length; i++)
            {
                if (isBracket)
                {
                    if (exp[i] == '(')
                        bracketCount++;
                    else if (exp[i] == ')')
                    {
                        if (bracketCount > 0)

                            bracketCount--;
                        else
                        {
                            subexp = "" + (parse(bracketExp));
                            bracketExp = "";
                            isBracket = false;
                            if (!subfun.Equals(""))
                            {
                                subexp = "" + resolveFun(subfun, subexp);
                            }
                            subfun = "";
                        }

                    }
                    if (isBracket)
                        bracketExp += exp[i];


                }
                else
                {




                    switch (exp[i])
                    {
                        case '+':
                        case '-':
                            {

                                if (!subfun.Equals(""))
                                    subexp = "" + resolveFun(subfun);
                                subfun = "";


                                if (prior == 1)
                                {
                                    if (lastOperation1 == '+')
                                        result1 += Double.Parse(subexp);
                                    else if (lastOperation1 == '-')
                                        result1 -= Double.Parse(subexp);

                                }
                                else if (prior == 2)
                                {
                                    if (lastOperation2 == '*')
                                        result1 *= Double.Parse(subexp);
                                    else if (lastOperation2 == '/')
                                    {
                                        if (Double.Parse(subexp) == 0) return 0;
                                        result1 /= Double.Parse(subexp);
                                    }
                                    if (lastOperation1 == '+')
                                        result1 += result2;
                                    else if (lastOperation1 == '-')
                                    {
                                        result2 -= result1;
                                        result1 = result2;
                                    }
                                }
                                else if (prior == 3)
                                {


                                    if (lastOperation3 == '^')
                                        result1 = Math.Pow(result1, Double.Parse(subexp));
                                    if (lastOperation2 == '*')
                                        result1 *= result2;
                                    else if (lastOperation2 == '/')
                                        result1 = result2 / result1;
                                    if (lastOperation1 == '+')
                                        result1 += result3;
                                    else if (lastOperation1 == '-')
                                        result1 = result3 - result1;
                                    result3 = 0;
                                    result2 = 0;

                                }

                                else
                                {
                                    if (subexp.Equals(""))
                                        result1 = 0;
                                    else
                                        result1 = Double.Parse(subexp);
                                }

                                subexp = "";
                                prior = 1;
                                lastOperation1 = exp[i];
                                break;

                            }
                        case '*':
                        case '/':
                            {

                                if (!subfun.Equals(""))
                                    subexp = "" + resolveFun(subfun);
                                subfun = "";
                                if (prior == 1)
                                {
                                    result2 = result1;
                                    result1 = Double.Parse(subexp);
                                }
                                else if (prior == 2)
                                {
                                    if (lastOperation2 == '*')
                                        result1 *= Double.Parse(subexp);
                                    else if (lastOperation2 == '/')
                                    {
                                        if (Double.Parse(subexp) == 0) return 0;
                                        result1 /= Double.Parse(subexp);
                                    }

                                }

                                else if (prior == 3)
                                {
                                    if (lastOperation3 == '^')
                                        result1 = Math.Pow(result1, Double.Parse(subexp));
                                    if (lastOperation2 == '*')
                                        result1 *= result2;
                                    else if (lastOperation2 == '/')
                                    {
                                        if (result1 == 0) return 0;
                                        result1 = result2 / result1;
                                    }

                                }

                                else
                                {

                                    result1 = Double.Parse(subexp);

                                }



                                prior = 2;
                                subexp = "";
                                lastOperation2 = exp[i];
                                break;
                            }

                        case '^':
                            {

                                if (!subfun.Equals(""))
                                    subexp = "" + resolveFun(subfun);
                                subfun = "";
                                wr(subexp + subfun);
                                wr("" + exp[i]);



                                if (prior == 1)
                                {
                                    result3 = result1;
                                    result2 = result1;
                                    result1 = Double.Parse(subexp);
                                }
                                else if (prior == 2)
                                {

                                    result3 = result2;
                                    result2 = result1;
                                    result1 = Double.Parse(subexp);
                                }
                                else if (prior == 3)
                                {
                                    if (lastOperation3 == '^')
                                        result1 = Math.Pow(result1, Double.Parse(subexp));

                                }

                                else
                                {
                                    result1 = Double.Parse(subexp);
                                }
                                prior = 3;
                                subexp = "";
                                lastOperation3 = '^';


                                break;
                            }


                        default:
                            {
                                if ((exp[i] >= '0' && exp[i] <= '9') || exp[i] == ',')
                                {
                                    subexp += exp[i];

                                }
                                else if (exp[i] >= 'a' && exp[i] <= 'z')
                                {
                                    subfun += exp[i];
                                }

                                else if (exp[i] == '(')
                                    isBracket = true;

                                break;
                            }
                    }
                }
            }


            if (!subfun.Equals(""))
                subexp = "" + resolveFun(subfun);
            subfun = "";


            if (prior == 1)
            {

                if (lastOperation1 == '+')
                    result1 += Double.Parse(subexp);
                else if (lastOperation1 == '-')
                    result1 -= Double.Parse(subexp);
            }
            else if (prior == 2)
            {
                if (lastOperation2 == '*')
                    result1 *= Double.Parse(subexp);
                else if (lastOperation2 == '/')
                {
                    if (Double.Parse(subexp) == 0) return 0;
                    result1 /= Double.Parse(subexp);
                }

                if (lastOperation1 == '+')
                    result1 += result2;
                else if (lastOperation1 == '-')
                {
                    result2 -= result1;
                    result1 = result2;

                }
            }

            else if (prior == 3)
            {

                if (lastOperation3 == '^')
                    result1 = Math.Pow(result1, Double.Parse(subexp));

                if (lastOperation2 == '*')
                    result1 *= result2;
                else if (lastOperation2 == '/')
                {
                    if (result1 == 0) return 0;
                    result1 = result2 / result1;
                }
                if (lastOperation1 == '+')
                    result1 += result3;
                else if (lastOperation1 == '-')
                    result1 = result3 - result1;



            }

            else
            {
                result1 = Double.Parse(subexp);
            }




            wr(subexp);
            subexp = "";
            wr(" ");
            return result1;
        }


    }
}