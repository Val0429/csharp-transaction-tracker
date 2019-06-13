using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Constant.Utility;

namespace Constant.Configuration
{
    public class ActivatorSection : ConfigurationSection
    {
        public Assembly Assembly
        {
            get
            {
                var file = GenericUtility.GetFileFullPath(AssemblyPath);
                return Assembly.LoadFrom(file);
            }
        }

        [ConfigurationProperty("assembly", IsRequired = true)]
        public string AssemblyPath
        {
            get { return (string)this["assembly"]; }
            set
            {
                this["assembly"] = value;
            }
        }

        [ConfigurationProperty("className", IsRequired = true)]
        public string ClassName
        {
            get { return (string)this["className"]; }
            set { this["className"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("parameters", IsDefaultCollection = false, IsRequired = false),
         ConfigurationCollection(typeof(ConstructorParameterCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ConstructorParameterCollection Parameters
        {
            get { return (ConstructorParameterCollection)this["parameters"]; }
        }
    }

    public class ConstructorParameterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                var result = (string)this["value"];

                return Encrypted ? Encryptions.DecryptDES(result) : result;
            }
            set
            {
                this["value"] = Encrypted ? Encryptions.EncryptDES(value) : value;
            }
        }

        [ConfigurationProperty("encrypted", IsRequired = false, DefaultValue = false)]
        public bool Encrypted
        {
            get { return (bool)this["encrypted"]; }
            set { this["encrypted"] = value; }
        }
    }

    public class ConstructorParameterCollection : GenericConfigurationCollection<ConstructorParameterElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConstructorParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }

    [ConfigurationCollection(typeof(ActivatorCollection), AddItemName = "activator", ClearItemsName = "clear", RemoveItemName = "remove")]
    public class ActivatorCollection : GenericConfigurationCollection<ActivatorSection>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ActivatorSection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }

    public class ActivatorRepostiory : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ActivatorCollection Activators
        {
            get { return (ActivatorCollection)base[""]; }
            set { base[""] = value; }
        }
    }

    public static class ActivatorSectionHelper
    {
        public static ConstructorParameterElement GetElementOrDefault(this ConstructorParameterCollection collection, string key)
        {
            var element = collection.OfType<ConstructorParameterElement>().FirstOrDefault(e => e.Name == key);
            if (element == null)
            {
                element = new ConstructorParameterElement() { Name = key };
                collection.Add(element);
            }

            return element;
        }
    }
}
