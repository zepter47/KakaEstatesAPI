using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblNokRelationship
{
    public int NokRelationshipId { get; set; }

    public string Relatioship { get; set; } = null!;

    public virtual ICollection<TblTenant> TblTenants { get; set; } = new ObservableCollection<TblTenant>();
}
