using AgroManTech.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

public class AuthController : Controller
{
    private readonly string _connectionString = "Server=192.168.50.8;Database=dbAgro;User Id=OperadorPract;Password=OPERADORPRACT@;TrustServerCertificate=True;";

    // Registro de usuario
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string hashedPassword = HashPassword(usuario.Contrasena);

                string query = "INSERT INTO taUsuario (Nombre, Email, Telefono, Direccion, Contrasena) " +
                               "VALUES (@Nombre, @Email, @Telefono, @Direccion, @Contrasena)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", usuario.Direccion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Contrasena", hashedPassword);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Login");
        }

        return View(usuario);
    }

    // Login de usuario
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string contrasena)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM taUsuario WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string hashedPassword = reader["Contrasena"].ToString();
                if (VerifyPassword(contrasena, hashedPassword))
                {
                    // Aquí puedes guardar información del usuario en la sesión
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        ViewBag.Error = "Email o contraseña incorrectos.";
        return View();
    }

    // Métodos para hashear/verificar contraseñas
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
    {
        string hashedPassword = HashPassword(enteredPassword);
        return hashedPassword == storedHashedPassword;
    }
}
