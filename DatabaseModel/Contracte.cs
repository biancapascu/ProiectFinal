namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contracte")]
    public partial class Contracte
    {
        [Key]
        public int id_contract { get; set; }

        public int? id_client { get; set; }

        public int? id_cazare { get; set; }

        public int? id_transport { get; set; }

        public virtual Cazare Cazare { get; set; }

        public virtual Clienti Clienti { get; set; }

        public virtual Transport Transport { get; set; }
    }
}
