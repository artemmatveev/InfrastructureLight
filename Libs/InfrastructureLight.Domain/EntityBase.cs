using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfrastructureLight.Domain
{
    public class EntityBase : Entity
    {
        [Column("ModifyDate", TypeName = "datetime")]
        public DateTime ModifyDate
        { get; set; }

        [Column("ModifyBy", TypeName = "nvarchar")]
        [StringLength(255)]
        public string ModifyBy
        { get; set; }
    }
}
