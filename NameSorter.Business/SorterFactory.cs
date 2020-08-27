using System;
using Microsoft.Extensions.DependencyInjection;
using NameSorter.Business.Interface;
using NameSorter.Common;

namespace NameSorter.Business
{
    public class SorterFactory : ISorterFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SorterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISort Get(SortType sortType)
        {
            switch (sortType)
            {
                case SortType.LastNameThenGivenName:
                    return ActivatorUtilities.CreateInstance<FromLastNameThenGivenNameSorter>(_serviceProvider);
                default: return null;
            }
        }
    }
}
