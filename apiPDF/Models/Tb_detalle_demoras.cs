using System.ComponentModel.DataAnnotations.Schema;

namespace apiPDF.Models
{
    public class Tb_detalle_demoras
    {

        [Column("ID")]
        public int Id { get; set; }

        [Column("TREN")]
        public string Tren { get; set; }

        [Column("REGION")]
        public string Region { get; set; }

        [Column("DISTRITO")]
        public string Distrito { get; set; }

        [Column("PK")]
        public string Pk { get; set; }

        [Column("CAUSA_DEMORA")]
        public string Causa_demora { get; set; }

        [Column("FECHA")]
        public DateTime Fecha { get; set; }

        [Column("TIEMPO_DEMORA")]
        public float Tiempo_demora { get; set; }


    }
}
