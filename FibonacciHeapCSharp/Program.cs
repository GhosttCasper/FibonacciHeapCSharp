using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciHeapCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            IntFibonacciHeap heap = ReadFile("input.txt");
            if (heap != null && !heap.IsEmpty())
            { 
            //ProcessList(heap);
            Node<int> minNode = heap.GetMinNode();
            heap.ExtractMin();
            //Console.WriteLine(minNode.Key);
            IntFibonacciHeap secondHeap = ReadFile("input2.txt");
            heap.Union(secondHeap);

            minNode = heap.GetMinNode();
            Console.WriteLine(minNode.Key);
            WriteFile(heap, "output.txt");
            }
        }

        private static void WriteFile(IntFibonacciHeap heap, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                List<String> trees = new List<string>();
                trees = heap.HeapWalk();
                foreach (string treeStr in trees)
                    writer.WriteLine(treeStr);
            }
        }

        private static void ProcessList(IntFibonacciHeap heap)
        {
            
        }

        private static IntFibonacciHeap ReadFile(string fileName)
        {
            IntFibonacciHeap heap = new IntFibonacciHeap();
            using (StreamReader reader = new StreamReader(fileName))
            {
                var numbersStr = reader.ReadLine();
                heap = new IntFibonacciHeap(numbersStr);
            }
            return heap;
        }
    }
}
