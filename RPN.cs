using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace WebApi
{
    public class Wynik
    {
        //wszystkie dane są wartościami liczbowymi
        public double X;
        public double Y;
    }
    public class WynikError
    {
        /*
         * przed jakimkolwiek przetwarzaniem, zestaw wyników Y musi być sprawdzony 
         * pole Y może zawierać informacje o próbie podzielenia przez 0 czy innych błędach
         * możliwość "odzysku" poprawnej części poprzez parsowanie string -> double
         */
        public double X;
        public string Y;
    }
    public class RPNclass
    {
        public static string EchoTest = "Echo!";
        public string ErrorLog="No issues";
        public string InfixTokensString = "";
        public string PostfixTokensString = "";
        public string[] InfixTokensArray;
        public string[] PostfixTokensArray;

        
    public RPNclass(string input)
        {
            string tmpString = InputCheck(input);
            if (tmpString[0] == 'E' && tmpString[1] == 'r' && tmpString[2] == 'r') {  ErrorLog = tmpString; return; }
            tmpString = InfixTokens(tmpString);
            if (tmpString[0] == 'E' && tmpString[1] == 'r' && tmpString[2] == 'r') {  ErrorLog = tmpString;return; }
            InfixTokensString = tmpString;
            InfixTokensArray = new string[InfixTokensCount(InfixTokensString)];
            InfixTokensArray = SplitInfixTokens(InfixTokensString, InfixTokensCount(InfixTokensString));
            PostfixTokensString = InfixToPostfix(InfixTokensArray);
            PostfixTokensArray = new string[PostfixTokensCount(PostfixTokensString)];
            PostfixTokensArray = SplitPostfixTokens(PostfixTokensString, PostfixTokensCount(PostfixTokensString));
        }

        public static string InputCheck(string input)
        {
            int DlugoscInput = input.Length;
            string Tekst = "";
            for (int i = 0; i < DlugoscInput; i++)
            {
                if (input[i] != ' ') Tekst += input[i];
            }
            string Wynik = "";
            int Dlugosc = Tekst.Length;
            int Nawias = 0;
            bool Kropka = false;
            for (int i = 0; i < Dlugosc; i++)
            {
                if (Nawias < 0) return "Error, unmatched ( ) symbols.";
                if (i == 0)
                {
                    if (Tekst[i] == '-')
                    {
                        Wynik += "(0-1)*";
                        continue;
                    }
                    if (Tekst[i] == '+') continue;
                    else if (Tekst[i] == '0' || Tekst[i] == '1' || Tekst[i] == '2' || Tekst[i] == '3' || Tekst[i] == '4' || Tekst[i] == '5'
                       || Tekst[i] == '6' || Tekst[i] == '7' || Tekst[i] == '8' || Tekst[i] == '9' || Tekst[i] == 'x') { Wynik += Tekst[i]; continue; }
                    else if (Tekst[i] == '(') { Nawias++; Wynik += '('; continue; }
                    else if (Tekst.Length - i >= 6)
                    {
                        if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "sin(") { Wynik += "sin("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "cos(") { Wynik += "cos("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "tan(") { Wynik += "tan("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "exp(") { Wynik += "exp("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "log(") { Wynik += "log("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "abs(") { Wynik += "abs("; i += 3; Nawias++; continue; }
                        else if (Tekst.Length - i >= 7)
                        {
                            if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "sqrt(") { Wynik += "sqrt("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "sinh(") { Wynik += "sinh("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "cosh(") { Wynik += "cosh("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "tanh(") { Wynik += "tanh("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "asin(") { Wynik += "asin("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "acos(") { Wynik += "acos("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "atan(") { Wynik += "atan("; Nawias++; i += 4; continue; }
                        }
                    }
                    Wynik = "Error, unknown symbol at beggining."; break; 
                }
                else
                {
                    if ((Tekst[i] == '0' || Tekst[i] == '1' || Tekst[i] == '2' || Tekst[i] == '3' || Tekst[i] == '4' || Tekst[i] == '5'
                       || Tekst[i] == '6' || Tekst[i] == '7' || Tekst[i] == '8' || Tekst[i] == '9' || Tekst[i] == 'x') && Tekst[i - 1] == 'x') { Wynik = "Error. Number after X."; break; }
                    else if (Tekst[i] == 'x' && (Tekst[i - 1] == '0' || Tekst[i - 1] == '1' || Tekst[i - 1] == '2' || Tekst[i - 1] == '3' || Tekst[i - 1] == '4' ||
                        Tekst[i - 1] == '5' || Tekst[i - 1] == '6' || Tekst[i - 1] == '7' || Tekst[i - 1] == '8' || Tekst[i - 1] == '9')) {
                        Wynik += "*x";
                        continue;
                    }
                    else if (Tekst[i] == '0' || Tekst[i] == '1' || Tekst[i] == '2' || Tekst[i] == '3' || Tekst[i] == '4' || Tekst[i] == '5'
                       || Tekst[i] == '6' || Tekst[i] == '7' || Tekst[i] == '8' || Tekst[i] == '9' || Tekst[i] == 'x') { Wynik += Tekst[i]; continue; }
                    else if (Tekst[i] == '.' || Tekst[i] == ',')
                    {
                        if (i == Tekst.Length - 1) { Wynik = "Error. Dot/comma at end."; break; }
                        else
                        {
                            if ((Tekst[i - 1] == '0' || Tekst[i - 1] == '1' || Tekst[i - 1] == '2' || Tekst[i - 1] == '3' || Tekst[i - 1] == '4' || Tekst[i - 1] == '5'
                       || Tekst[i - 1] == '6' || Tekst[i - 1] == '7' || Tekst[i - 1] == '8' || Tekst[i - 1] == '9') && (Tekst[i + 1] == '0' || Tekst[i + 1] == '1' || Tekst[i + 1] == '2' || Tekst[i + 1] == '3' || Tekst[i + 1] == '4' || Tekst[i + 1] == '5'
                       || Tekst[i + 1] == '6' || Tekst[i + 1] == '7' || Tekst[i + 1] == '8' || Tekst[i + 1] == '9')) {
                                if (Kropka == false) { Wynik += ","; Kropka = true; continue; }
                                else { Wynik = "Error. Multiple dots/commas in expected double. "; break; }
                            }
                            else { Wynik = "Error. Dot/comma not inside of number."; break; }
                        }
                    }
                    // v jeśli żadne z powyższych, liczba się skończyła i będzie jakiś znak v
                    Kropka = false; // w tej liczbie fizycznie nie wystąpi przecinek po raz drugi
                    if (Tekst[i] == '-' && (Tekst[i - 1] == '*' || Tekst[i - 1] == '/' || Tekst[i - 1] == '('))
                    {
                        Wynik += "(0-1)*";
                        continue;
                    }
                    else if ((Tekst[i] == '-' || Tekst[i] == '+') && (Tekst[i - 1] == '-' || Tekst[i - 1] == '+')) {
                        Wynik = "Error, repeated operation token.";
                        break;
                    }
                    else if ((Tekst[i] == '*' || Tekst[i] == '/' || Tekst[i] == '^') && (Tekst[i - 1] == '-' || Tekst[i - 1] == '+' || Tekst[i - 1] == '^'))
                    {
                        Wynik = "Error, repeated operation token.";
                        break;
                    }
                    else if (Tekst[i] == '+' && (Tekst[i - 1] == '*' || Tekst[i - 1] == '/' || Tekst[i - 1] == '('))
                    {
                        continue;
                    }
                    else if (Tekst[i] == '+' || Tekst[i] == '-' || Tekst[i] == '*' || Tekst[i] == '/' || Tekst[i] == '^') { Wynik += Tekst[i]; continue; }
                    else if (Tekst[i] == '(') { Nawias++; Wynik += Tekst[i]; continue; }
                    else if (Tekst[i] == ')') { Nawias--; Wynik += Tekst[i]; continue; }
                    else if (Tekst.Length - i >= 6)
                    {
                        if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "sin(") { Wynik += "sin("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "cos(") { Wynik += "cos("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "tan(") { Wynik += "tan("; i += 3; Nawias++; continue; }

                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "exp(") { Wynik += "exp("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "log(") { Wynik += "log("; i += 3; Nawias++; continue; }
                        else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] == "abs(") { Wynik += "abs("; i += 3; Nawias++; continue; }
                        else if (Tekst.Length - i >= 7)
                        {
                            if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "sqrt(") { Wynik += "sqrt("; Nawias++; i += 4; continue; }

                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "sinh(") { Wynik += "sinh("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "cosh(") { Wynik += "cosh("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "tanh(") { Wynik += "tanh("; Nawias++; i += 4; continue; }

                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "asin(") { Wynik += "asin("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "acos(") { Wynik += "acos("; Nawias++; i += 4; continue; }
                            else if ("" + Tekst[i] + Tekst[i + 1] + Tekst[i + 2] + Tekst[i + 3] + Tekst[i + 4] == "atan(") { Wynik += "atan("; Nawias++; i += 4; continue; }
                            else
                            {
                                Wynik = "Error. Unknown symbol: " + Tekst[i];
                                break;
                            }
                        }
                        else {
                            Wynik = "Error. Unknown symbol: " + Tekst[i];
                            break;
                        }
                    }
                    else
                    {
                        Wynik = "Error. Unknown symbol." + Tekst[i];
                        break;
                    }
                }

            }
            return Wynik;
        }
        public static string InfixTokens(string input)
        {
            string InputText;
            InputText = InputCheck(input);
            if (InputText[0] == 'E' && InputText[1] == 'r' && InputText[2] == 'r') return InputText;
            string Result = "";
            for (int i = 0; i < InputText.Length; i++)
            {
                if (InputText[i] == '(' && InputText[i + 1] == '0' && InputText[i + 2] == '-' && InputText[i + 3] == '1' && InputText[i + 4] == ')' && InputText[i + 5] == '*') { Result += '-'; i += 5; continue; }
                else if (InputText[i] == '(' || InputText[i] == ')' || InputText[i] == '+' || InputText[i] == '-' || InputText[i] == '*' || InputText[i] == '/' || InputText[i] == '^') { Result += (" " + InputText[i] + " "); continue; }
                else if (InputText[i] == '0' || InputText[i] == '1' || InputText[i] == '2' || InputText[i] == '3' || InputText[i] == '4' || InputText[i] == '5' || InputText[i] == '6' || InputText[i] == '7' ||
                    InputText[i] == '8' || InputText[i] == '9' || InputText[i] == 'x' || InputText[i] == ',') { Result += InputText[i]; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "sin(") { Result += "sin"; i += 2; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "cos(") { Result += "cos"; i += 2; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "tng(") { Result += "cos"; i += 2; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "log(") { Result += "log"; i += 2; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "abs(") { Result += "abs"; i += 2; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "exp(") { Result += "exp"; i += 2; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "sqrt") { Result += "sqrt"; i += 3; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "asin") { Result += "asin"; i += 3; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "acos") { Result += "acos"; i += 3; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "atan") { Result += "atan"; i += 3; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "sinh") { Result += "sinh"; i += 3; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "cosh") { Result += "cosh"; i += 3; continue; }
                else if ("" + InputText[i] + InputText[i + 1] + InputText[i + 2] + InputText[i + 3] == "tanh") { Result += "tanh"; i += 3; continue; }
                else { Result = "Error. Unknown: " + InputText[i]; return Result; }
            }
            return Result;
        }
        public static int InfixTokensCount(string input)
        {
            int TabCount = 1;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (i == 0) continue;
                    if (i == input.Length - 1) continue;
                    if (input[i + 1] == ' ') continue;
                    TabCount++;
                }
            }
            return TabCount;
        }
        public static string[] SplitInfixTokens(string input, int TabCount)
        {
            string[] tab = new string[TabCount];
            int TokensInArray = 0;
            string tmpString = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0 && input[i] == ' ') continue;
                if (i == input.Length-1 && input[i] == ' ') continue;
                if (input[i] == ' ')
                {
                    if (input[i - 1] == ' ') continue;
                    tab[TokensInArray] = tmpString;
                    TokensInArray++;
                    tmpString = "";
                    continue;
                }
                tmpString += input[i];
            }
            tab[TokensInArray] = tmpString;
            TokensInArray++;
            return tab;
        }
        public static string InfixToPostfix(string[] t)
        {
            string Result= "";
            Queue Q = new Queue();
            Stack S = new Stack();
            Dictionary<string, int> D = new Dictionary<string, int>
            {
                {"abs",4 },{"log",4 },{"exp",4 },{"-abs",4 },{"-log",4 },{"-exp",4 },
                {"sin",4 },{"cos",4 },{"tan",4 },{"-sin",4 },{"-cos",4 },{"-tan",4 },
                {"asin",4 },{"acos",4 },{"atan",4 },{"-asin",4 },{"-acos",4 },{"-atan",4 },
                {"sinh",4 },{"cosh",4 },{"tanh",4 },{"-sinh",4 },{"-cosh",4 },{"-tanh",4 },
                {"sqrt",4 },{"-sqrt",4},{"^",3},{"*",2},{"/",2},{"+",1},{"-",1 },{"(",0}
            };
            for(int i = 0; i < t.Length; i++)
            {
                if (t[i] == "(") S.Push(t[i]);
                else if (t[i] == ")")
                {
                    while (S.Peek().ToString() != "(")
                    {
                        Q.Enqueue(S.Pop());
                    }
                    S.Pop();
                }
                else if (D.ContainsKey(t[i]))
                {
                    while (S.Count>0 && D[t[i]]<=D[S.Peek().ToString()])
                    {
                        Q.Enqueue(S.Pop());
                    }
                    S.Push(t[i]);
                }
                else Q.Enqueue(t[i]);
            }
            while (S.Count > 0)
            {
                Q.Enqueue(S.Pop());
            }
            foreach (string token in Q)
            {
                Result += token + " ";
            }
            return Result;
        }
        public static int PostfixTokensCount(string input)
        {
            int TabCount = 1;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (i == 0 || i == input.Length - 1) continue;
                    TabCount++;
                }
            }
            return TabCount;
        }
        public static string[] SplitPostfixTokens(string input, int TabCount)
        {
            string[] Tab = new string[TabCount];
            int TokensInArray = 0;
            string tmpString = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    Tab[TokensInArray] = tmpString;
                    TokensInArray++;
                    tmpString = "";
                    continue;
                }
                tmpString += input[i];
            }
            return Tab;
        }
        public static string PostfixCalcSingleX(string[] input, double X) {
            Stack S = new Stack();
            double tmpDoub;
            double tmpA;
            double tmpB;
            foreach (string t in input)
            {
                if (double.TryParse(t, out double doubleT) == true)
                {
                    S.Push(doubleT);
                    continue;
                }
                if(t == "x")
                {
                    S.Push(X);
                }
                if (t == "abs") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(Math.Abs(tmpDoub).ToString()); }
                else if (t == "-abs") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1)*Math.Abs(tmpDoub)).ToString()); }
                else if(t == "exp") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(Math.Exp(tmpDoub).ToString()); }
                else if (t == "-exp") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Exp(tmpDoub)).ToString()); }
                else if (t == "log") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub <= 0) return "Error. X cannot be less or equal 0";
                    S.Push(Math.Log(tmpDoub).ToString());
                }
                else if (t == "-log") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub <= 0) return "Error. X cannot be less or equal 0";
                    S.Push(((-1) * Math.Log(tmpDoub)).ToString());
                }
                else if (t == "sqrt") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub < 0) return "Error. X cannot be less than 0";
                    S.Push(Math.Abs(tmpDoub).ToString());
                }
                else if (t == "-sqrt") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub < 0) return "Error. X cannot be less than 0";
                    S.Push(((-1) * Math.Abs(tmpDoub)).ToString());
                }
                /*
                 * wartości PI/2 +kPI są poza dziedziną tangensa. Niemożliwe jest jednak precyzyjne
                 * podanie takich wartości dla programu ze względu na naturę liczby PI
                 */
                else if (t == "tan") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(Math.Tan(tmpDoub).ToString()); }
                else if (t == "-tan") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Tan(tmpDoub)).ToString()); }
                else if (t == "sin") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push((Math.Sin(tmpDoub)).ToString()); }
                else if (t == "-sin") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Sin(tmpDoub)).ToString()); }
                else if (t == "cos") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push((Math.Cos(tmpDoub)).ToString()); }
                else if (t == "-cos") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Cos(tmpDoub)).ToString()); }
                else if (t == "asin") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub < -1.0 || tmpDoub > 1.0) return "Error. X cannot be  less than (-1) or greater than 1.";
                    S.Push(Math.Asin(tmpDoub).ToString());
                }
                else if (t == "-asin") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub < -1.0 || tmpDoub > 1.0) return "Error. X cannot be  less than (-1) or greater than 1.";
                    S.Push(((-1) * Math.Asin(tmpDoub)).ToString());
                }
                else if (t == "acos") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub < -1.0 || tmpDoub > 1.0) return "Error. X cannot be  less than (-1) or greater than 1.";
                    S.Push((Math.Acos(tmpDoub)).ToString());
                }
                else if (t == "-acos") {
                    tmpDoub = double.Parse(S.Pop().ToString());
                    if (tmpDoub < -1.0 || tmpDoub > 1.0) return "Error. X cannot be  less than (-1) or greater than 1.";
                    S.Push(((-1) * Math.Acos(tmpDoub)).ToString());
                }
                else if (t == "atan") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push((Math.Atan(tmpDoub)).ToString()); }
                else if (t == "-atan") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Atan(tmpDoub)).ToString()); }

                else if (t == "sinh") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push((Math.Sinh(tmpDoub)).ToString()); }
                else if (t == "-sinh") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Sinh(tmpDoub)).ToString()); }
                else if (t == "cosh") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push((Math.Cosh(tmpDoub)).ToString()); }
                else if (t == "-cosh") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Cosh(tmpDoub)).ToString()); }
                else if (t == "tanh") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push((Math.Tanh(tmpDoub)).ToString()); }
                else if (t == "-tanh") { tmpDoub = double.Parse(S.Pop().ToString()); S.Push(((-1) * Math.Tanh(tmpDoub)).ToString()); }
                else if (t == "+")
                {
                    tmpA = double.Parse(S.Pop().ToString());
                    tmpB = double.Parse(S.Pop().ToString());
                    tmpA = tmpA + tmpB;
                    S.Push(tmpA.ToString());
                }
                else if (t == "-")
                {
                    tmpA = double.Parse(S.Pop().ToString());
                    tmpB = double.Parse(S.Pop().ToString());
                    tmpA = tmpB - tmpA;
                    S.Push(tmpA.ToString());
                }
                else if (t == "*")
                {
                    tmpA = double.Parse(S.Pop().ToString());
                    tmpB = double.Parse(S.Pop().ToString());
                    tmpA = tmpA * tmpB;
                    S.Push(tmpA.ToString());
                }
                else if (t == "/")
                {
                    tmpA = double.Parse(S.Pop().ToString());
                    tmpB = double.Parse(S.Pop().ToString());
                    if (tmpA == 0 || tmpA == 0.0) return "Error. Division by 0.";
                    tmpA = tmpB / tmpA;
                    S.Push(tmpA.ToString());
                }
                else if (t == "^")
                {
                    tmpA = double.Parse(S.Pop().ToString());
                    tmpB = double.Parse(S.Pop().ToString());
                    tmpA = Math.Pow(tmpB, tmpA);
                    S.Push(tmpA.ToString());
                }
            }
            return S.Pop().ToString();
        }
        public static string[] PostfixCalcMultiXJSON(string[] input, double X_min, double X_max, int N)
        {
            double Step = (X_max - X_min) / (N - 1);
            string[] Tablica = new string[N];
            string tmpString;
            for (int i = 0; i < N; i++)
            {
                tmpString = PostfixCalcSingleX(input, X_min + (Step * i));
                if (tmpString[0] == 'E' && tmpString[1] == 'r' && tmpString[2] == 'r') Tablica[i] = "{ " + '"' + "x" + '"' + " : " + (input, X_min + (Step * i)) + ", " + '"' + "y" + '"' + " : " + tmpString + " }";
                else Tablica[i] = "{ " + '"' + "x" + '"' + " : " + (input, X_min + (Step * i)) + ", " + '"' + "y" + '"' + " : " + PostfixCalcSingleX(input, X_min + (Step * i)) + " }";
            }
            return Tablica;
        }
        public static Wynik[] PostfixCalcMultiXWynik(string[] input, double X_min, double X_max, int N)
        {
            double Step = (X_max - X_min) / (N - 1);
            Wynik[] Tablica = new Wynik[N];
            for (int i = 0; i < N; i++)
            {
                Tablica[i] = new Wynik();
                Tablica[i].X = (X_min + (Step * i));
                Tablica[i].Y = double.Parse(PostfixCalcSingleX(input, X_min + (Step * i)));
            }
            return Tablica;
        }
        public static WynikError[] PostfixCalcMultiXWynikError(string[] input, double X_min, double X_max, int N)
        {
            double Step = (X_max - X_min) / (N - 1);
            WynikError[] Tablica = new WynikError[N];
            for (int i = 0; i < N; i++)
            {
                Tablica[i] = new WynikError();
                Tablica[i].X = (X_min + (Step * i));
                Tablica[i].Y = PostfixCalcSingleX(input, X_min + (Step * i));
            }
            return Tablica;
        }
        public static string[,] PostfixCalcMultiX(string[] input, double X_min, double X_max, int N)
        {
            double Step = (X_max - X_min) / (N - 1);
            string[,] Tablica = new string[N, 2];
            for (int i = 0; i < N; i++)
            {
                Tablica[i, 0] = (X_min + (Step * i)).ToString();
                Tablica[i, 1] = PostfixCalcSingleX(input, X_min + (Step * i));
            }
            return Tablica;
        }
        public static bool PostfixCalcMultiXCheck(string[] input, double X_min, double X_max, int N)
        {
            double Step = (X_max - X_min) / (N - 1);
            bool FlagError = false;
            string tmpString = "jest ok";
            for (int i = 0; i < N; i++)
            {
                tmpString = PostfixCalcSingleX(input, X_min + (Step * i));
                if (tmpString[0] == 'E' && tmpString[1] == 'r' && tmpString[2] == 'r') { FlagError = true; return FlagError; }
            }
            return FlagError;
        }
    }
}
