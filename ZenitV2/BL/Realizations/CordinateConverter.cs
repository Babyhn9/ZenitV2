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
    class CordinateConverter : ICordinateConverterBL
    {
        private ICalculationBL _calculationBl;

        public CordinateConverter(ICalculationBL calculationBl)
        {
            _calculationBl = calculationBl;
        }
        public List<CordinateModel> ConvertToPZ90(List<WordInputData> values)
        {
            return values.Select(el => new CordinateModel
            {
                Name = el.Name,
                X = el.North,
                Y = el.East,
                Z = el.Height,
                B = _calculationBl.BL90(el.North, el.East, el.Height),
                L = _calculationBl.L90(el.North, el.East),
                H = _calculationBl.H90(el.North, el.East, el.Height)
            }).ToList();
        }

        public List<CordinateModel> ConvertToSK42(List<WordInputData> values, int zone)
        {
            return values.Select(el => new CordinateModel
            {
                Name = el.Name,
                X = el.North,
                Y = el.East,
                L = _calculationBl.CalculateL42(el.North, el.East, zone),
                B = _calculationBl.CalculateB42(el.North, el.East, el.Height),
                H = el.Height,

            }).ToList();
        }

        public List<AdvancedCordinateModel> ConvertToAdvanced(List<CordinateModel> cordinates, int zone) =>
            cordinates.Select(el => _calculationBl.CalculateY6ZN(el,zone)).ToList();

        public DirectAngleCordinateModel DirectAngle(AdvancedCordinateModel point1, AdvancedCordinateModel point2) => _calculationBl.DirectAngle(point1, point2);
    }
}
