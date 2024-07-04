using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Modales
{
    public partial class mdCargaStock : Form
    {
        public mdCargaStock()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in dgvData.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;


                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBusqueda.Clear();
            foreach (DataGridViewRow row in dgvData.Rows)
                row.Visible = true;
        }

        private void mdCargaStock_Load(object sender, EventArgs e)
        {
            txtBusqueda.Select();
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {

                if (columna.Visible == true)
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });

                }


            }

            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 1;

            //Mostrar todos los Productos
            List<Producto> listaProducto = new CN_Producto().Listar();

            foreach (Producto item in listaProducto)
            {
                int stockProducto = new CN_ProductoNegocio().ObtenerStockProductoEnSucursal(item.idProducto, GlobalSettings.SucursalId);
                dgvData.Rows.Add(new object[] { item.idProducto,
                    item.codigo,
                    item.nombre,
                    item.oCategoria.descripcion,
                    stockProducto,
                    item.precioCompra,
                    item.precioVenta,

                    });
            }
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActualizarStock_Click(object sender, EventArgs e)
        {
            // Iterar sobre las filas del DataGridView
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // Obtener el estado de cambio de la fila
                bool isRowModified = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.IsInEditMode) // Verificar si la celda está en modo de edición
                    {
                        isRowModified = true;
                        break;
                    }
                }

                if (isRowModified)
                {
                    // Obtener el idProducto, idNegocio y el nuevo valor de stock
                    int idProducto = Convert.ToInt32(row.Cells["idProducto"].Value); // Ajusta el nombre de la columna según tu DataGridView
                    int idNegocio = Convert.ToInt32(row.Cells["idNegocio"].Value);   // Ajusta el nombre de la columna según tu DataGridView
                    int nuevoStock = Convert.ToInt32(row.Cells["stock"].Value);      // Ajusta el nombre de la columna según tu DataGridView

                    // Llamar al método para cargar o actualizar el stock del producto
                    new CN_ProductoNegocio().CargarOActualizarStockProducto(idProducto, idNegocio, nuevoStock);
                }
            }

            MessageBox.Show("El stock ha sido actualizado correctamente.");
        }
    }
}
