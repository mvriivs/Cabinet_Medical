using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Cabinet_Medical_Maria_Salau
{
    public partial class Consultatii : System.Web.UI.Page
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
                LoadProgramariDisponibile(); 
                LoadConsultatii();
                ClearForm();
            }
        }

        
        private void LoadConsultatii()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = @"
SELECT
    c.ID_Consultatie,
    c.Data_Consultatie,
    c.Tensiune_Arteriala,
    c.Greutate,
    c.Puls,
    c.Diagnostic,
    c.ID_Programare,
    (pa.Nume + ' ' + pa.Prenume) AS Pacient,
    (m.Nume + ' ' + m.Prenume) AS Medic
FROM Consultatii c
INNER JOIN Programari pr ON pr.ID_Programare = c.ID_Programare
INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
ORDER BY c.Data_Consultatie DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvConsultatii.DataSource = dt;
                    gvConsultatii.DataBind();
                }
            }
        }

        // Dropdown: programari care NU au inca consultatie (evita UNIQUE pe ID_Programare)
        private void LoadProgramariDisponibile()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = @"
SELECT 
    pr.ID_Programare,
    (pa.Nume + ' ' + pa.Prenume + ' • ' + m.Nume + ' ' + m.Prenume + ' • ' 
      + CONVERT(varchar(10), pr.Data_Programare, 120) + ' ' 
      + LEFT(CONVERT(varchar(8), pr.Ora_Programare, 108), 5)
    ) AS Afisare
FROM Programari pr
INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
LEFT JOIN Consultatii c ON c.ID_Programare = pr.ID_Programare
WHERE c.ID_Programare IS NULL
ORDER BY pr.Data_Programare DESC, pr.Ora_Programare DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlProgramare.DataSource = dt;
                    ddlProgramare.DataTextField = "Afisare";
                    ddlProgramare.DataValueField = "ID_Programare";
                    ddlProgramare.DataBind();
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "msg";
            pnlForm.Visible = true;

            LoadProgramariDisponibile();
            ClearForm();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "msg";
            pnlForm.Visible = false;
            ClearForm();
        }

        // Select din GridView -> populam formular
        protected void gvConsultatii_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "msg";
            pnlForm.Visible = true;

            int id = Convert.ToInt32(gvConsultatii.SelectedDataKey.Value);
            hfIdConsultatie.Value = id.ToString();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = @"
