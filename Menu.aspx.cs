using System;

namespace Cabinet_Medical_Maria_Salau
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] == null || !(Session["LoggedIn"] is bool) || !(bool)Session["LoggedIn"])
            {
                Response.Redirect("~/Login.aspx", true);
                return;
            }
        }

        protected void BtnPacienti_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pacienti.aspx", true);
        }

        protected void BtnProgramari_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Programari.aspx", true);
        }

        protected void BtnConsultatii_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Consultatii.aspx", true);
        }

        protected void BtnStatistici_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Statistici.aspx", true);
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx", true);
        }
    }
}
