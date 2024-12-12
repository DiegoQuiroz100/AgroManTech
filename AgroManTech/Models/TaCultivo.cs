namespace AgroManTech.Models
{
    public class TaCultivo
    {
        public int CodigoCultivo { get; set; }
        public string NombreCultivo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicioProyectado { get; set; }
        public DateTime FechaFinProyectado { get; set; }
        public decimal GastoProyectado { get; set; }
        public int CodigoDistrito { get; set; }
        public int CodigoExtension { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class TaCultivoViewModel
    {
        public int CodigoCultivo { get; set; }
        public string NombreCultivo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicioProyectado { get; set; }
        public DateTime FechaFinProyectado { get; set; }
        public decimal GastoProyectado { get; set; }
        public string NombreDistrito { get; set; }
        public string NombreProvincia { get; set; }
        public string DescripcionExtension { get; set; }
    }

    public class DropdownItem
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
    }

}
