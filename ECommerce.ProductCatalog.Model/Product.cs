using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ECommerce.ProductCatalog.Model
{
    [DataContract]
    //[KnownType("KnownTypes")]
    public class Product
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public int Availability { get; set; }

        private static IEnumerable<Type> _productTypes;

        static IEnumerable<Type> KnownTypes()
        {
            if (_productTypes == null)
            {
                var list = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => typeof(Product).IsAssignableFrom(t))
                    .ToList();

                //list.Add(typeof(List<Product>));
                _productTypes = list;

            }
            return _productTypes;
        }
    }
}
