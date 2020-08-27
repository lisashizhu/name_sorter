using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NameSorter.Business.Interface;
using NameSorter.Business.Models;
using NameSorter.Common.Exceptions;
using NameSorter.WebApi.Models;
using NameSorter.WebApi.Extensions;


namespace NameSorter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NameController : Controller
    {
        private readonly ISorterFactory _sortFactory;

        public NameController(ISorterFactory sortFactory)
        {
            _sortFactory = sortFactory;
        }

        [HttpPost("Sort")]
        public ActionResult<SortNameResponse> SortName([FromBody] SortNameRequest sortNameRequest)
        {
            if (string.IsNullOrEmpty(sortNameRequest?.NameContent))
            {
                throw new BusinessException("No name to be sorted.");
            }

            var nameSorter = _sortFactory.Get(sortNameRequest.SortType);
            if (nameSorter == null)
            {
                throw new BusinessException("No name sorter found that can handle requested sort type.");
            }

            var fullNames = sortNameRequest.NameContent.ToFullNames();
            var sortedNames = nameSorter.Sort(fullNames);
            return CreateSortNameResponse(sortedNames);
        }

        [HttpGet("Download")]
        public ActionResult<SortNameResponse> Download([FromQuery] DownloadNameRequest downloadNameRequest)
        {
            var fileData = downloadNameRequest.NameContent;
            var data = Encoding.UTF8.GetBytes(fileData);

            return File(data, "text/plain",
                "sorted-names-list.txt");
        }

        private SortNameResponse CreateSortNameResponse(FullName[] fullNames)
        {
            // StringBuilder stringBuilder = new StringBuilder();
            // foreach (var fullName in fullNames)
            // {
            //     var stringGivenName = string.Join(' ', fullName.GivenNames);
            //     stringBuilder.AppendLine($"{stringGivenName} {fullName.LastName}");
            // }
            //
            // var res = stringBuilder.ToString();
            List<string> names = new List<string>();
            foreach (var fullName in fullNames)
            {
                var stringGivenName = string.Join(' ', fullName.GivenNames);
                names.Add($"{stringGivenName} {fullName.LastName}");
            }

            return new SortNameResponse {SortedNames = names.ToArray()};
        }
    }
}
