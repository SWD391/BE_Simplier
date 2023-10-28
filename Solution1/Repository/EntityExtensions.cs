using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class EntityExtensions
    {
        public static void UpdateNonNullProperties<T>(this T existingEntity, T updatedEntity)
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var updatedValue = property.GetValue(updatedEntity);
                if (updatedValue != null)
                {
                    property.SetValue(existingEntity, updatedValue);
                }
            }
        }
    }
}
