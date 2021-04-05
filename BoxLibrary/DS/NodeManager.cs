using System;
using System.Collections.Generic;
using System.Text;

namespace DS
{
    public class NodeManager<T>
    {
        public T data { get; set; }
        public NodeManager<T> left { get; set; }
        public NodeManager<T> right { get; set; }
        public NodeManager(T data) => this.data = data;
    }
}
