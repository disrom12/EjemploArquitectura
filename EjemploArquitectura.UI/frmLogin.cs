using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjemploArquitectura.UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void ValidarUsuario(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) { throw new Exception("Datos incompletos"); }
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.InitialCatalog = "EjemploArquitectura";
            cs.DataSource = "CASA\\SQLEXPRESS"; //El nombre de tu dbServer
            cs.IntegratedSecurity = true;
            cs.TrustServerCertificate = true;

            SqlConnection sql = new SqlConnection();
            sql.ConnectionString = cs.ConnectionString;
            IDataReader reader = null;

            try
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sql;
                cmd.CommandText = "select id_usuario from usuarios where username=@username and password=@password";
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                
                if (!reader.Read()) { throw new Exception("Usuario o contraseña incorrecta"); }
                Guid id = Guid.Parse(reader[0].ToString());

                frmMain frm = (frmMain) this.MdiParent;
                frm.Session = id;
                this.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarUsuario(this.txtUserName.Text, this.txtPassword.Text);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message); 
            }
        }
    }
}
