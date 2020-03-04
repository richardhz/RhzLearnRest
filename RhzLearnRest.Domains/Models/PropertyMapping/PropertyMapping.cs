using RhzLearnRest.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RhzLearnRest.Domains.Models.PropertyMapping
{
    public class PropertyMapping<TSource,TDestination> : IPropertyMapping
    {
        public Dictionary<string,PropertyMappingValue> MappingDictionary { get; }
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
        }
    }
}
