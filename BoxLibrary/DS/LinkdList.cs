using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class LinkdList<T>
    {
        private Node<T> first;
        private Node<T> last;
        int _current = 0;

        public void AddFirst(T data)
        {
            Node<T> toAdd = new Node<T>(data);
            if (first == null)
            {
                last = toAdd;
            }
            else
            {
                toAdd.next = first;
                first.pre = toAdd;
            }
            first = toAdd;
            _current++;
        }
        public bool RemoveTheLast(out T RemoveValue)
        {
            RemoveValue = default(T);
            if (first == null) return false;
            RemoveValue = first.data;
            first.pre = null;
            _current--;
            if (first == null) last = null;
            return true;
        }
        public bool GetAt(int position, out T value)
        {
            if (position < 0 || position > _current)
            {
                value = default(T);
                return false;
            }
            Node<T> node = first;
            while(node != null)
            {
                if (position == 0) break;
                position--;
                node = node.next;
            }
            value = node.data;
            return true;
        }
        public override string ToString()
        {
            Node<T> current = first;
            while(current != null)
            {
                Console.WriteLine(current.data);
                current = current.next;
            }
            return first.ToString();
        }
    }
}
