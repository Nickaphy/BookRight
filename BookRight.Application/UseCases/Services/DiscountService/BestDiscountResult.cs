using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountService
{
    
        public sealed class BestDiscountResult
        {
            private readonly Lock _gate = new();
            private decimal _bestDiscount;
            private string? _winningStrategy;

            public decimal BestDiscount
            {
                get { lock (_gate) return _bestDiscount; }
            }

            public string? WinningStrategy
            {
                get { lock (_gate) return _winningStrategy; }
            }

            public void OfferDiscount(string strategyName, decimal discount)
            {
                lock (_gate)
                {
                    if (discount > _bestDiscount)
                    {
                        _bestDiscount = discount;
                        _winningStrategy = strategyName;
                    }
                }
            }
        }

    }

