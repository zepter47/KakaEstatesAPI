using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblWaterbill
{
    public int WaterbillId { get; set; }

    public string? CustomInvoice { get; set; }

    public int TenantId { get; set; }

    public int HouseId { get; set; }

    public decimal PreviousReading { get; set; }

    public decimal CurrentReading { get; set; }

    public DateTime AddedOn { get; set; }

    public virtual TblHouse House { get; set; } = null!;

    public virtual TblPayment? TblPayment { get; set; }

    public virtual TblTenant Tenant { get; set; } = null!;
}
