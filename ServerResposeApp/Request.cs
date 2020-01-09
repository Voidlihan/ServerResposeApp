using System;
using System.Collections.Generic;
using System.Text;

namespace ServerResposeApp
{
    public class Request<T>
    {
        public string Action { get; set; }
        public T Give { get; set; }
        public T Add { get; set; }
        public T Update { get; set; }
        public T Remove { get; set; }
        public string Path { get; set; }
    }
}
