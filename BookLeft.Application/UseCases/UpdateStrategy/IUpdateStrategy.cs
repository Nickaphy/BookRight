using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.UpdateStrategy
{
    public interface IUpdateStrategy<T> // T is the type of entity being updated, e.g., Practitioner, Customer, etc.
    {
        Task UpdateAsync(T  entity, object command);
    }
}
