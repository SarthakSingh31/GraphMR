using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatexToASCIIMath
{
    public static string covert(string latexStr)
    {
        string fixedString = latexStr.Replace(" ", "");
        fixedString = fixedString.Replace("\\n", "");
        fixedString = fixedString.Replace("\\\\", "\\");
        fixedString = fixedString.Replace("\\[", "");
        fixedString = fixedString.Replace("\\]", "");
        fixedString = fixedString.Replace("\\times", "*");
        fixedString = fixedString.Replace("\\pi", "3.1415927");
        fixedString = fixedString.Replace("\\theta", "\u03B8");

        fixedString = fixedString.Replace("0(", "0*(");
        fixedString = fixedString.Replace("1(", "1*(");
        fixedString = fixedString.Replace("2(", "2*(");
        fixedString = fixedString.Replace("3(", "3*(");
        fixedString = fixedString.Replace("4(", "4*(");
        fixedString = fixedString.Replace("5(", "5*(");
        fixedString = fixedString.Replace("6(", "6*(");
        fixedString = fixedString.Replace("7(", "7*(");
        fixedString = fixedString.Replace("8(", "8*(");
        fixedString = fixedString.Replace("9(", "9*(");

        fixedString = fixedString.Replace(")0", ")*0");
        fixedString = fixedString.Replace(")1", ")*1");
        fixedString = fixedString.Replace(")2", ")*2");
        fixedString = fixedString.Replace(")3", ")*3");
        fixedString = fixedString.Replace(")4", ")*4");
        fixedString = fixedString.Replace(")5", ")*5");
        fixedString = fixedString.Replace(")6", ")*6");
        fixedString = fixedString.Replace(")7", ")*7");
        fixedString = fixedString.Replace(")8", ")*8");
        fixedString = fixedString.Replace(")9", ")*9");

        fixedString = fixedString.Replace("xz", "x*z");
        fixedString = fixedString.Replace("zx", "z*x");

        fixedString = fixedString.Replace("x(", "x*(");
        fixedString = fixedString.Replace("z(", "z*(");

        fixedString = fixedString.Replace(")x", ")*x");
        fixedString = fixedString.Replace(")z", ")*z");

        fixedString = fixedString.Replace(")(", ")*(");

        foreach (char c in "abcdefghijklmnopqrstuvwxyz")
        {
            foreach (char d in "0123456789")
            {
                fixedString = fixedString.Replace(c + "" + d, c + "*" + d);
                fixedString = fixedString.Replace(d + "" + c, d + "*" + c);
            }
        }

        fixedString = fixedString.Replace("--", "(neg");
        fixedString = fixedString.Replace("*-", "*neg");
        fixedString = fixedString.Replace("+-", "+neg");
        fixedString = fixedString.Replace("/-", "/neg");
        fixedString = fixedString.Replace("(-", "(neg");
        fixedString = fixedString.Replace("^-", "(neg");

        fixedString = fixFractions(fixedString);

        fixedString = fixedString.Replace("{", "(");
        fixedString = fixedString.Replace("}", ")");
        return fixedString;
    }

    private static bool checkEquationStringValidity(string eqStr)
    {
        int counterBrackets = 0;
        int counterParantheses = 0;
        foreach (char c in eqStr)
        {
            if (c == '{')
                counterBrackets++;
            if (c == '}')
                counterBrackets--;
            if (c == '(')
                counterParantheses++;
            if (c == ')')
                counterParantheses--;
        }
        return counterBrackets == 0 && counterParantheses == 0;
    }

    private static int endIndexOfSubEq(string subEqStr)
    {
        int counter = 1;
        for (int i = 0; i < subEqStr.Length; i++)
        {
            if (subEqStr[i] == '{')
                counter++;
            if (subEqStr[i] == '}')
                counter--;
            if (counter == 0)
                return i;
        }
        return -1;
    }

    private static string fixFractions(string eqStr)
    {
        for (int index = eqStr.IndexOf("\\frac"); index >= 0; index = eqStr.IndexOf("\\frac"))
        {
            string beforeFrac = eqStr.Substring(0, index);
            string param1 = eqStr.Substring(index + 6, endIndexOfSubEq(eqStr.Substring(index + 6)));
            string param2 = eqStr.Substring(index + 6 + param1.Length + 2, endIndexOfSubEq(eqStr.Substring(index + 6 + param1.Length + 2)));
            string afterFrac = eqStr.Substring(index + 6 + param1.Length + 2 + param2.Length + 1);

            eqStr = beforeFrac + "(" + param1 + ")/(" + param2 + ")" + afterFrac;
        }

        return eqStr;
    }
}
