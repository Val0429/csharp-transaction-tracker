using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Constant.Configuration
{
    public abstract class GenericConfigurationCollection<T> : ConfigurationElementCollection, 
        IEnumerable<T> where T : ConfigurationElement
    {
        // Constructor
        protected GenericConfigurationCollection() { }


        protected GenericConfigurationCollection(IEnumerable<T> configurationElements)
        {
            foreach (var configurationElement in configurationElements)
            {
                Add(configurationElement);
            }
        }


        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public T this[int index]
        {
            get { return (T)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                
                BaseAdd(index, value);
            }
        }


        // Methods
        public void Add(T element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.OfType<T>().GetEnumerator();
        }
    }
}