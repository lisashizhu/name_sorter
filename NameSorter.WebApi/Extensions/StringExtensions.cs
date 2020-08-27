using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NameSorter.Business.Models;
using NameSorter.Common.Exceptions;

namespace NameSorter.WebApi.Extensions
{
    public static class StringExtensions
    {
        public static FullName[] ToFullNames(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            List<string> nameList = new List<string>(Regex.Split(input, Environment.NewLine));

            List<FullName> fullNames = new List<FullName>();
            foreach (var name in nameList)
            {
                var fullNameAsCollection = name.Split(' ').ToList();
                if (fullNameAsCollection.Count < 2)
                {
                    throw new BusinessException($"Invalid name {name}, both last name and given name should be provided.");
                }
                var fullName = new FullName
                {
                    LastName = fullNameAsCollection.Last(),
                };
                fullNameAsCollection.RemoveAt(fullNameAsCollection.Count-1);
                fullName.GivenNames = fullNameAsCollection.ToArray();
                fullNames.Add(fullName);
            }

            return fullNames.ToArray();
        }

    }
}
