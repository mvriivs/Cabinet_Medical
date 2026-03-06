using System;
using System.Data;
using System.Data.SqlClient;

namespace Cabinet_Medical_Maria_Salau
{
    public partial class Programari : System.Web.UI.Page
    {
        private string ConnStr =>
            System.Configuration.ConfigurationManager.ConnectionStrings["CabinetConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["LoggedIn"] is bool ok) || !ok)
            {
                Response.Redirect("~/Login.aspx", true);
                return;
            }

            if (!IsPostBack)
            {
                LoadPacienti();
                LoadMedici();
                LoadProgramari();
                ClearForm();
            }
        }

        private void LoadProgramari()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                // JOIN: Programari + Pacienti + Medici
                string sql = @"
                    SELECT 
                        pr.ID_Programare,
                        pr.Data_Programare,
                        CONVERT(varchar(5), pr.Ora_Programare, 108) AS Ora_Programare,
                        pr.Motiv,
                        (pa.Nume + ' ' + pa.Prenume) AS Pacient,
                        (m.Nume + ' ' + m.Prenume) AS Medic,
                        pr.ID_Pacient,
                        pr.ID_Medic
                    FROM Programari pr
                    INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
                    INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
                    ORDER BY pr.Data_Programare DESC, pr.Ora_Programare DESC;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvProgramari.DataSource = dt;
                    gvProgramari.DataBind();
                }
            }
        }

        private void LoadPacienti()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = "SELECT ID_Pacient, (Nume + ' ' + Prenume + ' (CNP: ' + CNP + ')') AS NumeComplet FROM Pacienti ORDER BY Nume, Prenume;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlPacient.DataSource = dt;
                    ddlPacient.DataTextField = "NumeComplet";
                    ddlPacient.DataValueField = "ID_Pacient";
                    ddlPacient.DataBind();
                }
            }
        }

        private void LoadMedici()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = "SELECT ID_Medic, (Nume + ' ' + Prenume + ' - ' + Specializare) AS NumeComplet FROM Medici ORDER BY Nume, Prenume;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlMedic.DataSource = dt;
                    ddlMedic.DataTextField = "NumeComplet";
                    ddlMedic.DataValueField = "ID_Medic";
                    ddlMedic.DataBind();
                }
            }
        }

        protected void gvProgramari_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            int id = Convert.ToInt32(gvProgramari.SelectedDataKey.Value);
            hfIdProgramare.Value = id.ToString();

            var row = gvProgramari.SelectedRow;
            txtData.Text = DateTime.Parse(row.Cells[2].Text).ToString("yyyy-MM-dd");
            txtOra.Text = row.Cells[3].Text;
            txtMotiv.Text = System.Web.HttpUtility.HtmlDecode(row.Cells[4].Text);

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = "SELECT ID_Pacient, ID_Medic FROM Programari WHERE ID_Programare=@id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            ddlPacient.SelectedValue = r["ID_Pacient"].ToString();
                            ddlMedic.SelectedValue = r["ID_Medic"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!ValidateForm()) return;

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO Programari (Data_Programare, Ora_Programare, Motiv, ID_Pacient, ID_Medic)
                    VALUES (@data, @ora, @motiv, @idPacient, @idMedic);
                ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@data", SqlDbType.Date).Value = DateTime.Parse(txtData.Text);
                    cmd.Parameters.Add("@ora", SqlDbType.Time).Value = TimeSpan.Parse(txtOra.Text);
                    cmd.Parameters.Add("@motiv", SqlDbType.NVarChar, 200).Value = txtMotiv.Text.Trim();
                    cmd.Parameters.Add("@idPacient", SqlDbType.Int).Value = Convert.ToInt32(ddlPacient.SelectedValue);
                    cmd.Parameters.Add("@idMedic", SqlDbType.Int).Value = Convert.ToInt32(ddlMedic.SelectedValue);

                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.CssClass = "msg ok";
            lblMsg.Text = "Programare adăugată.";
            LoadProgramari();
            ClearForm();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (string.IsNullOrWhiteSpace(hfIdProgramare.Value))
            {
                lblMsg.Text = "Selectează o programare din tabel pentru Update.";
                return;
            }
            if (!ValidateForm()) return;

            int id = Convert.ToInt32(hfIdProgramare.Value);

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = @"
                    UPDATE Programari
                    SET Data_Programare=@data, Ora_Programare=@ora, Motiv=@motiv,
                        ID_Pacient=@idPacient, ID_Medic=@idMedic
                    WHERE ID_Programare=@id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@data", SqlDbType.Date).Value = DateTime.Parse(txtData.Text);
                    cmd.Parameters.Add("@ora", SqlDbType.Time).Value = TimeSpan.Parse(txtOra.Text);
                    cmd.Parameters.Add("@motiv", SqlDbType.NVarChar, 200).Value = txtMotiv.Text.Trim();
                    cmd.Parameters.Add("@idPacient", SqlDbType.Int).Value = Convert.ToInt32(ddlPacient.SelectedValue);
                    cmd.Parameters.Add("@idMedic", SqlDbType.Int).Value = Convert.ToInt32(ddlMedic.SelectedValue);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.CssClass = "msg ok";
            lblMsg.Text = "Programare actualizată.";
            LoadProgramari();
            ClearForm();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (string.IsNullOrWhiteSpace(hfIdProgramare.Value))
            {
                lblMsg.Text = "Selectează o programare din tabel pentru Delete.";
                return;
            }

            int id = Convert.ToInt32(hfIdProgramare.Value);

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = "DELETE FROM Programari WHERE ID_Programare=@id;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.CssClass = "msg ok";
            lblMsg.Text = "Programare ștearsă.";
            LoadProgramari();
            ClearForm();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            ClearForm();
        }

        private void ClearForm()
        {
            hfIdProgramare.Value = "";
            txtData.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtOra.Text = "10:00";
            txtMotiv.Text = "";

            if (ddlPacient.Items.Count > 0) ddlPacient.SelectedIndex = 0;
            if (ddlMedic.Items.Count > 0) ddlMedic.SelectedIndex = 0;

            lblMsg.CssClass = "msg";
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtData.Text) ||
                string.IsNullOrWhiteSpace(txtOra.Text) ||
                string.IsNullOrWhiteSpace(txtMotiv.Text))
            {
                lblMsg.Text = "Completează data, ora și motivul.";
                return false;
            }

            if (!TimeSpan.TryParse(txtOra.Text, out _))
            {
                lblMsg.Text = "Ora nu este validă. Exemplu: 10:00";
                return false;
            }

            return true;
        }
    }
}
