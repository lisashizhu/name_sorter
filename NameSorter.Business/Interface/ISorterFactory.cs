using NameSorter.Common;

namespace NameSorter.Business.Interface
{
    public interface ISorterFactory
    {
        ISort Get(SortType sortType);
    }
}
