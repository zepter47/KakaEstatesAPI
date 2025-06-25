using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblPayment
{
    public int PaymentId { get; set; }

    public int WaterbillId { get; set; }

    public decimal AmountOwed { get; set; }

    public DateTime AddedOn { get; set; }

    public virtual ICollection<TblPaymentDetail> TblPaymentDetails { get; set; } = new ObservableCollection<TblPaymentDetail>();

    public virtual TblWaterbill Waterbill { get; set; } = null!;
}
