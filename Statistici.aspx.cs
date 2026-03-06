using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Cabinet_Medical_Maria_Salau
{
    public partial class Statistici : System.Web.UI.Page
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
             
                var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var end = DateTime.Today;

                txtFrom.Text = start.ToString("yyyy-MM-dd");
                txtTo.Text = end.ToString("yyyy-MM-dd");
                txtMin.Text = "2";

                LoadMediciDropdown();
                RunAllStats();
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            RunAllStats();
        }

        private void LoadMediciDropdown()
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = "SELECT ID_Medic, (Nume + ' ' + Prenume + ' - ' + Specializare) AS Afisare FROM Medici ORDER BY Nume, Prenume;";
                using (var cmd = new SqlCommand(sql, conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlMedic.DataSource = dt;
                    ddlMedic.DataTextField = "Afisare";
                    ddlMedic.DataValueField = "ID_Medic";
                    ddlMedic.DataBind();

                    ddlMedic.Items.Insert(0, new ListItem("Toți medicii", ""));
                }
            }
        }

        private void RunAllStats()
        {
            lblMsg.CssClass = "msg";
            lblMsg.Text = "";

            DateTime fromDate, toDate;
            if (!DateTime.TryParse(txtFrom.Text, out fromDate) || !DateTime.TryParse(txtTo.Text, out toDate))
            {
                lblMsg.Text = "Interval de date invalid.";
                return;
            }

            int min;
            if (!int.TryParse(txtMin.Text.Trim(), out min)) min = 2;

            int? idMedic = null;
            if (!string.IsNullOrWhiteSpace(ddlMedic.SelectedValue))
                idMedic = int.Parse(ddlMedic.SelectedValue);

            try
            {
                //  SIMPLE JOIN (6) 
                BindGrid(gvS1, Query_S1_Programari(fromDate, toDate, idMedic));
                BindGrid(gvS2, Query_S2_PacientiMedici());
                BindGrid(gvS3, Query_S3_Consultatii(fromDate, toDate, idMedic));
                BindGrid(gvS4, Query_S4_Retete(fromDate, toDate, idMedic));
                BindGrid(gvS5, Query_S5_TratamenteRetete());
                BindGrid(gvS6, Query_S6_ConsultatiiPerMedic(fromDate, toDate));

                //  COMPLEXE (4) 
                BindGrid(gvC1, Query_C1_MediciPesteMedie());
                BindGrid(gvC2, Query_C2_PacientiFaraConsultatii());
                BindGrid(gvC3, Query_C3_MedCelMaiPrescris());
                BindGrid(gvC4, Query_C4_PacientiCuMinConsultatii(fromDate, toDate, min));

                lblMsg.CssClass = "msg ok";
                lblMsg.Text = "Statistici actualizate.";
            }
            catch
            {
                lblMsg.CssClass = "msg";
                lblMsg.Text = "Eroare la rularea statisticilor (verifică numele tabelelor/coloanelor).";
            }
        }

        private void BindGrid(GridView gv, DataTable dt)
        {
            gv.DataSource = dt;
            gv.DataBind();
        }

        private DataTable Exec(string sql, params SqlParameter[] p)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (p != null && p.Length > 0)
                        cmd.Parameters.AddRange(p);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // 6 SIMPLE JOIN

        private DataTable Query_S1_Programari(DateTime from, DateTime to, int? idMedic)
        {
            string sql = @"
SELECT
    pr.ID_Programare AS [Programare],
    CONVERT(varchar(10), pr.Data_Programare, 120) AS [Data],
    LEFT(CONVERT(varchar(8), pr.Ora_Programare, 108), 5) AS [Ora],
    pr.Motiv,
    (pa.Nume + ' ' + pa.Prenume) AS [Pacient],
    (m.Nume + ' ' + m.Prenume) AS [Medic]
FROM Programari pr
INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
WHERE pr.Data_Programare BETWEEN @from AND @to
  AND (@idMedic IS NULL OR pr.ID_Medic = @idMedic)
ORDER BY pr.Data_Programare DESC, pr.Ora_Programare DESC;";

            return Exec(sql,
                new SqlParameter("@from", SqlDbType.Date) { Value = from.Date },
                new SqlParameter("@to", SqlDbType.Date) { Value = to.Date },
                new SqlParameter("@idMedic", SqlDbType.Int) { Value = (object)idMedic ?? DBNull.Value }
            );
        }

        private DataTable Query_S2_PacientiMedici()
        {
            string sql = @"
SELECT
    pa.ID_Pacient AS [ID],
    pa.Nume,
    pa.Prenume,
    pa.CNP,
    pa.Telefon,
    pa.Email,
    pa.Sex,
    (m.Nume + ' ' + m.Prenume) AS [Medic (Nume Prenume)]
FROM Pacienti pa
INNER JOIN Medici m ON m.ID_Medic = pa.ID_Medic
ORDER BY pa.Nume, pa.Prenume;";

            return Exec(sql);
        }

        
        private DataTable Query_S3_Consultatii(DateTime from, DateTime to, int? idMedic)
        {
            string sql = @"
SELECT
    c.ID_Consultatie AS [ID],
    CONVERT(varchar(10), c.Data_Consultatie, 120) AS [Data consultatiei],
    c.Diagnostic,
    c.Tensiune_Arteriala,
    c.Greutate,
    c.Puls,
    (pa.Nume + ' ' + pa.Prenume) AS [Pacient],
    (m.Nume + ' ' + m.Prenume) AS [Medic]
FROM Consultatii c
INNER JOIN Programari pr ON pr.ID_Programare = c.ID_Programare
INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
WHERE c.Data_Consultatie BETWEEN @from AND @to
  AND (@idMedic IS NULL OR pr.ID_Medic = @idMedic)
ORDER BY c.Data_Consultatie DESC;";

            return Exec(sql,
                new SqlParameter("@from", SqlDbType.Date) { Value = from.Date },
                new SqlParameter("@to", SqlDbType.Date) { Value = to.Date },
                new SqlParameter("@idMedic", SqlDbType.Int) { Value = (object)idMedic ?? DBNull.Value }
            );
        }

       
        private DataTable Query_S4_Retete(DateTime from, DateTime to, int? idMedic)
        {
            string sql = @"
SELECT
    r.ID_Reteta AS [Reteta],
    CONVERT(varchar(10), r.Data_Eliberare, 120) AS [Data eliberare],
    (pa.Nume + ' ' + pa.Prenume) AS [Pacient],
    (m.Nume + ' ' + m.Prenume) AS [Medic],
    c.Diagnostic
FROM Retete r
INNER JOIN Consultatii c ON c.ID_Consultatie = r.ID_Consultatie
INNER JOIN Programari pr ON pr.ID_Programare = c.ID_Programare
INNER JOIN Pacienti pa ON pa.ID_Pacient = pr.ID_Pacient
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
WHERE r.Data_Eliberare BETWEEN @from AND @to
  AND (@idMedic IS NULL OR pr.ID_Medic = @idMedic)
ORDER BY r.Data_Eliberare DESC;";

            return Exec(sql,
                new SqlParameter("@from", SqlDbType.Date) { Value = from.Date },
                new SqlParameter("@to", SqlDbType.Date) { Value = to.Date },
                new SqlParameter("@idMedic", SqlDbType.Int) { Value = (object)idMedic ?? DBNull.Value }
            );
        }

        private DataTable Query_S5_TratamenteRetete()
        {
            string sql = @"
SELECT
    rt.ID_Reteta AS [Reteta],
    t.Medicament,
    rt.Dozaj AS [Dozaj (reteta)],
    rt.Durata AS [Durata (reteta)]
FROM Retete_Tratamente rt
INNER JOIN Tratamente t ON t.ID_Tratament = rt.ID_Tratament
INNER JOIN Retete r ON r.ID_Reteta = rt.ID_Reteta
ORDER BY rt.ID_Reteta, t.Medicament;";

            return Exec(sql);
        }

        private DataTable Query_S6_ConsultatiiPerMedic(DateTime from, DateTime to)
        {
            string sql = @"
SELECT
    (m.Nume + ' ' + m.Prenume) AS [Medic],
    COUNT(*) AS [Nr consultatii]
FROM Consultatii c
INNER JOIN Programari pr ON pr.ID_Programare = c.ID_Programare
INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
WHERE c.Data_Consultatie BETWEEN @from AND @to
GROUP BY m.Nume, m.Prenume
ORDER BY [Nr consultatii] DESC;";

            return Exec(sql,
                new SqlParameter("@from", SqlDbType.Date) { Value = from.Date },
                new SqlParameter("@to", SqlDbType.Date) { Value = to.Date }
            );
        }

       
        // 4 COMPLEXE (SUBQUERY)
      
        private DataTable Query_C1_MediciPesteMedie()
        {
            string sql = @"
SELECT x.Medic, x.NrConsultatii
FROM (
    SELECT (m.Nume + ' ' + m.Prenume) AS Medic, COUNT(*) AS NrConsultatii
    FROM Consultatii c
    INNER JOIN Programari pr ON pr.ID_Programare = c.ID_Programare
    INNER JOIN Medici m ON m.ID_Medic = pr.ID_Medic
    GROUP BY m.Nume, m.Prenume
) x
WHERE x.NrConsultatii > (
    SELECT AVG(CAST(y.NrConsultatii AS float))
    FROM (
        SELECT COUNT(*) AS NrConsultatii
        FROM Consultatii c2
        INNER JOIN Programari pr2 ON pr2.ID_Programare = c2.ID_Programare
        GROUP BY pr2.ID_Medic
    ) y
)
ORDER BY x.NrConsultatii DESC;";

            return Exec(sql);
        }

        private DataTable Query_C2_PacientiFaraConsultatii()
        {
            string sql = @"
SELECT pa.ID_Pacient AS [ID], pa.Nume, pa.Prenume, pa.CNP, pa.Telefon, pa.Email
FROM Pacienti pa
WHERE NOT EXISTS (
    SELECT 1
    FROM Programari pr
    INNER JOIN Consultatii c ON c.ID_Programare = pr.ID_Programare
    WHERE pr.ID_Pacient = pa.ID_Pacient
)
ORDER BY pa.Nume, pa.Prenume;";

            return Exec(sql);
        }

        private DataTable Query_C3_MedCelMaiPrescris()
        {
            string sql = @"
SELECT t.Medicament, cnt.Nr
FROM (
    SELECT rt.ID_Tratament, COUNT(*) AS Nr
    FROM Retete_Tratamente rt
    GROUP BY rt.ID_Tratament
) cnt
INNER JOIN Tratamente t ON t.ID_Tratament = cnt.ID_Tratament
WHERE cnt.Nr = (
    SELECT MAX(Nr) FROM (
        SELECT COUNT(*) AS Nr
        FROM Retete_Tratamente
        GROUP BY ID_Tratament
    ) q
);";

            return Exec(sql);
        }

        private DataTable Query_C4_PacientiCuMinConsultatii(DateTime from, DateTime to, int min)
        {
            string sql = @"
SELECT pa.ID_Pacient AS [ID], pa.Nume, pa.Prenume, x.NrConsultatii
FROM Pacienti pa
INNER JOIN (
    SELECT pr.ID_Pacient, COUNT(*) AS NrConsultatii
    FROM Consultatii c
    INNER JOIN Programari pr ON pr.ID_Programare = c.ID_Programare
    WHERE c.Data_Consultatie BETWEEN @from AND @to
    GROUP BY pr.ID_Pacient
) x ON x.ID_Pacient = pa.ID_Pacient
WHERE x.NrConsultatii >= @min
ORDER BY x.NrConsultatii DESC, pa.Nume, pa.Prenume;";

            return Exec(sql,
                new SqlParameter("@from", SqlDbType.Date) { Value = from.Date },
                new SqlParameter("@to", SqlDbType.Date) { Value = to.Date },
                new SqlParameter("@min", SqlDbType.Int) { Value = min }
            );
        }
    }
}
