using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciHeapCSharp
{
    public class FibonacciHeap<T> where T : IComparable
    {
        public Node<T> minimumNode;
        public int numberNodes; // текущее количество узлов

        public void MakeFibHeap() // амортизированная стоимость 0(1).
        {
            minimumNode = null;
            numberNodes = 0;
        }

        public void Insert(Node<T> node) // амортизированная стоимость 0(1).
        {
            if (node == null)
                throw new Exception("Unable to add null");

            node.degree = 0;
            node.Parent = null;
            node.Child = null;
            node.mark = false;
            if (minimumNode == null)
            {
                minimumNode = node;
                minimumNode.Left = minimumNode;
                minimumNode.Right = minimumNode;
            }
            else
            {
                node.Left = minimumNode.Left;
                node.Right = minimumNode;
                minimumNode.Left.Right = node;
                minimumNode.Left = node;
                if (node.Key.CompareTo(minimumNode.Key) < 0)
                    minimumNode = node;
            }
            numberNodes++;
        }

        public Node<T> GetMinNode() // амортизированная стоимость 0(1).
        {
            if (minimumNode != null)
                return minimumNode;
            return null;
        }

        public void Union(FibonacciHeap<T> heap) // амортизированная стоимость 0(1).
        {
            if (heap.minimumNode == null)
                return;
            if (minimumNode == null)
            {
                minimumNode = heap.minimumNode;
                numberNodes = heap.numberNodes;
            }
            else
            {
                UnionLists(minimumNode, heap.minimumNode);
                if (heap.minimumNode.Key.CompareTo(minimumNode.Key) < 0)
                    minimumNode = heap.minimumNode;
                numberNodes += heap.numberNodes;
            }
        }

        private void UnionLists(Node<T> first, Node<T> second)
        {
            if (second == null || first == null)
                return;
            Node<T> previousLeft = first.Left;
            Node<T> previousRight = second.Right;
            second.Right = first;
            first.Left = second;
            previousLeft.Right = previousRight;
            previousRight.Left = previousLeft;
        }

        public Node<T> ExtractMin() // амортизированная стоимость 0(lg п).
        {
            if (minimumNode == null)
                throw new Exception("Unable to Delete non-existing node");

            Node<T> previousMin = minimumNode;
            if (minimumNode.Child != null)
            {
                UnionLists(minimumNode, minimumNode.Child);
                var curNode = previousMin.Child;
                while (curNode.Parent != null)
                {
                    curNode.Parent = null;
                    curNode = curNode.Right;
                }
            }
            DeleteNodeFromList(minimumNode);
            if (previousMin == previousMin.Right)
            {
                minimumNode = null;
                numberNodes = 0;
            }
            else
            {
                minimumNode = previousMin.Right; // не обязательно минимальный узел
                Consolidate();
                numberNodes--;
            }
            return previousMin;
        }

        private void DeleteNodeFromList(Node<T> node)
        {
            if (node.Left == node.Right && node.Right == node)
                throw new Exception("Unable to Delete single existing node");
            Node<T> previousLeft = node.Left;
            Node<T> previousRight = node.Right;
            previousLeft.Right = previousRight;
            previousRight.Left = previousLeft;
        }

        private void Consolidate() // уплотнение
        {
            Node<T>[] A = new Node<T>[numberNodes + 1];
            var iterator = minimumNode; // iterator - текущий узел в корневом списке
            var startIterator = iterator;
            do
            {
                Node<T> curNode = iterator;
                iterator = iterator.Right;
                int degree = curNode.degree;
                while (A[degree] != null)
                {
                    Node<T> nodeWithSameDegreeAsCur = A[degree]; // Другой узел с той же степенью, что и у текущего
                    if (nodeWithSameDegreeAsCur.Key.CompareTo(curNode.Key) <= 0)
                    {
                        Node<T> prevCurNode = curNode;
                        curNode = nodeWithSameDegreeAsCur;
                        nodeWithSameDegreeAsCur = prevCurNode;
                    }
                    HeapLink(curNode, nodeWithSameDegreeAsCur);
                    A[degree] = null;
                    degree++;
                    if (curNode.Key.CompareTo(minimumNode.Key) < 0)
                        minimumNode = curNode;
                }
                A[degree] = curNode;
            }
            while (iterator != startIterator);
        }

        /// <summary>
        /// Удаляет 2-ой аргумент из списка корней и делет его дочерним узлом 1-го.
        /// </summary>
        private void HeapLink(Node<T> root, Node<T> toMakeChildNode)
        {
            DeleteNodeFromList(toMakeChildNode);
            toMakeChildNode.Left = toMakeChildNode;
            toMakeChildNode.Right = toMakeChildNode;
            if (root.Child == null)
                root.Child = toMakeChildNode;
            else
                UnionLists(root.Child, toMakeChildNode);
            toMakeChildNode.Parent = root;
            root.degree++;
            toMakeChildNode.mark = false;
        }

        public void DecreaseKey(Node<T> node, T key) // амортизированное время 0(1)
        {
            // если уменьшить узел, которого нет в куче, меньше, чем минимальный, то куча будет неправильно работать
            if (node.Key.CompareTo(key) < 0)
                throw new Exception("New key is larger than the current one.");
            node.Key = key;
            Node<T> parent = node.Parent;
            if (parent != null && node.Key.CompareTo(parent.Key) < 0)
            {
                Cut(node, parent);
                CascadingCut(parent);
            }
            if (node.Key.CompareTo(minimumNode.Key) < 0)
                minimumNode = node;
        }

        private void Cut(Node<T> child, Node<T> parent)
        {
            if (child.Left == child.Right && child.Right == child)
                parent.Child = null;
            else
                DeleteNodeFromList(child);
            parent.degree--;
            Insert(child);
        }

        private void CascadingCut(Node<T> node) //амортизированное время O(lgn).
        {
            Node<T> parent = node.Parent;
            if (parent != null)
                if (parent.mark == false)
                    parent.mark = true;
                else
                {
                    Cut(node, parent);
                    CascadingCut(parent);
                }
        }

        public void Delete(Node<T> node)
        {
            DecreaseKey(node, minimumNode.Key);
            minimumNode = node;
            ExtractMin();
        }

        public List<String> HeapWalk()
        {
            List<String> trees = new List<string>();
            var iterator = minimumNode;
            do
            {
                string treeStr = "*";
                trees.Add(TreeWalk(iterator, treeStr));
                iterator = iterator.Right;
            }
            while (iterator != minimumNode);
            return trees;
        }

        public string TreeWalk(Node<T> curRoot, string treeStr)
        {
            treeStr += curRoot.Key.ToString() + " ";
            var curChild = curRoot.Child;
            if (curChild != null)
            {
                treeStr = ChildsWalk(curChild, treeStr);
                var startChild = curChild;
                do
                {
                    treeStr = TreeWalk(curChild, treeStr);
                    curChild = curChild.Right;
                } while (curChild != startChild);
            }
            else
                treeStr += '\n'; // чтобы не ставить лишний перенос строки между деревьями
            return treeStr;
        }

        private string ChildsWalk(Node<T> curChild, string treeStr)
        {
            var startChild = curChild;
            do
            {
                treeStr += curChild.Key.ToString();
                treeStr += " ";
                curChild = curChild.Right;
            } while (curChild != startChild);
            treeStr += '\n';
            return treeStr;
        }

        public bool IsEmpty()
        {
            return minimumNode == null;
        }

    }

    public class IntFibonacciHeap : FibonacciHeap<int>
    {
        public IntFibonacciHeap(string str) : base()
        {
            try
            {
                var array = str.Split();
                foreach (var item in array)
                {
                    int intVar = int.Parse(item);
                    Node<int> curNode = new Node<int>(intVar);
                    Insert(curNode);
                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is FormatException)
                {
                    Console.WriteLine("String is empty (IntFibonacciHeap)");
                }
            }
        }
        public IntFibonacciHeap() : base()
        {
            MakeFibHeap();
        }
    }
}
