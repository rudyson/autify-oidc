using System;
using System.Collections.Generic;
using System.Text;

namespace Rudyson.Autify.Domain.Core;

public abstract class AggregateRoot<TId>
{
    public TId Id { get; protected set; } = default!;
}
