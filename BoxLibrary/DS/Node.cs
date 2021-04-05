using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    class Node<T>
    {
        public Node<T> next;
        public Node<T> pre;
        public T data;
        public Node(T value)
        {
            data = value;
        }
        public override string ToString()
        {
            return data.ToString();
        }
    }
}
