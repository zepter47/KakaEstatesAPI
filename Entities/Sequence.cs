using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Entities;

public partial class Sequence
{
    public string SequenceName { get; set; } = null!;

    public long CurrentValue { get; set; }
}
