using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class BinarySearchTree<T> : ICollection<T>
    {
        protected class TreeNode : IBinaryNode<TreeNode, T>
        {
            public TreeNode? Parent { get ; set ; }
            public TreeNode? Left { get; set; }
            public TreeNode? Right { get; set; }
            public T Value { get; set; }
            public TreeNode(T elem, TreeNode? left, TreeNode? right)
            {
                Value = elem;
                Left = left;
                Right = right;
            }

            public TreeNode(): this (default(T)!, null, null) { }

            public TreeNode(T elem) : this(elem, null, null) { }
        }

        public event EventHandler? OnAddTreeNode;
        public event EventHandler? OnRemoveTreeNode;
        public event EventHandler? OnClearBST;

        private int _size;
        private readonly BinarySearchTreeSortOrder _order;
        protected readonly IComparer<T> _comparer;


        protected TreeNode? Root;
        protected virtual TreeNode GetTreeNode(BinarySearchTree<T> bst) => bst.Root;

        public BinarySearchTree()
        {
            Root = null;
            _size = 0;
            _comparer = Comparer<T>.Default;
        }

        public BinarySearchTree(IEnumerable<T> data, BinarySearchTreeSortOrder order, IComparer<T>? comparer)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!data.Any())
            {
                throw new ArgumentException(nameof(data));
            }
            Root = new TreeNode(data.First());
            _size = 1;
            _order = order;
            _comparer = comparer??Comparer<T>.Default;

            foreach (T elem in data.Skip(1))
            {
                this.Add(elem);
            }
        }

        public BinarySearchTree(IEnumerable<T> data) : this(data, BinarySearchTreeSortOrder.Ascending, null) { }
        public BinarySearchTree(BinarySearchTreeSortOrder order) : this() 
        {
            _order = order;
        }
        public BinarySearchTree(IComparer<T> comparer) : this()
        {
            _comparer = comparer;
        }
        public BinarySearchTree(IEnumerable<T> data, BinarySearchTreeSortOrder order) : this(data, order, null) { }

        public BinarySearchTree(IEnumerable<T> data, IComparer<T> comparer) : this(data, BinarySearchTreeSortOrder.Ascending, comparer) { }

        public BinarySearchTree(BinarySearchTreeSortOrder order, IComparer<T> comparer) : this()
        {
            _order = order;
            _comparer = comparer;
        }

        public int Count => _size;

        public bool IsReadOnly => false;

        //Add new item to tree
        public void Add(T item)
        {
            if (Contains(item))
            {
                return;
            }
            Add(Root, ref item);
            _size++;
            OnAddTreeNode?.Invoke(this, EventArgs.Empty);
        }

        private TreeNode? Add(TreeNode? addRoot, ref T item)
        {
            if (addRoot == null)
            {
                addRoot = new TreeNode(item);
            }
            else
            {
                var cmp = this.Comparer(item, addRoot.Value);
                if (cmp < 0)
                {
                    addRoot.Left = Add(addRoot.Left, ref item);
                }
                if (cmp > 0)
                {
                    addRoot.Right = Add(addRoot.Right, ref item);
                }

            }
            
            return addRoot;
        }

        //compare items
        private int Comparer(T item1, T value2)
        {
            int compVal = this._comparer.Compare(item1, value2);
            return compVal * (int)_order;
        }

        //clear BST
        public void Clear()
        {
            Root = null;
            _size = 0;
            OnClearBST?.Invoke(this, EventArgs.Empty);
        }

        //check if BST contains item
        public bool Contains(T item)
        {
            TreeNode node = Root;
            while (node != null)
            {
                int cmp = this.Comparer(item, node.Value);
                if (cmp < 0)
                {
                    node = node.Left;
                }
                else if (cmp > 0)
                {
                    node = node.Right;
                }
                else if(cmp == 0)
                {
                    return true;
                }
            }
            return false;
        }

        //copy elements from BST to array in selected order
        public void CopyTo(T[] array, int arrayIndex)
        {
            using (var enumerator = this.GetEnumerator(TreeTraversal.InOrder))
            {
                int i = arrayIndex;
                for (int j = 0; j < i; j++)
                {
                    enumerator.MoveNext();

                }
                while (enumerator.MoveNext())
                {
                    array[i++] = enumerator.Current;
                }
            };
        }

        // get Min element from BST
        public T Min()
        {
            if (Root == null)
            {
                throw new NullReferenceException(nameof(Root));
            }
            return this.Min(this.Root).Value;
        }

        protected TreeNode Min(TreeNode minRoot)
        {
            while (minRoot.Left != null)
            {
                minRoot = minRoot.Left;
            }
            return minRoot;
        }

        // get Max element from BST
        public T Max()
        {
            if (Root == null)
            {
                throw new NullReferenceException(nameof(Root));
            }
            return this.Max(this.Root).Value;
        }

        private TreeNode Max(TreeNode maxRoot)
        {
            if (maxRoot.Right != null)
            {
                maxRoot = maxRoot.Right;
            }
            return maxRoot;
        }


        //remove item from the BST
        public bool Remove(T item)
        {
            if (!Contains(item))
            {
                return false;
            }

            Root = Remove(Root, item);
            _size--;
            OnRemoveTreeNode?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private TreeNode? Remove(TreeNode? removeRoot, T item)
        {
            if (removeRoot == null)
            {
                return null;
            }

            var cmp = this.Comparer(item, removeRoot.Value);

            if (cmp > 0)
            {
                removeRoot.Right = Remove(removeRoot.Right, item);
            }
            else if (cmp < 0)
            {
                removeRoot.Left = Remove(removeRoot.Left, item);
            }
            else
            {
                if (removeRoot.Left == null && removeRoot.Right == null)
                {
                    return null;
                }
                if (removeRoot.Left != null && removeRoot.Right != null)
                {
                    var rightMin = Min(removeRoot.Right);
                    rightMin.Right = Remove(removeRoot.Right, rightMin.Value);
                    rightMin.Left = removeRoot.Left;
                    return rightMin;
                }

                return removeRoot.Left ?? removeRoot.Right;
            }
            return removeRoot;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(TreeTraversal.InOrder);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(TreeTraversal.InOrder);
        }

        public IEnumerator<T> GetEnumerator(TreeTraversal treeTraversal)
        {
            switch (treeTraversal)
            {
                case TreeTraversal.InOrder:
                    return new InOrderIterator(this);
                case TreeTraversal.InOrderReverse:
                    return new InOrderReverseIterator(this);
                case TreeTraversal.PreOrder:
                    return new PreOrderIterator(this);
                case TreeTraversal.LevelOrder:
                    return new LevelOrderIterator(this);
                case TreeTraversal.PostOrder:
                    return new PostOrderIterator(this);

                default:
                    break;
            }

            return new InOrderIterator(this);
        }

        //******************* InOrder Traversal *******************
        private class InOrderIterator : IEnumerator<T>
        {
            private readonly TreeNode _bstRoot;
            private readonly Stack<TreeNode> _stack = new();

            private TreeNode _trav;
            private TreeNode _current;

            public InOrderIterator(BinarySearchTree<T> bst)
            {
                _bstRoot = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _trav = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _stack.Push(bst.Root);
                _current = bst.Root;
            }
            public T Current => _current.Value;
            object IEnumerator.Current => _current;

            //move next in InOrder traversal
            public bool MoveNext()
            {
                if (_stack.Count == 0)
                {
                    return false;
                }

                while (_trav.Left != null)
                {
                    _stack.Push(_trav.Left);
                    _trav = _trav.Left;
                }

                _current = _stack.Pop();

                if (_current.Right != null)
                {
                    _stack.Push(_current.Right);
                    _trav = _current.Right;
                }
                return true;
            }

            public void Reset()
            {
                if (_bstRoot != null)
                {
                    _trav = _bstRoot;
                    _stack.Clear();
                    _stack.Push(_bstRoot);
                };
            }

            public void Dispose()
            {
                //throw new NotImplementedException();
                //there is no unmanaged resources -> shoud be automatically finalized -> disposed
				
				// Dispose(true);
				// GC.SuppressFinalize(this);

            }
        }

        //******************* InOrderReverse Traversal *******************
        private class InOrderReverseIterator : IEnumerator<T>
        {
            private readonly TreeNode _bstRoot;
            private readonly Stack<TreeNode> _stack = new();

            private TreeNode _trav;
            private TreeNode _current;

            public InOrderReverseIterator(BinarySearchTree<T> bst)
            {
                _bstRoot = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _trav = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _stack.Push(bst.Root);
                _current = bst.Root;
            }
            public T Current => _current.Value;

            object IEnumerator.Current => _current;

            //move next in InOrderReverse traversal
            public bool MoveNext()
            {
                if (_stack.Count == 0)
                {
                    return false;
                }

                while (_trav.Right != null)
                {
                    _stack.Push(_trav.Right);
                    _trav = _trav.Right;
                }

                _current = _stack.Pop();

                if (_current.Left != null)
                {
                    _stack.Push(_current.Left);
                    _trav = _current.Left;
                }
                return true;
            }

            public void Reset()
            {
                if (_bstRoot != null)
                {
                    _trav = _bstRoot;
                    _stack.Clear();
                    _stack.Push(_bstRoot);
                };
            }

            public void Dispose()
            {
                //throw new NotImplementedException();
                //there is no unmanaged resources -> shoud be automatically finalized -> disposed
            }
        }

        //******************* PreOrder Traversal *******************
        private class PreOrderIterator : IEnumerator<T>
        {
            private readonly TreeNode _bstRoot;
            private readonly Stack<TreeNode> _stack = new();

            private TreeNode _current;

            public PreOrderIterator(BinarySearchTree<T> bst)
            {
                _bstRoot = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _current = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _stack.Push(bst.Root);
            }

            public T Current => _current.Value;

            object IEnumerator.Current => _current;

            //move next in PreOrder traversal
            public bool MoveNext()
            {
                if (_stack.Count == 0)
                {
                    return false;
                }
                _current = _stack.Pop();
                if (_current.Right != null)
                {
                    _stack.Push(_current.Right);
                }
                if (_current.Left != null)
                {
                    _stack.Push(_current.Left);
                }
                return true;
            }

            public void Reset()
            {
                if (_bstRoot != null)
                {
                    _stack.Clear();
                    _stack.Push(_bstRoot);
                }
            }


            public void Dispose()
            {
                //throw new NotImplementedException();
                //there is no unmanaged resources -> shoud be automatically finalized -> disposed
            }
        }

        //******************* PostOrder Traversal *******************
        public class PostOrderIterator : IEnumerator<T>
        {
            private readonly TreeNode _bstRoot;
            private readonly Stack<TreeNode> _stack = new();

            private TreeNode _trav;
            private TreeNode _current;

            public PostOrderIterator(BinarySearchTree<T> bst)
            {
                _bstRoot = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _trav = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _current = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _stack.Push(bst.Root);
            }

            public T Current => _current.Value;

            object IEnumerator.Current => _current;

            //move next in PreOrder traversal
            public bool MoveNext()
            {
                if (_stack.Count == 0)
                {
                    return false;
                }

                while (_trav.Left != null)
                {
                    _stack.Push(_trav.Left);
                    _trav = _trav.Left;
                }

                _current = _stack.Pop();

                if (_trav == _current.Right || _trav == _current.Right?.Right)
                {
                    return true;
                }

                if (_current.Right != null)
                {

                    //_stack.Push(_current.Right);
                    _stack.Push(_current);
                    _trav = _current.Right;
                    _stack.Push(_trav);
                    //_current = _trav;
                    while (_trav.Left != null)
                    {
                        _stack.Push(_trav.Left);
                        _trav = _trav.Left;
                    }
                    _current = _stack.Pop();
                }

                //if (_current.Right != null)
                //{
                //    //_stack.Push(_current.Right);
                //    _trav = _current;
                //    _stack.Push(_current);
                //    _current = _trav.Right;
                //    //_trav = _current.Right;
                //    if (_current.Left != null)
                //    {
                //        while (_current.Left != null)
                //        {
                //            _stack.Push(_current.Left);
                //            _current = _current.Left;
                //        }
                //        _trav = _current;
                //    }
                //}
                return true;




                //public bool MoveNext()
                //{
                //    if (_stack.Count == 0)
                //    {
                //        return false;
                //    }

                //    while (_trav.Left != null)
                //    {
                //        _stack.Push(_trav.Left);
                //        _trav = _trav.Left;
                //    }

                //    _current = _stack.Pop();

                //    if (_current.Right != null)
                //    {
                //        _stack.Push(_current.Right);
                //        _trav = _current.Right;
                //    }
                //    return true;
                //}









            }

            public void Reset()
            {
                if (_bstRoot != null)
                {
                    _stack.Clear();
                    _stack.Push(_bstRoot);
                }
            }


            public void Dispose()
            {
                //throw new NotImplementedException();
                //there is no unmanaged resources -> shoud be automatically finalized -> disposed
            }
        }

        //******************* LevelOrder Traversal *******************
        private class LevelOrderIterator : IEnumerator<T>
        {
            private readonly TreeNode _bstRoot;
            private Queue<TreeNode> _queue = new();
            private TreeNode _current;

            public LevelOrderIterator(BinarySearchTree<T> bst)
            {
                _bstRoot = bst.Root ?? throw new ArgumentNullException(nameof(bst.Root));
                _current = bst.Root ?? throw new NullReferenceException(nameof(bst.Root));
                _queue.Enqueue(bst.Root);
            }

            public T Current => _current.Value;
            object IEnumerator.Current => _current;

            //move next in PreOrder traversal
            public bool MoveNext()
            {
                if (_queue.Count == 0)
                {
                    return false;
                }
                _current = _queue.Dequeue();
                if (_current.Left != null)
                {
                    _queue.Enqueue(_current.Left);
                }
                if (_current.Right != null)
                {
                    _queue.Enqueue(_current.Right);
                }
                return true;
            }

            public void Reset()
            {
                if (_bstRoot != null)
                {
                    _queue.Clear();
                    _queue.Enqueue(_bstRoot);
                }
            }

            public void Dispose()
            {
                //throw new NotImplementedException();
                //there is no unmanaged resources -> shoud be automatically finalized -> disposed
            }
        }

    }
}
