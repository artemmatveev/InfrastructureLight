using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfrastructureLight.Domain
{
    public abstract class Entity : IEntity<int>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", Order = 0, TypeName = "int")]
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

        [Column("DeletedFlag")]
        public bool DeletedFlag
        { get; set; }

        #region Methods

        public virtual void Delete()
        {
            DeletedFlag = true;
        }

        #endregion
    }
}
