  public class Tree<T> : IEnumerable<T>
    {
        public class TreeNode<T>
        {
            private IList<TreeNode<T>> childs;

            public T Value { get; set; }
            public IList<TreeNode<T>> Childs
            {
                get
                {
                    return this.childs;
                }
                protected set
                {
                    this.childs = value;
                    foreach (TreeNode<T> child in this.childs)
                    {
                        child.Parent = this;
                    }
                }
            }
            public TreeNode<T> Parent { get; protected set; }

            public TreeNode(T value)
            {
                this.Value = value;
                this.childs = new List<TreeNode<T>>();
            }

            public override bool Equals(object obj)
            {
                TreeNode<T> other = (TreeNode<T>)obj;
                if (other == null)
                {
                    return false;
                }
                else
                {
                    return this.Value.Equals(other.Value);
                }
            }
        }

        public TreeNode<T> Root { get; set; }

        public Tree(T root)
        {
            TreeNode<T> rootNode = new TreeNode<T>(root);
            this.Root = rootNode;
        }

        public void Insert(T elementToInsert, T parent)
        {
            TreeNode<T> parentAsTreeNode = FindNode(this.Root, parent);
            if (parentAsTreeNode != null)
            {
                TreeNode<T> elementAsTreeNode = new TreeNode<T>(elementToInsert);
                parentAsTreeNode.Childs.Add(elementAsTreeNode);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unable to find element: {0}", parent.ToString()));
            }
        }
        public IEnumerable<T> GetElements()
        {
            IList<TreeNode<T>> nodeElements = GetElements(this.Root);
            IEnumerable<T> elements = nodeElements.Select(n => n.Value);
            return elements;
        }

        protected IList<TreeNode<T>> GetElements(TreeNode<T> root)
        {
            if (root == null)
            {
                return null;
            }

            List<TreeNode<T>> result = new List<TreeNode<T>>();

            IList<TreeNode<T>> children = root.Childs;

            foreach (var child in children)
            {
                if (child != null)
                {
                    result.AddRange(GetElements(child));
                    if (child.Childs.Count > 0)
                    {
                        result.AddRange(child.Childs);
                    }
                }
            }

            return result;
        }
        protected TreeNode<T> FindNode(TreeNode<T> root, T searched)
        {
            foreach (TreeNode<T> node in root.Childs)
            {
                if (node.Value.Equals(searched))
                {
                    return node;
                }
                else
                {
                    return FindNode(node, searched);
                }
            }

            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<T> elements = GetElements();
            return elements.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
