using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblTenant
{
    public int TenantId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string NinNumber { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int OccupantsNumber { get; set; }

    public int MaritalstatusId { get; set; }

    public string NextofkinName { get; set; } = null!;

    public int NokRelationshipId { get; set; }

    public string NokPhonenumber { get; set; } = null!;

    public int HouseId { get; set; }

    public DateTime AddedOn { get; set; }

    public virtual TblHouse House { get; set; } = null!;

    public virtual TblMaritalstatus Maritalstatus { get; set; } = null!;

    public virtual TblNokRelationship NokRelationship { get; set; } = null!;

    public virtual ICollection<TblWaterbill> TblWaterbills { get; set; } = new ObservableCollection<TblWaterbill>();
}
