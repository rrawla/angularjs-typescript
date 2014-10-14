using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Redis
{

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class StorageKeyAttribute : Attribute
    {
        public StorageKeyAttribute()
        {

        }
    }
}
