using System.Threading.Channels;
using System.Xml.Linq;

namespace Lab_1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int[] array = { 3, 3, 16, 2 };
            var bst = new BinarySearchTree<int>(array);

            bst.OnAddTreeNode += (obj, e) => Console.WriteLine("New Node is added to the BST");
            bst.OnRemoveTreeNode += (obj, e) => Console.WriteLine("Node is removed from the BST");
            bst.OnClearBST += (obj, e) => Console.WriteLine("BST is cleared");

            bst.Add(5);

            Console.WriteLine("InOrder Traversal");
            using (var iter = bst.GetEnumerator(TreeTraversal.InOrder))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\nInOrderReverse Traversal");
            using (var iter = bst.GetEnumerator(TreeTraversal.InOrderReverse))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine($"\n\nBST contains element 5 -> {bst.Contains(5)}\n");

            bst.Remove(16);
            Console.WriteLine("\nElement \"16\" was removed from BST");
            Console.WriteLine("InOrder Traversal after removal");
            using (var iter = bst.GetEnumerator(TreeTraversal.InOrder))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\nInOrderReverse Traversal after removal");
            using (var iter = bst.GetEnumerator(TreeTraversal.InOrderReverse))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\n\nCopy from BST to Array with InOrder travrsal");
            int[] arrayToCopy = new int[bst.Count];
            bst.CopyTo(arrayToCopy, 0);
            Console.WriteLine("BST copied in new array (with InOrder traversal method):");
            foreach (var item in arrayToCopy)
            {
                Console.Write($"{item} ; ");
            }

            Console.WriteLine("\n\nDifferent traversal methods:");
            List<int> bigList = new List<int> { 100, 20, 200, 10, 30, 150, 300 };
            Console.WriteLine("Original list:");
            foreach (var item in bigList)
            {
                Console.Write($"{item} ; ");
            }

            var bstTraversal = new BinarySearchTree<int>(bigList);
            Console.WriteLine("\n\nInorder traversal:");
            using (var iter = bstTraversal.GetEnumerator(TreeTraversal.InOrder))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\n\nInorderReverse traversal:");
            using (var iter = bstTraversal.GetEnumerator(TreeTraversal.InOrderReverse))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\n\nPreorder traversal:");
            using (var iter = bstTraversal.GetEnumerator(TreeTraversal.PreOrder))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\n\nPostorder traversal:");
            using (var iter = bstTraversal.GetEnumerator(TreeTraversal.PostOrder))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine("\n\nLevelorder traversal:");
            using (var iter = bstTraversal.GetEnumerator(TreeTraversal.LevelOrder))
            {
                while (iter.MoveNext())
                {
                    Console.Write($"{iter.Current} ; ");
                }
            }

            Console.WriteLine();
        }
    }
}