using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Domains.Models.PropertyMapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhzLearnRest.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public void Init<TDto,TObj>(Dictionary<string, PropertyMappingValue> propertyMapping)
        {
            _propertyMappings.Add(new PropertyMapping<TDto, TObj>(propertyMapping));
        }

        public bool ValidMappingExistsFor<TSource,TDestinatoon>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestinatoon>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields are 
                // coming from an orderBy string, this must be ignored.
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            //get matching mapping
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
