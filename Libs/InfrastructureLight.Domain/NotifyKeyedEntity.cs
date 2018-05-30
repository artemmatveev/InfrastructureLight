using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InfrastructureLight.ComponentModel;

namespace InfrastructureLight.Domain
{
    using Interfaces;

    public class NotifyKeyedEntity : NotifyPropertyEntry, IKeyedEntity<int>
    {
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
    }
}
