using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dto.Common
{
    public class RequestResultData
    {
        public DateTime RequertAt { get; set; }
        public double ExpiresIn { get; set; }
        public string TokeyType { get; set; }
        public string AccessToken { get; set; }

    }
}
