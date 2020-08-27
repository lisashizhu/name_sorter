using System.Collections.Generic;
using System.Threading.Tasks;
using NameSorter.Business.Models;

namespace NameSorter.Business.Interface
{
    public interface ISort
    {
        FullName[] Sort(FullName[] fullNames);
    }
}
