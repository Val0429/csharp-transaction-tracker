using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpHandler
{
    public interface IDataWorker
    {
        String Get(Dictionary<String, String> qsPair);
        String Post(Dictionary<String, String> qsPair, String param);
        String User { get; set; }
        String Group { get; set; }
    }
}