SELECT Data_Consultatie, Tensiune_Arteriala, Greutate, Puls, Diagnostic, ID_Programare
FROM Consultatii
WHERE ID_Consultatie = @id;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            txtData.Text = Convert.ToDateTime(r["Data_Consultatie"]).ToString("yyyy-MM-dd");
                            txtTA.Text = r["Tensiune_Arteriala"] == DBNull.Value ? "" : r["Tensiune_Arteriala"].ToString();
                            txtGreutate.Text = r["Greutate"] == DBNull.Value ? "" : Convert.ToDecimal(r["Greutate"]).ToString("0.##");
                            txtPuls.Text = r["Puls"] == DBNull.Value ? "" : r["Puls"].ToString();
                            txtDiagnostic.Text = r["Diagnostic"] == DBNull.Value ? "" : r["Diagnostic"].ToString();

                            int idProg = Convert.ToInt32(r["ID_Programare"]);

                           
                            LoadProgramariDisponibile();
                            EnsureProgramareExistsInDropdown(idProg);

                            ddlProgramare.SelectedValue = idProg.ToString();
                        }
                    }
                }
            }
        }

        private void EnsureProgramareExistsInDropdown(int idProg)
        {
            if (ddlProgramare.Items.FindByValue(idProg.ToString()) != null)
                return;

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = @"
SELECT TOP 1
    pr.ID_Programare,
    (pa.Nume + ' ' + pa.Prenume + ' • ' + m.Nume + ' ' + m.Prenume + ' • ' 
      + CONVERT(varchar(10), pr.Data_Programare, 120) + ' ' 
      + LEFT(CONVERT(varchar(8), pr.Ora_Programare, 108), 5)
    ) AS Afisare
FROM Programari pr
INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
WHERE pr.ID_Programare = @idProg;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@idProg", SqlDbType.Int).Value = idProg;

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            ddlProgramare.Items.Insert(0,
                                new ListItem(
                                    r["Afisare"].ToString(),
                                    r["ID_Programare"].ToString()
                                )
                            );
                        }
                    }
                }
            }
        }

        // INSERT
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "msg";

            if (!ValidateForm()) return;

            if (ddlProgramare.Items.Count == 0)
            {
                lblMsg.CssClass = "msg err";
                lblMsg.Text = "Nu există programări disponibile (fără consultație).";
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = @"
INSERT INTO Consultatii
    (Data_Consultatie, Tensiune_Arteriala, Greutate, Puls, Diagnostic, ID_Programare)
VALUES
    (@data, @ta, @greutate, @puls, @diag, @idProg);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@data", SqlDbType.Date).Value = DateTime.Parse(txtData.Text);

                    cmd.Parameters.Add("@ta", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(txtTA.Text) ? (object)DBNull.Value : txtTA.Text.Trim();

                    cmd.Parameters.Add("@greutate", SqlDbType.Decimal).Value =
                        string.IsNullOrWhiteSpace(txtGreutate.Text) ? (object)DBNull.Value : decimal.Parse(txtGreutate.Text);

                    cmd.Parameters.Add("@puls", SqlDbType.Int).Value =
                        string.IsNullOrWhiteSpace(txtPuls.Text) ? (object)DBNull.Value : int.Parse(txtPuls.Text);

                    cmd.Parameters.Add("@diag", SqlDbType.NVarChar, 255).Value = txtDiagnostic.Text.Trim();
                    cmd.Parameters.Add("@idProg", SqlDbType.Int).Value = int.Parse(ddlProgramare.SelectedValue);

                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.CssClass = "msg ok";
            lblMsg.Text = "✅ Consultație adăugată.";

            LoadConsultatii();
            LoadProgramariDisponibile();
            ClearForm();
        }

        // UPDATE
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "msg";

            if (string.IsNullOrWhiteSpace(hfIdConsultatie.Value))
            {
                lblMsg.CssClass = "msg err";
                lblMsg.Text = "Selectează o consultație pentru Update.";
                return;
            }

            if (!ValidateForm()) return;

            int id = int.Parse(hfIdConsultatie.Value);

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = @"
UPDATE Consultatii
SET Data_Consultatie=@data,
    Tensiune_Arteriala=@ta,
    Greutate=@greutate,
    Puls=@puls,
    Diagnostic=@diag,
    ID_Programare=@idProg
WHERE ID_Consultatie=@id;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@data", SqlDbType.Date).Value = DateTime.Parse(txtData.Text);

                    cmd.Parameters.Add("@ta", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(txtTA.Text) ? (object)DBNull.Value : txtTA.Text.Trim();

                    cmd.Parameters.Add("@greutate", SqlDbType.Decimal).Value =
                        string.IsNullOrWhiteSpace(txtGreutate.Text) ? (object)DBNull.Value : decimal.Parse(txtGreutate.Text);

                    cmd.Parameters.Add("@puls", SqlDbType.Int).Value =
                        string.IsNullOrWhiteSpace(txtPuls.Text) ? (object)DBNull.Value : int.Parse(txtPuls.Text);

                    cmd.Parameters.Add("@diag", SqlDbType.NVarChar, 255).Value = txtDiagnostic.Text.Trim();
                    cmd.Parameters.Add("@idProg", SqlDbType.Int).Value = int.Parse(ddlProgramare.SelectedValue);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.CssClass = "msg ok";
            lblMsg.Text = "✅ Consultație actualizată.";

            LoadConsultatii();
            LoadProgramariDisponibile();
            ClearForm();
        }

        // DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "msg";

            if (string.IsNullOrWhiteSpace(hfIdConsultatie.Value))
            {
                lblMsg.CssClass = "msg err";
                lblMsg.Text = "Selectează o consultație pentru Delete.";
                return;
            }

            int id = int.Parse(hfIdConsultatie.Value);

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                string sql = "DELETE FROM Consultatii WHERE ID_Consultatie=@id;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.CssClass = "msg ok";
            lblMsg.Text = "✅ Consultație ștearsă.";

            LoadConsultatii();
            LoadProgramariDisponibile();
            ClearForm();
        }

        private void ClearForm()
        {
            hfIdConsultatie.Value = "";
            txtData.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtTA.Text = "";
            txtGreutate.Text = "";
            txtPuls.Text = "";
            txtDiagnostic.Text = "";

            if (ddlProgramare.Items.Count > 0)
                ddlProgramare.SelectedIndex = 0;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtData.Text) || string.IsNullOrWhiteSpace(txtDiagnostic.Text))
            {
                lblMsg.CssClass = "msg err";
                lblMsg.Text = "Completează data și diagnosticul.";
                return false;
            }
            return true;
        }
    }
}
