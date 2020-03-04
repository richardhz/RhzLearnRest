using RhzLearnRest.Domains.Models.PropertyMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace RhzLearnRest.Domains.Models.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(',');

            // apply each orderBy clause in reverse order 
            // otherwise IQueryable will be in the wrong order
            foreach(var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedClause = orderByClause.Trim();
                var orderDescending = trimmedClause.EndsWith(" desc");

                // move " asc" or " desc" from the orderBy clause so we get the property 
                // name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedClause : trimmedClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing.");
                }

                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));

                } 
            }
            return source;

        }
    }
}
