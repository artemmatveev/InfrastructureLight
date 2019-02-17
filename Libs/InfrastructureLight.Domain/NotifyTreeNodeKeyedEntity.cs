using InfrastructureLight.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfrastructureLight.Domain
{
    using Interfaces;

    public class NotifyTreeNodeKeyedEntity<T> : NotifyPropertyEntity, IKeyedEntity<int>, ITreeNode<T>
         where T : NotifyTreeNodeKeyedEntity<T>
    {
        private readonly ICollection<T> _children = new HashSet<T>();
        protected T This => (T)this;

        #region IKeyedEntity

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(nameof(Id), Order = 0)]
        public int Id { get; set; }

        /// <summary>
        ///     Return <see cref="bool.True" />, 
        ///     if entity not saved in database
        /// </summary>
        [NotMapped]
        public bool IsTransient
        {
            get { return Id == 0; }
        }

        #endregion

        #region ITreeNode

        public virtual T Parent { get; private set; }
        public virtual ICollection<T> Children => _children;
        public virtual void AddChild(T child)
        {
            _children.Add(child);
            child.Parent = This;
        }
        public virtual void ClearParent()
        {
            if (Parent == null)
                return;

            var collection = Parent.Children;
            collection.Remove(This);
            Parent = null;
        }

        #endregion
    }
}
