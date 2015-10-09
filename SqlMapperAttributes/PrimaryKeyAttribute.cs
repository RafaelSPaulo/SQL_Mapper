using System;

namespace SqlMapperAttributes
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, Inherited = false, AllowMultiple=false)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute() { }
        public PrimaryKeyAttribute(object value) {
            this.value = value;
        }

        private object value;
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
