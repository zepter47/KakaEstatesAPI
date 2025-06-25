using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblHouse
{
    public int HouseId { get; set; }

    public string HouseNumber { get; set; } = null!;

    public virtual ICollection<TblTenant> TblTenants { get; set; } = new ObservableCollection<TblTenant>();

    public virtual ICollection<TblWaterbill> TblWaterbills { get; set; } = new ObservableCollection<TblWaterbill>();
}
