namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cazare")]
    public partial class Cazare
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cazare()
        {
            Contracte = new HashSet<Contracte>();
        }

        [Key]
        public int id_cazare { get; set; }

        public int? id_destinatie { get; set; }

        [StringLength(50)]
        public string tip_cazare { get; set; }

        [StringLength(50)]
        public string nume_cazare { get; set; }

        public virtual Destinatii Destinatii { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contracte> Contracte { get; set; }
    }
}
