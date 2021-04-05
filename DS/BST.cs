using System;
using System.Collections.Generic;
using System.Text;

namespace DS
{
    public class BST<T> where T : IComparable<T>
    {
        Node _root = null;

        public void Add(T item)
        {
            if (_root == null)
            {
                _root = new Node(item);
                return;
            }

            Node tmp = _root;
            Node parent = null;
            while (tmp != null)
            {
                if (tmp.data.CompareTo(item) == 0) return;
                parent = tmp;
                if (item.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }
            if (item.CompareTo(parent.data) < 0) parent.left = new Node(item);
            else parent.right = new Node(item);
        }

        public T GetData(T data)
        {
            return GetNode(_root);
            T GetNode(Node root)
            {
                if (data.CompareTo(root.data) < 0) return GetNode(root.left);
                else if (data.CompareTo(root.data) > 0) return GetNode(root.right);
                return root.data;
            }
        }

        public void ScanInOrder(Action<T> action)
        {
            ScanInOrder(_root, action);
        }
        private void ScanInOrder(Node root, Action<T> action)
        {
            if (root == null) return;
            ScanInOrder(root.left, action);
            action(root.data);
            ScanInOrder(root.right, action);
        }
        //return add at home.
        public bool SearchAndAdd(T data,out T founded)
        {
            founded = default;
            if (_root == null)
            {
                _root = new Node(data);
                founded = _root.data;
                return false;
            }
            Node tmp = _root;
            Node parent = null;
            while (tmp != null)
            {
                if (data.CompareTo(tmp.data) != 0)
                {
                    parent = tmp;
                    if (data.CompareTo(tmp.data) < 0)
                    {
                        tmp = tmp.left;
                    }
                    else if(data.CompareTo(tmp.data) > 0)
                    {
                        tmp = tmp.right;
                    }
                    else if (data.CompareTo(tmp.data) == 0)
                    {
                        founded = tmp.data;
                        return true;
                    }
                }
                else
                {
                    founded = tmp.data;
                    return false;
                }
            }
            if (data.CompareTo(parent.data) < 0)
            {
                parent.left = new Node(data);
                founded = parent.left.data;
            }
            else
            {
                parent.right = new Node(data);
                founded = parent.right.data;
            }
            return true;
        }
        #region dont use that shit *need to fix it*
        /* public bool Remove(T item)
        {
            if (_root == null)
                return false;

            Node tmp = _root;
            Node parent = null;
            byte direction = (byte)Direction.unassigned;
            bool exist;

            while (tmp != null)
            {
                if (item.CompareTo(tmp.data) != 0)
                {
                    parent = tmp;
                    if (item.CompareTo(tmp.data) < 0)
                    {
                        direction = (byte)Direction.left;
                        tmp = tmp.left;
                    }
                    else
                    {
                        direction = (byte)Direction.right;
                        tmp = tmp.right;
                    }
                }
                else
                {
                    if (parent == null && tmp.left == null && tmp.right == null) _root = null;
                    else if (tmp.left == null && tmp.right == null)
                    {
                        if (direction == 1) parent.left = null;
                        else if (direction == 2) parent.right = null;
                    }
                    else
                    {
                        if (tmp.left == null && tmp.right != null)
                        {
                            if (direction == 1) parent.left = tmp.right;
                            else if (direction == 2) parent.right = tmp.right;
                        }


                        else if (tmp.left != null && tmp.right == null)
                        {
                            if (direction == 1) parent.left = tmp.left;
                            else if (direction == 2) parent.right = tmp.left;
                        }

                        else
                        {
                            Node savedRightNode = tmp.right;
                            tmp = tmp.left;

                            if (direction == 1) parent.left = tmp;
                            else if (direction == 2) parent.right = tmp;

                            while (tmp.right != null) tmp = tmp.right;
                            tmp.right = savedRightNode;
                        }

                    }
                    exist = true;
                    IfTheNumberRemove(item, exist);
                    return true;

                }
            }
            exist = false;
            IfTheNumberRemove(item, exist);
            return false;

        }
        */
        #endregion
        public void Delete(T value)
        {
            _root = Delete(_root);

            Node Delete(Node root)
            {
                if (root == null) return root;
                // searching for value
                else if (value.CompareTo(root.data) < 0) root.left = Delete(root.left);
                else if (value.CompareTo(root.data) > 0) root.right = Delete(root.right);
                else
                {
                    if (root.left == null) return root.right; // one or no childs
                    else if (root.right == null) return root.left; // one or no childs
                    var tmp = GetMostLeftData(root.right);
                    this.Delete(tmp); // Delete the successor
                    root.data = tmp;
                }
                return root;
            }
            T GetMostLeftData(Node root) // a method to find a successor(minimal value of right subtree)
            {
                T mostLeftData = root.data;
                while (root.left != null)
                {
                    mostLeftData = root.left.data;
                    root = root.left;
                }
                return mostLeftData;
            }
        }
        public void FindBestMatch(T data, out Node node)
        {
            node = FindBestMatch(_root);
            Node FindBestMatch(Node root)
            {
                if (root == null) return root; 
                if (root.data.CompareTo(data) < 0) return FindBestMatch(root.right); 
                else if (root.data.CompareTo(data) == 0) return root; 
                else
                { 
                    Node nodedata = FindBestMatch(root.left);
                    if (nodedata == null) return root;
                    return nodedata;
                }
            }
        }
        public void FindNextBestMatch(Node bestMatch, out Node nextBest)
        {
            Node parent = nextBest = null;
            if (bestMatch.right != null)
                nextBest = FindMostLeft(bestMatch.right);
            else
            {
                FindCorrectParent(_root);
                if (parent == null) return;
                if (parent.data.CompareTo(bestMatch.data) < 0) return;
                nextBest = parent;
            }

            Node FindMostLeft(Node root)
            {
                if (root.left != null) return FindMostLeft(root.left);
                else return root;
            }
            void FindCorrectParent(Node root)
            {
                if (root == null) return;
                else if (root.right == bestMatch) return;
                else if (root.data.CompareTo(bestMatch.data) < 0) FindCorrectParent(root.right);
                else if (root.data.CompareTo(bestMatch.data) > 0) FindCorrectParent(root.left);
                if (root.data.CompareTo(bestMatch.data) > 0 && parent == null) parent = root;
            }
        }
        public void IfTheNumberRemove(T item, bool exist)
        {
            if (exist) Console.WriteLine($"The node with the value of {item} was removed.");
            else Console.WriteLine($"The node with the value of {item} was doesn't exist.");
        }
        public T GetNode(T value)
        {
            return GetNode(_root);

            T GetNode(Node root)
            {
                if (root == null) return default;
                if (value.CompareTo(root.data) < 0) return GetNode(root.left);
                else if (value.CompareTo(root.data) > 0) return GetNode(root.right);
                return root.data;
            }
        }
        public bool IsEmpty() => _root == null;
        enum Direction
        {
            unassigned = 0,
            left = 1,
            right = 2
        }


        public class Node
        {
            public T data { get; set; }
            public Node left { get; set; }
            public Node right { get; set; }
            public Node(T data) => this.data = data;
        }
    }
}
