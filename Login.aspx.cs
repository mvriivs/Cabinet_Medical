using System;
using System.Data;
using System.Data.SqlClient;

namespace Cabinet_Medical_Maria_Salau
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] is bool loggedIn && loggedIn)
            {
                Response.Redirect("~/Menu.aspx", true);
                return;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            string email = txtEmail.Text.Trim();
            string cnp = txtCNP.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(cnp))
            {
                lblMsg.Text = "Completează email și CNP.";
                return;
            }

            string connStr = System.Configuration.ConfigurationManager
                .ConnectionStrings["CabinetConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT TOP 1 ID_Medic, ISNULL(IsAdmin, 0) AS IsAdmin
                        FROM Medici
                        WHERE Email = @Email AND CNP = @CNP;
                    ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
                        cmd.Parameters.Add("@CNP", SqlDbType.VarChar, 13).Value = cnp;

                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                Session["LoggedIn"] = true;
                                Session["UserEmail"] = email;
                                Session["ID_Medic"] = Convert.ToInt32(r["ID_Medic"]);
                                Session["IsAdmin"] = Convert.ToInt32(r["IsAdmin"]) == 1;

                                Response.Redirect("~/Menu.aspx", true);
                                return;
                            }
                        }
                    }
                }

                lblMsg.Text = "Date greșite!";
            }
            catch
            {
                lblMsg.Text = "Eroare la conectarea la baza de date.";
            }
        }
    }
}
