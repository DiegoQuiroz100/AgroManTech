using AgroManTech.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

public class TaCultivoData
{
    private readonly string _connectionString = "Server=192.168.50.8;Database=dbAgro;User Id=OperadorPract;Password=OPERADORPRACT@;TrustServerCertificate=True;";

    public IEnumerable<TaCultivoViewModel> GetAll()
    {
        var cultivos = new List<TaCultivoViewModel>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(@"
                SELECT c.CodigoCultivo, c.NombreCultivo, c.Descripcion, c.FechaInicioProyectado, c.FechaFinProyectado,
                       c.GastoProyectado, d.NombreDistrito, p.NombreProvincia, e.Descripcion AS DescripcionExtension
                FROM TaCultivo c
                INNER JOIN GnDistrito d ON c.CodigoDistrito = d.CodigoDistrito
                INNER JOIN GnProvincia p ON d.CodigoProvincia = p.CodigoProvincia
                INNER JOIN gnExtension e ON c.CodigoExtension = e.CodigoExtension", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cultivos.Add(new TaCultivoViewModel
                        {
                            CodigoCultivo = reader.GetInt32(0),
                            NombreCultivo = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            FechaInicioProyectado = reader.GetDateTime(3),
                            FechaFinProyectado = reader.GetDateTime(4),
                            GastoProyectado = reader.GetDecimal(5),
                            NombreDistrito = reader.GetString(6),
                            NombreProvincia = reader.GetString(7),
                            DescripcionExtension = reader.GetString(8),
                        });
                    }
                }
            }
        }

        return cultivos;
    }

    public void Insert(TaCultivo cultivo)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(@"
                INSERT INTO TaCultivo (NombreCultivo, Descripcion, FechaInicioProyectado, FechaFinProyectado, GastoProyectado, CodigoDistrito, CodigoExtension, FechaRegistro)
                VALUES (@NombreCultivo, @Descripcion, @FechaInicioProyectado, @FechaFinProyectado, @GastoProyectado, @CodigoDistrito, @CodigoExtension, @FechaRegistro)", connection))
            {
                command.Parameters.AddWithValue("@NombreCultivo", cultivo.NombreCultivo);
                command.Parameters.AddWithValue("@Descripcion", cultivo.Descripcion);
                command.Parameters.AddWithValue("@FechaInicioProyectado", cultivo.FechaInicioProyectado);
                command.Parameters.AddWithValue("@FechaFinProyectado", cultivo.FechaFinProyectado);
                command.Parameters.AddWithValue("@GastoProyectado", cultivo.GastoProyectado);
                command.Parameters.AddWithValue("@CodigoDistrito", cultivo.CodigoDistrito);
                command.Parameters.AddWithValue("@CodigoExtension", cultivo.CodigoExtension);
                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
    }

    public IEnumerable<DropdownItem> GetDistritos()
    {
        var distritos = new List<DropdownItem>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(@"
                SELECT d.CodigoDistrito, d.NombreDistrito, p.NombreProvincia 
                FROM GnDistrito d
                INNER JOIN GnProvincia p ON d.CodigoProvincia = p.CodigoProvincia", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        distritos.Add(new DropdownItem
                        {
                            Codigo = reader.GetInt32(0),
                            Nombre = $"{reader.GetString(1)} - {reader.GetString(2)}"
                        });
                    }
                }
            }
        }

        return distritos;
    }

    public IEnumerable<DropdownItem> GetExtensions()
    {
        var extensions = new List<DropdownItem>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT CodigoExtension, Descripcion FROM gnExtension", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        extensions.Add(new DropdownItem
                        {
                            Codigo = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        });
                    }
                }
            }
        }

        return extensions;
    }

    public TaCultivoViewModel GetById(int id)
    {
        TaCultivoViewModel cultivo = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(@"
            SELECT c.CodigoCultivo, c.NombreCultivo, c.Descripcion, c.FechaInicioProyectado, c.FechaFinProyectado,
                   c.GastoProyectado, d.NombreDistrito, p.NombreProvincia, e.Descripcion AS DescripcionExtension
            FROM TaCultivo c
            INNER JOIN GnDistrito d ON c.CodigoDistrito = d.CodigoDistrito
            INNER JOIN GnProvincia p ON d.CodigoProvincia = p.CodigoProvincia
            INNER JOIN gnExtension e ON c.CodigoExtension = e.CodigoExtension
            WHERE c.CodigoCultivo = @CodigoCultivo", connection))
            {
                command.Parameters.AddWithValue("@CodigoCultivo", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cultivo = new TaCultivoViewModel
                        {
                            CodigoCultivo = reader.GetInt32(0),
                            NombreCultivo = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            FechaInicioProyectado = reader.GetDateTime(3),
                            FechaFinProyectado = reader.GetDateTime(4),
                            GastoProyectado = reader.GetDecimal(5),
                            NombreDistrito = reader.GetString(6),
                            NombreProvincia = reader.GetString(7),
                            DescripcionExtension = reader.GetString(8),
                        };
                    }
                }
            }
        }

        return cultivo;
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("DELETE FROM TaCultivo WHERE CodigoCultivo = @CodigoCultivo", connection))
            {
                command.Parameters.AddWithValue("@CodigoCultivo", id);
                command.ExecuteNonQuery();
            }
        }
    }


}
