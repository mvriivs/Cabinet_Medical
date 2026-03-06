using System;
using System.Data;
using System.Data.SqlClient;

namespace Cabinet_Medical_Maria_Salau
{
    public partial class Pacienti : System.Web.UI.Page
    {
        private string ConnStr =>
            System.Configuration.ConfigurationManager.ConnectionStrings["CabinetConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["LoggedIn"] == null || !(bool)Session["LoggedIn"])
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadMedici();
                LoadPacienti();
                HideForm();
            }
        }

        private void LoadPacienti()
        {
            lblMsg.Text = "";

            string sql = @"
                SELECT 
                    p.ID_Pacient,
                    p.Nume, p.Prenume, p.CNP, p.Telefon, p.Email, p.Sex,
                    (m.Nume + ' ' + m.Prenume) AS Medic
                FROM Pacienti p
                INNER JOIN Medici m ON m.ID_Medic = p.ID_Medic
                ORDER BY p.Nume, p.Prenume;";

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPacienti.DataSource = dt;
                gvPacienti.DataBind();
            }
        }

        private void LoadMedici()
        {
            string sql = "SELECT ID_Medic, (Nume + ' ' + Prenume) AS NumeComplet FROM Medici ORDER BY Nume, Prenume;";

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    ddlMedic.Items.Clear();
                    while (r.Read())
                    {
                        ddlMedic.Items.Add(new System.Web.UI.WebControls.ListItem(
                            r["NumeComplet"].ToString(),
                            r["ID_Medic"].ToString()
                        ));
                    }
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ShowFormForInsert();
        }

        protected void gvPacienti_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            int id = Convert.ToInt32(gvPacienti.SelectedDataKey.Value);
            hfIDPacient.Value = id.ToString();

            string sql = @"SELECT ID_Pacient, Nume, Prenume, CNP, Data_Nasterii, Adresa, Telefon, Email, Sex, Data_Inscriere, ID_Medic
                           FROM Pacienti WHERE ID_Pacient = @id;";

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        txtNume.Text = r["Nume"].ToString();
                        txtPrenume.Text = r["Prenume"].ToString();
                        txtCNP.Text = r["CNP"].ToString();
                        txtTelefon.Text = r["Telefon"].ToString();
                        txtEmail.Text = r["Email"].ToString();
                        ddlSex.SelectedValue = r["Sex"].ToString();

                        txtDataNasterii.Text = Convert.ToDateTime(r["Data_Nasterii"]).ToString("yyyy-MM-dd");
                        txtDataInscriere.Text = Convert.ToDateTime(r["Data_Inscriere"]).ToString("yyyy-MM-dd");

                        txtAdresa.Text = r["Adresa"].ToString();
                        ddlMedic.SelectedValue = r["ID_Medic"].ToString();
                    }
                }
            }

            ShowFormForEdit();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (!ValidateForm())
                return;

            string sql = @"
                INSERT INTO Pacienti
                    (Nume, Prenume, CNP, Data_Nasterii, Adresa, Telefon, Email, Sex, Data_Inscriere, ID_Medic)
                VALUES
                    (@Nume, @Prenume, @CNP, @DataNasterii, @Adresa, @Telefon, @Email, @Sex, @DataInscriere, @IDMedic);";

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                FillParams(cmd);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.Text = "Pacient adăugat cu succes!";
            LoadPacienti();
            HideForm();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (string.IsNullOrWhiteSpace(hfIDPacient.Value))
            {
                lblMsg.Text = "Selectează un pacient din listă înainte de actualizare.";
                return;
            }

            if (!ValidateForm())
                return;

            string sql = @"
                UPDATE Pacienti SET
                    Nume=@Nume,
                    Prenume=@Prenume,
                    CNP=@CNP,
                    Data_Nasterii=@DataNasterii,
                    Adresa=@Adresa,
                    Telefon=@Telefon,
                    Email=@Email,
                    Sex=@Sex,
                    Data_Inscriere=@DataInscriere,
                    ID_Medic=@IDMedic
                WHERE ID_Pacient=@IDPacient;";

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                FillParams(cmd);
                cmd.Parameters.AddWithValue("@IDPacient", Convert.ToInt32(hfIDPacient.Value));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.Text = "Pacient actualizat!";
            LoadPacienti();
            HideForm();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (string.IsNullOrWhiteSpace(hfIDPacient.Value))
            {
                lblMsg.Text = "Selectează un pacient din listă înainte de ștergere.";
                return;
            }

            int id = Convert.ToInt32(hfIDPacient.Value);

            
            string sql = "DELETE FROM Pacienti WHERE ID_Pacient = @id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.Text = "Pacient șters!";
                LoadPacienti();
                HideForm();
            }
            catch (SqlException ex)
            {
                
                lblMsg.Text = "Nu pot șterge pacientul (are înregistrări asociate: programări/consultații).";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            HideForm();
        }

        private void FillParams(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Nume", txtNume.Text.Trim());
            cmd.Parameters.AddWithValue("@Prenume", txtPrenume.Text.Trim());
            cmd.Parameters.AddWithValue("@CNP", txtCNP.Text.Trim());
            cmd.Parameters.AddWithValue("@DataNasterii", DateTime.Parse(txtDataNasterii.Text.Trim()));
            cmd.Parameters.AddWithValue("@Adresa", txtAdresa.Text.Trim());
            cmd.Parameters.AddWithValue("@Telefon", txtTelefon.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@Sex", ddlSex.SelectedValue);
            cmd.Parameters.AddWithValue("@DataInscriere", DateTime.Parse(txtDataInscriere.Text.Trim()));
            cmd.Parameters.AddWithValue("@IDMedic", Convert.ToInt32(ddlMedic.SelectedValue));
        }

        private bool ValidateForm()
        {
            
            if (string.IsNullOrWhiteSpace(txtNume.Text) ||
                string.IsNullOrWhiteSpace(txtPrenume.Text) ||
                string.IsNullOrWhiteSpace(txtCNP.Text))
            {
                lblMsg.Text = "Completează minim: Nume, Prenume, CNP.";
                return false;
            }

           
            if (!DateTime.TryParse(txtDataNasterii.Text.Trim(), out _))
            {
                lblMsg.Text = "Data nașterii invalidă. Folosește format YYYY-MM-DD.";
                return false;
            }

            if (!DateTime.TryParse(txtDataInscriere.Text.Trim(), out _))
            {
                lblMsg.Text = "Data înscrierii invalidă. Folosește format YYYY-MM-DD.";
                return false;
            }

            return true;
        }

        private void ShowFormForInsert()
        {
            pnlForm.Visible = true;
            hfIDPacient.Value = "";

            txtNume.Text = "";
            txtPrenume.Text = "";
            txtCNP.Text = "";
            txtTelefon.Text = "";
            txtEmail.Text = "";
            txtAdresa.Text = "";
            ddlSex.SelectedValue = "F";
            txtDataNasterii.Text = "2000-01-01";
            txtDataInscriere.Text = DateTime.Today.ToString("yyyy-MM-dd");

            if (ddlMedic.Items.Count > 0)
                ddlMedic.SelectedIndex = 0;

            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }

        private void ShowFormForEdit()
        {
            pnlForm.Visible = true;
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
        }

        private void HideForm()
        {
            pnlForm.Visible = false;
            hfIDPacient.Value = "";
            gvPacienti.SelectedIndex = -1;

            btnSave.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }
    }
}
