using System.Collections.Generic;

namespace InfrastructureLight.Domain
{
    using Interfaces;

    public abstract class TreeNodeKeyedEntity<T> : KeyedEntity, ITreeNode<T>
         where T : TreeNodeKeyedEntity<T>
    {
        private readonly ICollection<T> _children = new HashSet<T>();
        protected T This => (T)this;

        #region ITreeNode

        public virtual T Parent { get; private set; }
        public virtual ICollection<T> Childrens => _children;
        public virtual void AddChild(T child)
        {
            _children.Add(child);
            child.Parent = This;
        }
        public virtual void ClearParent()
        {
            if (Parent == null)
                return;

            var collection = Parent.Childrens;
            collection.Remove(This);
            Parent = null;
        }

        #endregion
    }
}
