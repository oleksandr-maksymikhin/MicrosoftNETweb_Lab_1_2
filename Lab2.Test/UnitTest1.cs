using Lab_1;

namespace Lab2.Test
{
    public class UnitTest1: BinarySearchTree<int>
    {
        [Fact]
        public void ContainsTest()
        {
            // Arrange
            int[] array = { 3, 16, 2 };
            var bst = new BinarySearchTree<int>(array);

            // Act
            bool containsValue16 = bst.Contains(16);

            // Assert
            Assert.Equal(true, containsValue16);
        }

        [Fact]
        public void NotContainsTest()
        {
            // Arrange
            int[] array = { 3, 16, 2 };
            var bst = new BinarySearchTree<int>(array);

            // Act
            bool containsValue55 = bst.Contains(55);

            // Assert
            Assert.Equal(false, containsValue55);
        }

        [Fact]
        public void AddTest()
        {
            // Arrange
            int[] array = { 3, 16, 2};
            var bst = new BinarySearchTree<int>(array);

            // Act
            bst.Add(5);

            // Assert
            Assert.Equal(true, bst.Contains(5));
            Assert.Equal(5, GetTreeNode(bst).Right.Left.Value);
            Assert.Equal(array.Length + 1, bst.Count);
        }

        [Fact]
        public void RemoveTest()
        {
            // Arrange
            int[] array = { 3, 16, 2, 5 };
            var bst = new BinarySearchTree<int>(array);

            // Act
            bst.Remove(16);

            // Assert
            Assert.Equal(false, bst.Contains(16));
            Assert.Equal(array.Length - 1, bst.Count);
        }

        [Fact]
        public void MaxTest()
        {
            // Arrange
            int[] array = { 3, 16, 2};
            var bst = new BinarySearchTree<int>(array);

            // Act
            int maxValue = bst.Max();

            // Assert
            Assert.Equal(array.Max(), maxValue);
        }

        [Fact]
        public void MinTest()
        {
            // Arrange
            int[] array = { 3, 16, 2 };
            var bst = new BinarySearchTree<int>(array);

            // Act
            int minValue = bst.Min();

            // Assert
            Assert.Equal(array.Min(), minValue);
        }

        [Fact]
        public void ClearTest()
        {
            // Arrange
            int[] array = { 3, 16, 2 };
            var bst = new BinarySearchTree<int>(array);

            // Act
            bst.Clear();

            // Assert
            Assert.Equal(null, GetTreeNode(bst));
            Assert.Equal(0, bst.Count);
        }

        [Fact]
        public void CopyToInOrderTraversalTest()
        {
            //Arrange
            int[] array = { 3, 16, 2 };
            var bst = new BinarySearchTree<int>(array);
            int[] arrayModel = { 2, 3, 16 };

            //Act
            int[] copyToArray = new int[array.Length];
            bst.CopyTo(copyToArray, 0);
            //Assert

            Assert.NotEmpty(copyToArray);
            Assert.Equal(arrayModel, copyToArray);
        }

        [Fact]
        public void InOrderTraversalTest()
        {
            //Arrange
            List<int> list = new List<int>{ 3, 16, 2 };
            var bst = new BinarySearchTree<int>(list);
            List<int> listModel = new List<int> { 2, 3, 16 };
            List<int> InOrderTraversalResut = new List<int>();

            //Act
            using (var iterator = bst.GetEnumerator(TreeTraversal.InOrder))
            {
                while (iterator.MoveNext())
                {
                    InOrderTraversalResut.Add(iterator.Current);
                }
            }

            //Assert
            Assert.Equal(listModel, InOrderTraversalResut);

        }

        [Fact]
        public void InOrderReverseTraversalTest()
        {
            //Arrange
            List<int> list = new List<int> { 3, 16, 2 };
            var bst = new BinarySearchTree<int>(list);
            List<int> listModel = new List<int> { 16, 3, 2 };
            List<int> InOrderReverseTraversalResut = new List<int>();

            //Act
            using (var iterator = bst.GetEnumerator(TreeTraversal.InOrderReverse))
            {
                while (iterator.MoveNext())
                {
                    InOrderReverseTraversalResut.Add(iterator.Current);
                }
            }

            //Assert
            Assert.Equal(listModel, InOrderReverseTraversalResut);
        }

    }
}