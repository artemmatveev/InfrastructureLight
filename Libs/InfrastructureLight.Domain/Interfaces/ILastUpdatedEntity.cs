using System;
namespace InfrastructureLight.Domain.Interfaces
{
    public interface ILastUpdatedEntity : IEntity
    {
        DateTime ModifyDate { get; set; }
        string ModifyBy { get; set; }
    }
}
