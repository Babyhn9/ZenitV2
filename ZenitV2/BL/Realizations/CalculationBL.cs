using ZenitV2.BL.Interfaces;
using ZenitV2.Data;
using ZenitV2.Models;
using ZenitV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.BL.Realizations
{
    [Buissnes]
    public class CalculationBL : ICalculationBL
    {

        public CalculationBL()
        {

        }

        public DegreeData Division(DegreeData degree1, int n) => RadianToDegree(DegreeToRadian(degree1) / n);

        public bool IsLess(DegreeData degree1, DegreeData degree2)
        {
            throw new NotImplementedException();
        }

        public bool IsMore(DegreeData degree1, DegreeData degree2)
        {
            throw new NotImplementedException();
        }

        public DegreeData Multiply(DegreeData degree1, int n) => RadianToDegree(DegreeToRadian(degree1) * n);

        public DegreeData Substruct(DegreeData degree1, DegreeData degree2) =>
            RadianToDegree(DegreeToRadian(degree1) - DegreeToRadian(degree2));

        public DegreeData Sum(DegreeData degree1, DegreeData degree2) =>
            RadianToDegree(DegreeToRadian(degree1) + DegreeToRadian(degree2));

        public DegreeData BL90(double x, double y, double z)
        {
            double a1 = 6378137;
            double a2 = 1 / 298.257223563;//298.2564151;

            double e1 = (a2 * 2) - (Math.Pow(a2, 2));
            double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double Br = 0;
            if (r == 0)
            { Br = (Math.PI * z) / (2 * Math.Abs(z)); }
            else if (r > 0)
            {
                if (z == 0)
                {
                    Br = 0;
                }
                else
                {
                    double p1 = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
                    double c = Math.Asin(z / p1);
                    double s1 = 0;
                    double p2 = (e1 * a1) / (2 * p1);
                    double d, b, s2;
                    d = 1;
                    for (int i1 = 0; d > 0.000000001; i1++)
                    {
                        b = c + s1;
                        s2 = Math.Asin((p2 * Math.Sin(2 * b)) / (Math.Sqrt(1 - (e1 * Math.Pow(Math.Sin(b), 2)))));
                        d = Math.Abs(s2 - s1);
                        s1 = s2;
                        if (i1 > 100)
                        { break; }
                        Br = b;
                    }
                }
            }
            return RadianToDegree(Br);
        }

        public double H90(double x, double y, double z)
        {
            double H = 0;
            double a1 = 6378137;
            double a2 = 1 / 298.257223563; // 1 / 298.2564151;
            double e1 = (a2 * 2) - (Math.Pow(a2, 2));
            double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double Br = 0;
            if (r == 0)
            {
                Br = (Math.PI * z) / (2 * Math.Abs(z));
                H = z * Math.Sin(Br) - a1 * Math.Sqrt(1 - e1 * Math.Sin(Br));
            }
            else if (r > 0)
            {
                if (z == 0)
                {
                    H = r - a1;
                }
                else
                {
                    double p1 = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
                    double c = Math.Asin(z / p1);
                    double s1 = 0;
                    double p2 = (e1 * a1) / (2 * p1);
                    double d, b, s2;
                    d = 1;
                    for (int i1 = 0; d > 0.000000001; i1++)
                    {
                        b = c + s1;
                        s2 = Math.Asin((p2 * Math.Sin(2 * b)) / (Math.Sqrt(1 - (e1 * Math.Pow(Math.Sin(b), 2)))));
                        d = Math.Abs(s2 - s1);
                        s1 = s2;
                        if (i1 > 100)
                        {
                            break;
                        }
                        Br = b;
                    }
                    H = r * Math.Cos(Br) + z * Math.Sin(Br) - a1 * (Math.Sqrt(1 - e1 * Math.Pow(Math.Sin(Br), 2)));
                }
            }
            return GauseRound(H, 3);
        }

        public DegreeData L90(double x, double y)
        {
            double Lr = 0;
            //double a2 = 1 / 298.257223563;
            //double e1 = (a2 * 2) - (Math.Pow(a2, 2));
            double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            if (r == 0)
            { Lr = 0; }
            else if (r > 0)
            {
                double la1 = Math.Abs(Math.Asin(y / r));
                if (y < 0 && x > 0)
                { Lr = 2 * Math.PI - la1; }
                else if (y < 0 && x < 0)
                { Lr = Math.PI + la1; }
                else if (y > 0 && x < 0)
                { Lr = Math.PI - la1; }
                else if (y > 0 && x > 0)
                { Lr = la1; }
            }
            return RadianToDegree(Lr);
        }

        public string AngleToAngleM(DegreeData degree)
        {
            double YglD = 0, YglM = 0;
            double min = 0;
            min = degree.Min;
            if (degree.Sec >= 30) { min++; }
            YglD = Math.Truncate(degree.Degree / 6);
            YglM = GauseRound(((degree.Degree - YglM * 60) + min) / 3.6);
            string res = Convert.ToString(YglD) + "-" + Convert.ToString(YglM);
            return res;
        }

        public string DegreeToString(DegreeData degree, int n)
        {
            string res;
            if (degree.Degree == 0 && degree.Min == 0 && degree.Sec == 0) { return ""; }
            else
            {
                if (n == 0)
                {
                    degree.Sec = GauseRound(degree.Sec);
                    res = degree.Degree.ToString() + Constants.DegreeSymbol + String.Format("{0:00}", degree.Min) + '\'' + String.Format("{0:00}", degree.Sec) + '\"';
                }
                else if (n == 1)
                {
                    degree.Sec = GauseRound(degree.Sec, 1);
                    res = degree.Degree.ToString() + Constants.DegreeSymbol + String.Format("{0:00}", degree.Min) + '\'' + String.Format("{0:00.0}", degree.Sec) + '\"';
                }
                else if (n == 2)
                {
                    degree.Sec = GauseRound(degree.Sec, 2);
                    res = degree.Degree.ToString() + Constants.DegreeSymbol + String.Format("{0:00}", degree.Min) + '\'' + String.Format("{0:00.00}", degree.Sec) + '\"';
                }
                else
                {
                    degree.Sec = GauseRound(degree.Sec, 3);
                    res = degree.Degree.ToString() + Constants.DegreeSymbol + String.Format("{0:00}", degree.Min) + '\'' + String.Format("{0:00.000}", degree.Sec) + '\"';
                }
                if (res.Contains("-")) { res = res.Remove('-'); res = "-" + res; }
                if (res.ToUpper().Contains("NAN")) { res = ""; }
                return res;
            }
        }

        public double GauseRound(double x, double n)
        {
            if (n == 0)
            {
                x = Convert.ToDouble(Convert.ToString(x * 10));
                x = Math.Truncate(x);
                x = x / 10;
                x = Math.Round(x, 0, MidpointRounding.ToEven);
            }
            else
            {
                n = Math.Pow(10, n);
                x = Convert.ToDouble(Convert.ToString(x * n * 10));
                x = Math.Truncate(x);
                x = x / 10;
                x = Math.Round(x, 0, MidpointRounding.ToEven);
                x = x / n;
            }
            return x;
        }

        public double GauseRound(double x)
        {
            x = Convert.ToDouble(Convert.ToString(x * 10));
            x = Math.Truncate(x);
            x = x / 10;
            x = Math.Round(x, 0, MidpointRounding.ToEven);
            return x;
        }

        public DegreeData RadianToDegree(double radian)
        {
            var result = new DegreeData
            {
                Degree = (radian * 180) / Math.PI
            };
            result.Min = (result.Degree - Math.Truncate(result.Degree)) * 60;
            result.Sec = (result.Min - Math.Truncate(result.Min)) * 60;
            result.Degree = Math.Truncate(result.Degree);
            result.Min = Math.Truncate(result.Min);
            result.Sec = GauseRound(result.Sec, 3);
            while (result.Sec >= 60)
            {
                result.Min += 1;
                result.Sec -= 60;
            }
            while (result.Sec <= -60)
            {
                result.Min -= 1;
                result.Sec += 60;
            }
            while (result.Min >= 60)
            {
                result.Degree += 1;
                result.Min -= 60;
            }
            while (result.Min <= -60)
            {
                result.Degree -= 1;
                result.Min += 60;
            }

            if (result.Degree < 0) { result.Min = Math.Abs(result.Min); result.Sec = Math.Abs(result.Sec); }
            else if (result.Degree == 0 && result.Min < 0) { result.Sec = Math.Abs(result.Sec); }

            return result;
        }

        public DegreeData CalculateB42(double x, double y, double zone)
        {
            int n = 0;
            try
            {
                n = Convert.ToInt32(zone);
                double b = x / 6367558.4968,
                       B0 = CalculateB0(b),
                       y_ist = CalculateYist(y, n),
                       z0 = CalculateZ0(y_ist, n, B0),
                       Bx = -Math.Pow(z0, 2) * Math.Sin(2 * B0) * (0.251684631 - 0.003369263 * Sin2(B0) + 0.000011276 * Sin4(B0)
                       - Math.Pow(z0, 2) * (0.10500614 - 0.04559916 * Sin2(B0) + 0.00228901 * Sin4(B0)
                       - 0.00002987 * Sin6(B0) - Math.Pow(z0, 2) * (0.042858 - 0.025318 * Sin2(B0) + 0.014346 * Sin4(B0)
                       - 0.001264 * Sin6(B0) - Math.Pow(z0, 2) * (0.01672 - 0.0063 * Sin2(B0) + 0.01188 * Sin4(B0)
                       - 0.00328 * Sin6(B0))))),
                       B_rad = B0 + Bx;
                return RadianToDegree(B_rad);
            }
            catch
            {
                System.Exception ex1 = new Exception()
                {
                    Source = "-1.53856788363",
                };
                throw ex1;
            }
        }

        public DegreeData CalculateL42(double x, double y, double zone)
        {
            int n = 0;
            try
            {
                n = Convert.ToInt32(zone);
                double b = x / 6367558.4968,
                       B0 = CalculateB0(b),
                       y_ist = CalculateYist(y, n),
                       z0 = CalculateZ0(y_ist, n, B0),
                       l = z0 * (1 - 0.0033467108 * Sin2(B0) - 0.0000056002 * Sin4(B0) - 0.0000000187 * Sin6(B0)
                       - Math.Pow(z0, 2) * (0.16778975 + 0.16273586 * Sin2(B0) - 0.0005249 * Sin4(B0) - 0.00000846 * Sin6(B0)
                       - Math.Pow(z0, 2) * (0.0420025 + 0.1487407 * Sin2(B0) + 0.005942 * Sin4(B0) - 0.000015 * Sin6(B0)
                       - Math.Pow(z0, 2) * (0.01225 + 0.09477 * Sin2(B0) + 0.03282 * Sin4(B0) - 0.00034 * Sin6(B0)
                       - Math.Pow(z0, 2) * (0.0038 + 0.0524 * Sin2(B0) + 0.0482 * Sin4(B0) + 0.0032 * Sin6(B0)))))),
                       L_rad = 6 * (n - 0.5) / 57.29577951 + l;
                return RadianToDegree(L_rad);
            }
            catch
            {
                System.Exception ex12 = new Exception()
                {
                    Source = "-1.53856788363",
                };
                throw ex12;
            }
        }

        private double Sin2(double a)
        {
            return Math.Pow(Math.Sin(a), 2);
        }

        private double Sin4(double a)
        {
            return Math.Pow(Math.Sin(a), 4);
        }

        private double Sin6(double a)
        {
            return Math.Pow(Math.Sin(a), 6);
        }

        private double CalculateZ0(double a_Yist, int a_N, double a_B0)
        {
            return (a_Yist - (10 * a_N + 5) * 100000) / (6378245 * Math.Cos(a_B0));
        }
        private double CalculateYist(double a_Y, int a_N)
        {
            return a_Y + (a_N * 1000000);
        }
        private double CalculateB0(double a_B)
        {
            return a_B + Math.Sin(2 * a_B) * (0.00252588685 - 0.0000149186 * Sin2(a_B) + 0.00000011904 * Sin4(a_B));
        }

        public CordinateModel CalculateY(CordinateModel cordinateModel, int zone)
        {
            try
            {
                var L0 = new DegreeData() { Degree = (zone * 6) - 3, Min = 0.0, Sec = 0.0 };
                var L_L0 = Substruct(cordinateModel.L, L0);
                DegreeData temp;

                double l42n = ((cordinateModel.L.Degree * Math.PI) / 180) + ((cordinateModel.L.Min * Math.PI) / (180 * 60)) + ((cordinateModel.L.Sec * Math.PI) / (180 * 3600));
                double l0n = ((L0.Degree * Math.PI) / 180) + ((L0.Min * Math.PI) / (180 * 60)) + ((L0.Sec * Math.PI) / (180 * 3600));
                double L_LOOn = l42n - l0n;


                double sinB, tgY, tgL_L0;
                string L_L0_out = "",
                       tgL_L0_out, sinB_out, tgY_out,
                       Y_out = "";
                bool flag = true;

                temp = L_L0;
                if (L_L0.Degree == 0 && Check(L_L0) == true)
                {
                    L_L0.Min = Math.Abs(L_L0.Min);
                    L_L0.Sec = Math.Abs(L_L0.Sec);
                    L_L0_out = '-' + L_L0.Degree.ToString() + '°' + String.Format("{0:00}", L_L0.Min) + '\'' + String.Format("{0:00.000}", L_L0.Sec);
                    flag = false;
                }
                else
                    if (Check(L_L0) == true)
                {
                    L_L0.Min = Math.Abs(L_L0.Min);
                    L_L0.Sec = Math.Abs(L_L0.Sec);
                }
                if (flag == true)
                {
                    L_L0_out = DegreeToString(L_L0, 3);
                }

                L_L0 = temp;
                flag = true;

                tgL_L0 = GauseRound(Math.Tan(L_LOOn), 6);

                double tgL_L0n = Math.Tan(L_LOOn);

                tgL_L0_out = String.Format("{0:0.000000}", tgL_L0);
                sinB = GauseRound(Math.Sin(DegreeToRadian(cordinateModel.B)), 6);
                double sinBn = Math.Sin(DegreeToRadian(cordinateModel.B));

                sinB_out = String.Format("{0:0.000000}", sinB);
                tgY = GauseRound(tgL_L0n * sinBn, 6);
                double tgYn = tgL_L0n * sinBn;

                tgY_out = String.Format("{0:0.000000}", tgY);
                DegreeData Ynn = RadianToDegree(Math.Atan(tgYn));
                Ynn.Sec = GauseRound(Ynn.Sec, 3);

                if (Ynn.Degree == 0 && Check(Ynn) == true)
                {
                    Ynn.Min = Math.Abs(Ynn.Min);
                    Ynn.Sec = Math.Abs(Ynn.Sec);
                    Y_out = '-' + Ynn.Degree.ToString() + '°' + String.Format("{0:00}", Ynn.Min) + '\'' + String.Format("{0:00.000}", Ynn.Sec);
                    flag = false;
                }
                else
                   if (Check(Ynn) == true)
                {
                    Ynn.Min = Math.Abs(Ynn.Min);
                    Ynn.Sec = Math.Abs(Ynn.Sec);
                }
                if (flag == true)
                {
                    Y_out = DegreeToString(Ynn, 3);
                }


                return cordinateModel;
            }
            catch
            { throw; }
        }

        public double DegreeToRadian(DegreeData degree)
        {
            if (degree.Degree < 0) { degree.Min = -degree.Min; degree.Sec = -degree.Sec; }
            else if (degree.Degree == 0 && degree.Min < 0) { degree.Sec = -degree.Sec; }
            double rad1 = ((degree.Degree * Math.PI) / 180) + ((degree.Min * Math.PI) / (180 * 60)) + ((degree.Sec * Math.PI) / (180 * 3600));
            if (degree.Degree < 0) { degree.Min = Math.Abs(degree.Min); degree.Sec = Math.Abs(degree.Sec); }
            else if (degree.Degree == 0 && degree.Min < 0) { degree.Sec = Math.Abs(degree.Sec); }
            return rad1;
        }

        private bool Check(DegreeData a_Deg)
        {
            if (a_Deg.Min < 0 || a_Deg.Sec < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DegreeData String2Degree(string a_Text)
        {
            DegreeData result = new DegreeData();
            if (a_Text == null)
            {
                DegreeData res1 = new DegreeData() { Degree = 0 };
                return res1;
            }
            if (a_Text.Contains("\""))
            {
                if (a_Text.IndexOf("\"") == a_Text.Length - 1)
                { a_Text = a_Text.Substring(0, a_Text.IndexOf("\"")) + "" + a_Text.Substring((a_Text.IndexOf("\"") + 1)); }
            }

            try
            {
                result.Degree = double.Parse(a_Text.Substring(0, FindDegIndex(a_Text)));
                result.Min = double.Parse(a_Text.Substring(FindDegIndex(a_Text) + 1, FindMinIndex(a_Text) - FindDegIndex(a_Text) - 1));
                result.Sec = double.Parse(a_Text.Substring(FindMinIndex(a_Text) + 1));
            }
            catch
            {

            }
            if (a_Text.Contains("-"))
            {
                if (result.Degree == 0) { result.Min = -Math.Abs(result.Min); }
                if (result.Degree == 0 && result.Min == 0) { result.Sec = -Math.Abs(result.Sec); }
                if (result.Degree != 0) { result.Degree = -Math.Abs(result.Degree); result.Min = Math.Abs(result.Min); result.Sec = Math.Abs(result.Sec); }
            }
            return result;
        }

        private int FindDegIndex(string a_Text)
        {
            int result = 0;

            for (int i = 0; i <= a_Text.Length; i++)
            {
                if (a_Text[i] == '°')
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        private int FindMinIndex(string a_Text)
        {
            int result = 0;

            for (int i = 0; i <= a_Text.Length; i++)
            {
                if (a_Text[i] == '\'')
                {
                    result = i;
                    break;
                }
            }
            return result;
        }




        public AdvancedCordinateModel CalculateY6ZN(CordinateModel model, int zone)
        {
            try
            {
                DegreeData B42 = model.B,
                       L42 = model.L,
                       L0 = new DegreeData() { Degree = (zone * 6) - 3, Min = 0.0, Sec = 0.0 },
                       L_L0 = Substruct(L42, L0),
                       temp;

                double l42n = ((L42.Degree * Math.PI) / 180) + ((L42.Min * Math.PI) / (180 * 60)) + ((L42.Sec * Math.PI) / (180 * 3600));
                double l0n = ((L0.Degree * Math.PI) / 180) + ((L0.Min * Math.PI) / (180 * 60)) + ((L0.Sec * Math.PI) / (180 * 3600));
                double L_LOOn = l42n - l0n;


                double sinB, tgY, tgL_L0;
                string L_L0_out = "",
                       tgL_L0_out, sinB_out, tgY_out,
                       Y_out = "";
                bool flag = true;

                temp = L_L0;
                if (L_L0.Degree == 0 && Check(L_L0) == true)
                {
                    L_L0.Min = Math.Abs(L_L0.Min);
                    L_L0.Sec = Math.Abs(L_L0.Sec);
                    L_L0_out = '-' + L_L0.Degree.ToString() + '°' + String.Format("{0:00}", L_L0.Min) + '\'' + String.Format("{0:00.000}", L_L0.Sec);
                    flag = false;
                }
                else
                    if (Check(L_L0) == true)
                {
                    L_L0.Min = Math.Abs(L_L0.Min);
                    L_L0.Sec = Math.Abs(L_L0.Sec);
                }
                if (flag == true)
                {
                    L_L0_out = DegreeToString(L_L0, 3);
                }

                L_L0 = temp;
                flag = true;

                tgL_L0 = GauseRound(Math.Tan(L_LOOn), 6);

                double tgL_L0n = GauseRound(Math.Tan(L_LOOn), 6);

                tgL_L0_out = String.Format("{0:0.000000}", tgL_L0);
                sinB = GauseRound(Math.Sin(DegreeToRadian(B42)), 6);
                double sinBn = GauseRound(Math.Sin(DegreeToRadian(B42)), 6);

                sinB_out = String.Format("{0:0.000000}", sinB);
                tgY = GauseRound(tgL_L0n * sinBn, 6);
                double tgYn = GauseRound(tgL_L0n * sinBn, 6);

                tgY_out = String.Format("{0:0.000000}", tgY);
                DegreeData Ynn = RadianToDegree(Math.Atan(tgYn));
                Ynn.Sec = GauseRound(Ynn.Sec, 3);

                if (Ynn.Degree == 0 && Check(Ynn) == true)
                {
                    Ynn.Min = Math.Abs(Ynn.Min);
                    Ynn.Sec = Math.Abs(Ynn.Sec);
                    Y_out = '-' + Ynn.Degree.ToString() + '°' + String.Format("{0:00}", Ynn.Min) + '\'' + String.Format("{0:00.000}", Ynn.Sec);
                    flag = false;
                }
                else
                   if (Check(Ynn) == true)
                {
                    Ynn.Min = Math.Abs(Ynn.Min);
                    Ynn.Sec = Math.Abs(Ynn.Sec);
                }
                if (flag == true)
                {
                    Y_out = DegreeToString(Ynn, 3);
                }

                var result = new AdvancedCordinateModel()
                {
                    Name = model.Name,
                    L = model.L,
                    H = model.H,
                    B = model.B,
                    X = model.X,
                    Z = model.Z,
                    Y = model.Y,
                    Ynn = Ynn,

                    L_L0 = L_L0,
                    TgL_L0 = tgL_L0n,
                    SinB = sinB,
                    TgY = tgY,
                };
                return result;
            }
            catch
            { throw; }
        }

        public DegreeData Round(DegreeData degree, int a_Mode)
        {
            var result = new DegreeData
            {
                Degree = degree.Degree,
                Min = degree.Min,
                Sec = degree.Sec
            };

            switch (a_Mode)
            {
                case 1:
                    if (result.Sec - Math.Truncate(result.Sec) > 0)
                    {
                        result.Sec = GauseRound(result.Sec);
                    }

                    if (result.Sec >= 60)
                    {
                        result.Sec = 0;
                        result.Min += 1;
                    }
                    break;
                case 2:
                    if (result.Sec - Math.Truncate(result.Sec) > 0)
                    {
                        result.Sec = GauseRound(result.Sec);
                    }

                    if (result.Sec >= 60)
                    {
                        result.Sec = 0;
                        result.Min += 1;
                    }
                    if (result.Sec == 30)
                    {
                        if (result.Min % 2 == 0)
                        {
                            result.Sec = 0;
                        }
                        else
                        {
                            result.Min += 1;
                            result.Sec = 0;
                        }
                    }
                    if (result.Sec > 30)
                    {
                        result.Min += 1;
                    }

                    result.Sec = 0;
                    if (result.Min >= 60)
                    {
                        result.Min = 0;
                        result.Degree += 1;
                    }
                    break;
                case 3:
                    if (result.Sec - Math.Truncate(result.Sec) > 0)
                    {
                        result.Sec = GauseRound(result.Sec);
                    }

                    if (result.Sec >= 60)
                    {
                        result.Sec = 0;
                        result.Min += 1;
                    }
                    if (result.Sec > 30)
                    {
                        result.Min += 1;
                    }

                    result.Sec = 0;
                    if (result.Min >= 60)
                    {
                        result.Min = 0;
                        result.Degree += 1;
                    }
                    if (result.Min > 30)
                    {
                        result.Degree += 1;
                    }

                    result.Min = 0;
                    break;
                default: break;
            }
            return result;
        }


        public DirectAngleCordinateModel DirectAngle(AdvancedCordinateModel point1, AdvancedCordinateModel point2)
        {

            double X2_X1 = point2.X - point1.X,
                   Y2_Y1 = point2.Y - point1.Y,
                   tga = Y2_Y1 / X2_X1;
            DegreeData a_temp = RadianToDegree(Math.Atan(Math.Abs(tga))),
                   a_true = a_temp;

            if (X2_X1 < 0)
            {
                if (Y2_Y1 < 0)
                {
                    a_true = Sum(a_temp, new DegreeData() { Degree = 180.0, Min = 0, Sec = 0 });
                }
                else
                {
                    a_true = Substruct(new DegreeData() { Degree = 180.0, Min = 0, Sec = 0 }, a_temp);
                }
            }
            else
                if (Y2_Y1 < 0)
            {
                a_true = Substruct(new DegreeData() { Degree = 360.0, Min = 0, Sec = 0 }, a_temp);
            }

            DegreeData A_temp;

            a_true.Sec = GauseRound(a_true.Sec);
            if (a_true.Sec == 60) { a_true.Min++; a_true.Sec = 0; }
            DegreeData y_temp = point1.Ynn; // see!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            y_temp.Sec = GauseRound(y_temp.Sec);
            if (y_temp.Sec == 60) { y_temp.Min++; y_temp.Sec = 0; }
            A_temp = Sum(a_true, y_temp);

            DegreeData A_round = Round(A_temp, 1),
                   a_round = Round(a_true, 1),
                   y_round = Round(point1.Ynn, 1);

            //if (p1.L90 != null && p1.B90 != null)
            //{
            //    Degree a23 = String2Degree(p1.L42) - String2Degree(p1.L90);
            //    double l42_l90 = gausround1(a23.Sec, 0);
            //    Degree b42_b90_2 = Rad2Deg(Deg2Rad(String2Degree(p1.B42) + String2Degree(p1.B90)) / 2);
            //    b42_b90_2.Sec = gausround1(b42_b90_2.Sec, 2);
            //    double sin = Math.Sin(Deg2Rad(b42_b90_2));
            //    Degree a90 = A_temp - new Degree() { Deg = 0, Min = 0, Sec = gausround1(l42_l90 * sin) };
            //    a90 = a90.Round(1);

            //    TObject1 temp3 = new TObject1()
            //    {
            //        Name = p1.Name + "\n" + p2.Name,
            //        Name2 = p1.Name,

            //        X42 = p1.X42 + "\n" + p2.X42,
            //        Y42 = p1.Y42 + "\n" + p2.Y42,

            //        X142 = p1.X42,
            //        X242 = p2.X42,
            //        Y142 = p1.Y42,
            //        Y242 = p2.Y42,

            //        H142 = p1.H42,
            //        H242 = p2.H42,

            //        X190 = p1.X90,
            //        X290 = p2.X90,
            //        Y190 = p1.Y90,
            //        Y290 = p2.Y90,
            //        Z190 = p1.Z90,
            //        Z290 = p2.Z90,

            //        DirAngle = Deg2String(a_round, 0),
            //        Y = Deg2String(y_round, 0),
            //        A42 = Deg2String(A_round, 0),
            //        L42_L90 = gausround1(l42_l90).ToString(),
            //        B42_B90_2 = b42_b90_2.Deg.ToString() + '°' + String.Format("{0:00}", b42_b90_2.Min) + '\'' + String.Format("{0:00.00}", b42_b90_2.Sec),
            //        Sin_B42_B90_2 = gausround1(sin, 6).ToString(),
            //        st3xst5 = gausround1(gausround1(l42_l90, 0) * sin, 0).ToString(),
            //        A90 = Deg2String(a90, 0),
            //        Name3 = p2.Name
            //    };
            //    string aa = Deg2String(A_round, 0);
            //    dirk.Add(temp3);
            //    dirangle42_table.Items.Add(temp3);
            //    A_geo_table.Items.Add(temp3);
            //}
            //else
            //{
            return new DirectAngleCordinateModel()
            {
                Name = point1.Name + "\n" + point2.Name,
                X = point1.X + "\n" + point2.X,
                Y = point1.Y + "\n" + point2.Y,
                DirectAngle = a_round,
                Meredian = y_round,
                Azimut = A_round
            };

            //}
        }
    }

}
