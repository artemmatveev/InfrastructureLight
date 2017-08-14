using System.Collections.Generic;

namespace InfrastructureLight.Domain.Tree
{
    public abstract class TreeNode<T> : EntityBase
         where T : TreeNode<T>
    {       
        public virtual T Parent
        { get; set; }
        
        public virtual ICollection<T> Childrens
        { get; set; }
    }
}
