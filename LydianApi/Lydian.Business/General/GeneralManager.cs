using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lydian.Business.General
{
    public class GeneralManager
    {
        public static string CreateUniqueName()
        {
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString();
            guidString = Regex.Replace(guidString, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return guidString;
        }

    }
}
