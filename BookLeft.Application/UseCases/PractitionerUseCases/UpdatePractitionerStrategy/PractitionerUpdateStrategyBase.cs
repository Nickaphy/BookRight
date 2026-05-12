using BookRight.Application.UseCases.UpdateStrategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.PractitionerUseCases.UpdatePractitionerStrategy
{
    public abstract class PractitionerUpdateStrategyBase : IUpdateStrategy<Practitioner>
    {
        protected readonly IPractitionerRepository _practitionerRepository;

        protected PractitionerUpdateStrategyBase(IPractitionerRepository practitionerRepository)
        {
            _practitionerRepository = practitionerRepository;
        }

        public abstract Task UpdateAsync(Practitioner entity, object command);
    }
}
