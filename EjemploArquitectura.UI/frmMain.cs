using System.Data;
using System.Data.SqlClient;

namespace EjemploArquitectura.UI
{
    public partial class frmMain : Form
    {
        private Guid _session;
        public Guid Session
        {
            get { return _session; }
            set
            {
                _session = value;
                MostrarUsuario();
                ValidarForm();
            }

        }

        public frmMain()
        {
            InitializeComponent();
            MostrarUsuario();
            ValidarForm();
        }

        private void MnuLogin_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.MdiParent = this;
            frm.Show();
        }

        private void MnuLogout_Click(object sender, EventArgs e)
        {
            this.Session = Guid.Empty;
        }
        private void ValidarForm()
        {
            this.MnuLogin.Enabled = (_session == Guid.Empty);
            this.MnuLogout.Enabled = (_session != Guid.Empty);
        }
        private void MostrarUsuario()
        {
          
            if (_session.Equals(Guid.Empty))
            {
                this.lblSession.Text = "Por favor, inicie sesión";
            }
            else
            {
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
                    SqlCommand cmd = sql.CreateCommand();
                    cmd.Connection = sql;
                    cmd.CommandText = "select username from usuarios where id_usuario=@id";
                    cmd.Parameters.AddWithValue("id", _session);
                    reader = cmd.ExecuteReader();
                    if (!reader.Read()) { throw new Exception("No hay datos"); }
                    this.lblSession.Text = reader[0].ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }
    }
}
