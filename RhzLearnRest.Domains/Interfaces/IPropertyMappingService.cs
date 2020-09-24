using RhzLearnRest.Domains.Models.PropertyMapping;
using System.Collections.Generic;

namespace RhzLearnRest.Domains.Interfaces
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestinatoon>(string fields);
        void Init<TDto, TObj>(Dictionary<string, PropertyMappingValue> propertyMapping);
    }
}