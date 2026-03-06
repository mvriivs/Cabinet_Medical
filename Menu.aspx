<%@ Page Title="Meniu principal" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs"
    Inherits="Cabinet_Medical_Maria_Salau.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .menu-wrapper {
        min-height: 80vh;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .menu-card {
        width: 700px;
        background: #ffffff;
        padding: 50px;
        border-radius: 20px;
        box-shadow: 0 12px 40px rgba(0,0,0,0.15);
        text-align: center;
    }

    .menu-title {
        font-size: 32px;
        font-weight: 800;
        color: #4a2ba8;
        margin-bottom: 35px;
    }

    .menu-buttons {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 25px;
    }

    .menu-btn {
        padding: 18px;
        font-size: 18px;
        font-weight: 700;
        border-radius: 12px;
        border: none;
        cursor: pointer;
        background: linear-gradient(135deg, #6a4bcc, #5331b3);
        color: white;
        transition: 0.25s;
    }

    .menu-btn:hover {
        transform: translateY(-3px);
        background: linear-gradient(135deg, #5331b3, #6a4bcc);
    }

    .logout-btn {
        margin-top: 30px;
        background: #b00020;
    }

    .logout-btn:hover {
        background: #8a0019;
    }
</style>

<div class="menu-wrapper">

    <div class="menu-card">

        <div class="menu-title">
            Cabinet Medical MS<br />
            <span style="font-size:18px;font-weight:400;color:#666;">
                Panou de administrare
            </span>
        </div>

        <div class="menu-buttons">

            <asp:Button runat="server" Text="Pacienți"
                CssClass="menu-btn"
                OnClick="BtnPacienti_Click" />

            <asp:Button runat="server" Text="Programări"
                CssClass="menu-btn"
                OnClick="BtnProgramari_Click" />

            <asp:Button runat="server" Text="Consultații"
                CssClass="menu-btn"
                OnClick="BtnConsultatii_Click" />

            <asp:Button runat="server" Text="Statistici"
                CssClass="menu-btn"
                OnClick="BtnStatistici_Click" />

        </div>

        <asp:Button runat="server" Text="Logout"
            CssClass="menu-btn logout-btn"
            OnClick="BtnLogout_Click" />

    </div>

</div>

</asp:Content>
