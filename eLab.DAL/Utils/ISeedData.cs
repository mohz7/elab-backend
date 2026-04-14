using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Utils
{
    public interface ISeedData
    {
        public Task IdentityDataSeedingAsync();
    }
}
