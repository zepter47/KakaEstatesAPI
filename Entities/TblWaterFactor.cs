using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblWaterFactor
{
    public int WaterFactorId { get; set; }

    public int UnitFactor { get; set; }

    public decimal FactorAmount { get; set; }
}
