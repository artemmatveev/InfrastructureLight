using System.Collections.Generic;

namespace InfrastructureLight.Domain.Tree
{
    public abstract class TreeNode<T> : EntityBase
         where T : TreeNode<T>
    {
        private readonly ICollection<T> children = new HashSet<T>();

        /// <summary>
        ///     Возвращает родительский узел
        /// </summary>
        public virtual T Parent
        { get; set; }

        /// <summary>
        ///     Возвращает все дочерние узлы
        /// </summary>
        public virtual ICollection<T> Childrens
        {
            get { return children; }
        }

        protected T This
        {
            get { return (T)this; }
        }

        /// <summary>
        ///     Добавляет дочерний узел для текущего узла
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(T child)
        {
            children.Add(child);
            child.Parent = This;
        }

        /// <summary>
        ///     Удаляет ссылку на родительский узел
        /// </summary>
        public virtual void ClearParent()
        {
            if (Parent == null)
                return;
            
            var collection = (ICollection<T>)Parent.Childrens;
            collection.Remove(This);
            Parent = null;
        }
    }
}
