using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace RhzLearnRest.Domains.Models.Helpers
{
    public static class IEnumerableExtinsions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            //create a list to hold our expandoObjects
            var expandoObjectList = new List<ExpandoObject>();

            // Create a list with PropertyInfo objects on TSource. Reflection is
            // expensive, so, rather than doing it for each object in the list, we do
            // it once and reuse the results. After all, part of the reflection is on the 
            // type of the object (TSource), not on the instance.
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');
                foreach(var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    // use reflection to get the property on the source object
                    // we need to include public and instance, because specifying a binding
                    // flag overwrites the already existing binding flags.
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} was not found on {typeof(TSource)}");
                    }

                    // add propertyInfo to list
                    propertyInfoList.Add(propertyInfo);
                }

            }

            // process the source objects
            foreach(TSource sourceObject in source)
            {
                // create an expandoObject that will hold the selected properties and values
                var dataShapedObject = new ExpandoObject();

                //Get the value of each property
                foreach(var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    // add the field to ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(dataShapedObject);
            }
            return expandoObjectList;
        }
    }
}
