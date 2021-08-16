using ZenitV2.Data;
using ZenitV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.BL.Interfaces
{
    public interface ICordinateConverterBL
    {
        List<CordinateModel> ConvertToPZ90(List<WordInputData> values);
        List<CordinateModel> ConvertToSK42(List<WordInputData> values, int zone);
        List<AdvancedCordinateModel> ConvertToAdvanced(List<CordinateModel> cordinates, int zone);
        DirectAngleCordinateModel DirectAngle(AdvancedCordinateModel point1, AdvancedCordinateModel point2);

    }
}
