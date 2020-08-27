using System.Linq;
using NameSorter.Business.Interface;
using NameSorter.Business.Models;

namespace NameSorter.Business
{
    public class FromLastNameThenGivenNameSorter : ISort
    {
        public FullName[] Sort(FullName[] fullNames)
        {
            var sortedFullNames = fullNames
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.GivenNames[0])
                .ToArray();

            return sortedFullNames;
        }
    }
}
