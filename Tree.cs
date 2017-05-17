using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tree<T> : IEnumerable<T>
{
    public Tree(T root)
    {
        TreeNode<T> rootNode = new TreeNode<T>(root);
        this.Root = rootNode;
    }

    public TreeNode<T> Root { get; set; }

    public IEnumerable<TreeNode<T>> NodeElements
    {
        get { return this.GetAllNodeElements(); }
    }

    public void Insert(T elementToInsert, T parent)
    {
        TreeNode<T> parentAsTreeNode = this.FindNode(this.Root, parent);
        if (parentAsTreeNode != null)
        {
            TreeNode<T> elementAsTreeNode = new TreeNode<T>(elementToInsert);
            parentAsTreeNode.InsertChild(elementAsTreeNode);
        }
        else
        {
            throw new InvalidOperationException(string.Format("Unable to find element: {0}", parent.ToString()));
        }
    }

    public void Remove(T elementToRemove)
    {
        TreeNode<T> element = this.FindNode(this.Root, elementToRemove);

        if (element == null)
        {
            throw new InvalidOperationException(string.Format("Unable to find element: {0}", elementToRemove.ToString()));
        }

        if (element.Equals(this.Root))
        {
            throw new InvalidOperationException("Root cannot be deleted");
        }

        element.Remove();
    }

    public IEnumerable<T> GetAllElements()
    {
        IList<TreeNode<T>> childElements = this.GetAllNodeElements();
        IEnumerable<T> elements = childElements.Select(n => n.Value);
        return elements;
    }

    public IEnumerator<T> GetEnumerator()
    {
        IEnumerable<T> elements = this.GetAllElements();
        return elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    protected IList<TreeNode<T>> GetAllNodeElements()
    {
        IList<TreeNode<T>> elements = this.GetChilds(this.Root);
        elements.Insert(0, this.Root);
        return elements;
    }

    protected IList<TreeNode<T>> GetChilds(TreeNode<T> root)
    {
        if (root == null)
        {
            return null;
        }

        List<TreeNode<T>> result = new List<TreeNode<T>>();

        IEnumerable<TreeNode<T>> children = root.Childs;

        foreach (var child in children)
        {
            if (child != null)
            {
                result.Add(child);
                IList<TreeNode<T>> subChildren = this.GetChilds(child);
                result.AddRange(subChildren);
            }
        }

        return result;
    }

    protected TreeNode<T> FindNode(TreeNode<T> root, T searched)
    {
        // TODO: Implement DFS or BFS
        IList<TreeNode<T>> elements = this.GetAllNodeElements();
        return elements.FirstOrDefault(x => x.Value.Equals(searched));
    }

    public class TreeNode<K>
    {
        private IList<TreeNode<K>> childs;

        public TreeNode(K value)
        {
            this.Value = value;
            this.childs = new List<TreeNode<K>>();
        }

        public K Value { get; set; }

        public IEnumerable<TreeNode<K>> Childs
        {
            get { return this.childs; }
        }

        public TreeNode<K> Parent { get; protected set; }

        public void InsertChild(TreeNode<K> node)
        {
            this.childs.Add(node);
            node.Parent = this;
        }

        public void Remove()
        {
            this.Parent.childs.Remove(this);
        }

        public override bool Equals(object obj)
        {
            TreeNode<K> other = obj as TreeNode<K>;
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.Value.Equals(other.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
