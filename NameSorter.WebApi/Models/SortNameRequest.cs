using NameSorter.Common;

namespace NameSorter.WebApi.Models
{
    public class SortNameRequest
    {
        public string NameContent { get; set; }
        public SortType SortType { get; set; }
    }
}
