using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciHeapCSharp
{
    public class Node<T> where T : IComparable
    {
        public T Key;
        public Node<T> Parent;
        public Node<T> Child;
        public Node<T> Left;
        public Node<T> Right;
        public int degree;
        public bool mark; // указывает, были ли потери узлом х дочерних узлов начиная с момента, когда х стал дочерним узлом какого-то другого узла.

        public Node(T key)
        {
            Key = key;
        }
    }
}
