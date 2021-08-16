using ZenitV2.Data;
using ZenitV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.BL.Interfaces
{
    public interface ICalculationBL
    {
        DegreeData BL90(double x, double y, double z);
        DegreeData L90(double x, double y);
        double H90(double x, double y, double z);
        DegreeData CalculateB42(double x, double y, double z);
        DegreeData CalculateL42(double x, double y, double zone);
        double GauseRound(double x, double n);
        double GauseRound(double x);
        DegreeData RadianToDegree(double radian);
        string AngleToAngleM(DegreeData degree);
        double DegreeToRadian(DegreeData degree);
        public DegreeData Sum(DegreeData degree1, DegreeData degree2);
        DegreeData Multiply(DegreeData degree1, int n);
        DegreeData Division(DegreeData degree1, int n);
        DegreeData Substruct(DegreeData degree1, DegreeData degree2);
        bool IsMore(DegreeData degree1, DegreeData degree2);
        bool IsLess(DegreeData degree1, DegreeData degree2);
        CordinateModel CalculateY(CordinateModel model, int zone);
        AdvancedCordinateModel CalculateY6ZN(CordinateModel model, int zone);
        DirectAngleCordinateModel DirectAngle(AdvancedCordinateModel point1, AdvancedCordinateModel point2);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="n">количество знаков после запятой</param>
        /// <returns></returns>
        string DegreeToString(DegreeData degree, int n);






    }
}
