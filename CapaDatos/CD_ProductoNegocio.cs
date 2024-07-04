﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_ProductoNegocio
    {

        public int ObtenerStockProductoEnSucursal(int idProducto, int idNegocio)
        {
            int stock = 0;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT stock 
            FROM PRODUCTONEGOCIO 
            WHERE idProducto = @idProducto AND idNegocio = @idNegocio", oconexion);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                oconexion.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["stock"] != DBNull.Value)
                        {
                            stock = Convert.ToInt32(reader["stock"]);
                        }
                    }
                }
            }
            return stock;
        }

            public void CargarOActualizarStockProducto(int idProducto, int idNegocio, int stock)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand checkCmd = new SqlCommand(@"
            SELECT COUNT(*) 
            FROM PRODUCTONEGOCIO 
            WHERE idProducto = @idProducto AND idNegocio = @idNegocio", oconexion);
                checkCmd.Parameters.AddWithValue("@idProducto", idProducto);
                checkCmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                oconexion.Open();
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    // Insertar nuevo registro
                    SqlCommand insertCmd = new SqlCommand(@"
                INSERT INTO PRODUCTONEGOCIO (idProducto, idNegocio, stock)
                VALUES (@idProducto, @idNegocio, @stock)", oconexion);
                    insertCmd.Parameters.AddWithValue("@idProducto", idProducto);
                    insertCmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    insertCmd.Parameters.AddWithValue("@stock", stock);

                    insertCmd.ExecuteNonQuery();
                }
                else
                {
                    // Actualizar el stock existente
                    SqlCommand updateCmd = new SqlCommand(@"
                UPDATE PRODUCTONEGOCIO
                SET stock = @stock
                WHERE idProducto = @idProducto AND idNegocio = @idNegocio", oconexion);
                    updateCmd.Parameters.AddWithValue("@idProducto", idProducto);
                    updateCmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    updateCmd.Parameters.AddWithValue("@stock", stock);

                    updateCmd.ExecuteNonQuery();
                }
            }
        }

    }
}
