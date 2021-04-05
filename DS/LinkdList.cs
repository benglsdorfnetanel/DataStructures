using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class LinkdList<T>
    {
        public Node first;
        public Node last;
        public int ExpValidity { get; set; } // a lot of references
        int _current = 0;
        public void AddFirst(T data)
        {
            Node toAdd = new Node(data);
            if (first == null)
            {
                last = toAdd;
            }
            else
            {
                toAdd.next = first;
                first.prev = toAdd;
            }
            first = toAdd;
            _current++;
        }
        public bool RemoveTheLast(out T RemoveValue)
        {
            RemoveValue = default(T);
            if (first == null) return false;
            RemoveValue = first.data;
            first.prev = null;
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
            Node node = first;
            while(node != null)
            {
                if (position == 0) break;
                position--;
                node = node.next;
            }
            value = node.data;
            return true;
        }
        public void RemoveNode(Node n)
        {
            // 7f 6 5 4 3 2 1l
            if (n == null) return;
            Node nextNode = n.next;
            Node prevNode = n.prev;
            if (n.next == null && n.prev == null) first = last = null; // the only one
            else if (n.next == null) // the last one
            {
                last = prevNode;
                last.next = null;
            }
            else if (n.prev == null) // the first one
            {
                first = nextNode;
                first.prev = null;
            }
            else // some where between first and last
            {
                nextNode.prev = prevNode;
                prevNode.next = nextNode;
            }
        }
        public void MoveNodeToFirst(Node n)
        {
            if (n == null) return;
            Node nextNode = n.next;
            Node prevNode = n.prev;
            if (nextNode == null && prevNode == null) return; // the only one
            else if (prevNode == null) return; // the last one 
            else if (nextNode == null)// the first one
            {
                prevNode.next = null;
                last = prevNode;
                MoveNodeToFirstInner();
            }
            else // some where in between /// 1 2 3 4 5
            {
                nextNode.prev = prevNode;
                prevNode.next = nextNode;
                MoveNodeToFirstInner();
            }

            // when you call for this method do not forget to update date

            void MoveNodeToFirstInner()
            {
                first.prev = n;
                n.next = first;
                n.prev = null;
                first = n;
            }
        }
        public override string ToString()
        {
            Node current = first;
            while(current != null)
            {
                Console.WriteLine(current.data);
                current = current.next;
            }
            return first.ToString();
        }

        public class Node
        {
            public T data { get; set; }
            public Node next { get; set; }
            public Node prev { get; set; }
            public Node(T data) => this.data = data;
        }
    }
}
