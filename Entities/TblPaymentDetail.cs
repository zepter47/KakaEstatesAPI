using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class TblPaymentDetail
{
    public int PaymentDetailsId { get; set; }

    public int PaymentId { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal AmountRemaining { get; set; }

    public DateTime AddedOn { get; set; }

    public virtual TblPayment Payment { get; set; } = null!;
}
