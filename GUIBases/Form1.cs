using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GUIBases
{
    public partial class Form1 : Form
    {
        AccesoBaseDatos baseDatos;
        String consultaSQL = "SELECT PrimerApellido FROM TERAPEUTA";
        public Form1()
        {
            InitializeComponent();
            baseDatos = new AccesoBaseDatos();
            llenarComboBox(consultaSQL, comboBoxTerapeutaAsignadoPacientes);
            llenarComboBox(consultaSQL, comboBoxTerapeutaAsignadoEmpresas);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButtonConvenioEmpresas.Checked = true;
        }

        private void llenarComboBox(String consulta, ComboBox comboBox) {
            SqlDataReader datos = null;
            try {
                datos = baseDatos.ejecutarConsulta(consulta);
            } catch (SqlException ex) {
                string mensajeError = ex.ToString();
                MessageBox.Show(mensajeError);
            } if (datos != null) {
                while (datos.Read()) comboBox.Items.Add(datos.GetValue(0));
            } else {
                MessageBox.Show("Datos vacio");
            }
        }

        private void llenarTabla(String consulta, DataGridView dataGridView) {
            DataTable tabla = null;
            try {
                tabla = baseDatos.ejecutarConsultaTabla(consulta);
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = tabla;
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                dataGridView.DataSource = bindingSource;
                for (int i = 0; i < dataGridViewConsultas.ColumnCount; i++) dataGridView.Columns[i].Width = 100;
            } catch (SqlException ex) {
                string mensajeError = ex.ToString();
                MessageBox.Show(mensajeError);
            }
        }

        private void textBoxNumTelPacientes1_TextChanged(object sender, EventArgs e)
        {
            textBoxNumTelPacientes2.Visible = true;
        }

        private void textBoxNumTelPacientes2_TextChanged(object sender, EventArgs e)
        {
            textBoxNumTelPacientes3.Visible = true;
        }

        private void numericUpDownDuracionPacientes_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonDescuentoSi_CheckedChanged(object sender, EventArgs e)
        {
            label15.Visible = true;
            textBoxCedJurTrabajoPacientes.Visible = true;
        }

        private void radioButtonDescuentoNo_CheckedChanged(object sender, EventArgs e)
        {
            label15.Visible = false;
            textBoxCedJurTrabajoPacientes.Visible = false;
        }

        private void textBoxNumTelEmpresas1_TextChanged(object sender, EventArgs e)
        {
            textBoxNumTelEmpresas2.Visible = true;
        }

        private void textBoxNumTelEmpresas2_TextChanged(object sender, EventArgs e)
        {
            textBoxNumTelEmpresas3.Visible = true;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButtonConvenioEmpresas_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDescuentoEmpresas.Enabled = true;
            textBoxLugarEmpresas.Enabled = false;
            textBoxTemaCharlaEmpresas.Enabled = false;
            comboBoxTerapeutaAsignadoEmpresas.Enabled = false;
            numericUpDownDuracionEmpresas.Enabled = false;
            numericUpDownDuracionEmpresas.Value = 0;
            dateTimePickerFechaEmpresas.Enabled = false;
        }

        private void radioButtonCharlaEmpresas_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDescuentoEmpresas.Enabled = false;
            numericUpDownDescuentoEmpresas.Value = 0;
            textBoxLugarEmpresas.Enabled = true;
            textBoxTemaCharlaEmpresas.Enabled = true;
            comboBoxTerapeutaAsignadoEmpresas.Enabled = true;
            numericUpDownDuracionEmpresas.Enabled = true;
            dateTimePickerFechaEmpresas.Enabled = true;
        }

        private void buttonConsultaCharlas_Click(object sender, EventArgs e)
        {
            llenarTabla("SELECT T.PrimerNombre, T.PrimerApellido, C.FechaHoraServ, C.LugarServ, C.Tema FROM TERAPEUTA T JOIN CHARLA C ON T.Cédula = C.CédTerapeuta", dataGridViewConsultas);
        }

        private void buttonConsultaCitas_Click(object sender, EventArgs e)
        {
            llenarTabla("SELECT T.PrimerNombre, T.PrimerApellido, C.FechaHoraServ, C.LugarServ, C.Tipo FROM TERAPEUTA T JOIN CITA C ON T.Cédula = C.CédTerapeuta", dataGridViewConsultas);
        }

        private void buttonAgregarPacientes_Click(object sender, EventArgs e)
        {
            String insertarCliente = "INSERT INTO CLIENTE (Cédula,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Email) VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxPrimerNombrePacientes.Text + "','" + textBoxSegundoNombrePacientes.Text + "','" + textBoxPrimerApellidoPacientes.Text + "','" + textBoxSegundoApellidoPacientes.Text + "','" + textBoxCorreoPacientes.Text +"')";
            String insertarNum1Cliente = "INSERT INTO NÚMERO_DE_TELÉFONO VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxNumTelPacientes1.Text + "')";
            String insertarNum2Cliente = "INSERT INTO NÚMERO_DE_TELÉFONO VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxNumTelPacientes2.Text + "')";
            String insertarNum3Cliente = "INSERT INTO NÚMERO_DE_TELÉFONO VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxNumTelPacientes3.Text + "')";
            String apellidoTerapeuta = comboBoxTerapeutaAsignadoPacientes.Text;
            if (!(textBoxNumTelPacientes1.Text == ""))
            {
                baseDatos.insertarValores(insertarNum1Cliente);
            }
            if (!(textBoxNumTelPacientes2.Text == ""))
            {
                baseDatos.insertarValores(insertarNum2Cliente);
            }
            if (!(textBoxNumTelPacientes3.Text == ""))
            {
                baseDatos.insertarValores(insertarNum3Cliente);
            }
            String obtenerCedulaTerapeuta = "SELECT Cédula FROM TERAPEUTA WHERE PrimerApellido = '" + apellidoTerapeuta + "';";
            String cedulaTerapeuta = baseDatos.getStringConsulta(obtenerCedulaTerapeuta);

            String dateTime = dateTimePickerFechaPacientes.Value.ToString("yyyy-MM-dd HH:mm:ss");
            textBoxCedJurEmpresas.Text = dateTime;


            //dateTimePickerFechaPacientes.Format = DateTimePickerFormat.Custom;
            //dateTimePickerFechaPacientes.CustomFormat = "yyyy'-'MM'-'dd HH':'mm':'ss'.000'";
            //String dateTimeServicio = dateTimePickerFechaPacientes.ToString();

            String insertarServicio = "INSERT INTO SERVICIOS (CédTerapeuta,Lugar,FechaHora,Duración) VALUES ( '" + cedulaTerapeuta + "','" + textBoxLugarPacientes.Text + "','" + dateTime + "'," + numericUpDownDuracionPacientes.Text + ")";

            baseDatos.insertarValores(insertarCliente);
            baseDatos.insertarValores(insertarServicio);
            //String insertarServicio = "INSERT INTO SERVICIOS ( '" + +"')";
            //baseDatos.insertarValores(consulta);
            //llenarComboBox(consultaSQL2, comboBox1);
            //llenarTabla(consultaSQL2, dataGridView1);
        }

        private void textBoxCedJurEmpresas_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButtonConvenioEmpresas.Checked == true)
            {
                String insertarEmpresa = "INSERT INTO EMPRESA (CédJurídica,Nombre,Descuento,Email) VALUES( '" + textBoxCedJurEmpresas.Text + "','" + textBoxNombreEmpresas.Text + "','" + numericUpDownDescuentoEmpresas.Text + "','" + textBoxCorreoEmpresas.Text + "')";
                String insertarNum1Empresa = "INSERT INTO NUMERO_TELEFONO_EMPRESA VALUES( '" + textBoxCedJurEmpresas.Text + "','" + textBoxNumTelEmpresas1.Text + "')";
                String insertarNum2Empresa = "INSERT INTO NUMERO_TELEFONO_EMPRESA VALUES( '" + textBoxCedJurEmpresas.Text + "','" + textBoxNumTelEmpresas2.Text + "')";
                String insertarNum3Empresa = "INSERT INTO NUMERO_TELEFONO_EMPRESA VALUES( '" + textBoxCedJurEmpresas.Text + "','" + textBoxNumTelEmpresas3.Text + "')";

                baseDatos.insertarValores(insertarEmpresa);
                if (!(textBoxNumTelEmpresas1.Text == "")) {
                    baseDatos.insertarValores(insertarNum1Empresa);

                }
                if (!(textBoxNumTelEmpresas2.Text == "")) {
                    baseDatos.insertarValores(insertarNum2Empresa);
                }
                if (!(textBoxNumTelEmpresas3.Text == "")) {
                    baseDatos.insertarValores(insertarNum3Empresa);
                }
            
            }
            if (radioButtonCharlaEmpresas.Checked)
            {

                String cedulaTerapeuta = baseDatos.getStringConsulta("select cédula from terapeuta where primerapellido = '" + comboBoxTerapeutaAsignadoEmpresas.SelectedItem.ToString() + "';");
               // MessageBox.Show(comboBoxTerapeutaAsignadoEmpresas.SelectedItem.ToString());
                MessageBox.Show(cedulaTerapeuta);
                

                String insertarCharla = "INSERT INTO Charla(CédJurídica,Nombre,Descuento,Email) VALUES( '" + textBoxCedJurEmpresas.Text + "','" + textBoxNombreEmpresas.Text + "','" + numericUpDownDescuentoEmpresas.Text + "','" + textBoxCorreoEmpresas.Text + "')";
                String insertarNum1Cliente = "INSERT INTO NÚMERO_DE_TELÉFONO VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxNumTelPacientes1 + "')";
                String insertarNum2Cliente = "INSERT INTO NÚMERO_DE_TELÉFONO VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxNumTelPacientes2 + "')";
                String insertarNum3Cliente = "INSERT INTO NÚMERO_DE_TELÉFONO VALUES( '" + textBoxNumCedPacientes.Text + "','" + textBoxNumTelPacientes3 + "')";
                String apellidoTerapeuta = comboBoxTerapeutaAsignadoPacientes.Text;
                baseDatos.insertarValores(insertarCharla);

            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
