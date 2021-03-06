﻿using System.Collections.Generic;

namespace InfrastructureLight.Domain.Interfaces
{
    public interface ITreeNode<T> : IEntity
         where T : ITreeNode<T>
    {
        T Parent { get; }
        ICollection<T> Children { get; }
        void AddChild(T child);
        void ClearParent();
    }
}
