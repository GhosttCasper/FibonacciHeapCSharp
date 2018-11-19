using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Фибоначчиева куча
3. Объединение двух куч.
 */

namespace FibonacciHeapCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            IntFibonacciHeap heap = ReadFile("input.txt");
            if (heap != null && !heap.IsEmpty())
                ProcessHeap(heap);
            WriteFile(heap, "output.txt");
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

        private static void ProcessHeap(IntFibonacciHeap heap)
        {
            heap.ExtractMin();
            IntFibonacciHeap secondHeap = ReadFile("input2.txt");
            if (secondHeap != null && !secondHeap.IsEmpty())
                heap.Union(secondHeap);

            //Console.WriteLine(heap.GetMinNode().Key);
            //Console.WriteLine(heap.GetMinNode().Key);
            //heap.DecreaseKey(heap.GetMinNode(), 1);
            //Console.WriteLine(heap.GetMinNode().Key);
            //heap.Delete(heap.GetMinNode());
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
