namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Destinatii")]
    public partial class Destinatii
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Destinatii()
        {
            Cazare = new HashSet<Cazare>();
        }

        [Key]
        public int id_destinatie { get; set; }

        [StringLength(50)]
        public string oras { get; set; }

        [StringLength(50)]
        public string tara { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cazare> Cazare { get; set; }
    }
}
