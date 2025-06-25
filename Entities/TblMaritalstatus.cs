using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblMaritalstatus
{
    public int MaritalstatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<TblTenant> TblTenants { get; set; } = new ObservableCollection<TblTenant>();
}
