namespace AgroManTech.Models
{
    public class Usuario
    {
        public int CodigoUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Contrasena { get; set; }
    }


}
