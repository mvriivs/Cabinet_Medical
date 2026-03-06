<%@ Page Title="Programări" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Programari.aspx.cs"
    Inherits="Cabinet_Medical_Maria_Salau.Programari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .page-card {
        background: #fff;
        border-radius: 18px;
        box-shadow: 0 12px 40px rgba(0,0,0,0.08);
        padding: 28px;
        border-top: 6px solid #6a4bcc;
        margin-top: 20px;
    }

    .title-row {
        display:flex; align-items:center; justify-content:space-between;
        gap:16px; margin-bottom:14px;
    }

    .page-title{
        margin:0;
        font-size:40px;
        font-weight:900;
        color:#3f2aa8;
        letter-spacing:.2px;
    }

    .btn-pro {
        padding: 12px 18px;
        border: none;
        border-radius: 12px;
        font-weight: 800;
        cursor: pointer;
        color: #fff;
        background: linear-gradient(135deg, #6a4bcc, #4a7bd9);
        transition: .2s;
    }
    .btn-pro:hover { transform: translateY(-2px); }

    .btn-danger-pro{
        background: #b00020;
    }

    .grid-wrap { margin-top: 14px; overflow:auto; }

    .form-grid{
        margin-top:22px;
        display:grid;
        grid-template-columns: 1fr 1fr;
        gap:16px 18px;
    }

    .input-pro{
        width:100%;
        padding:12px 14px;
        border-radius:12px;
        border:2px solid #c7b5ff;
        background:#fbfaff;
        outline:none;
    }
    .input-pro:focus{
        border-color:#6a4bcc;
        box-shadow: 0 0 0 4px rgba(106,75,204,.12);
        background:#fff;
    }

    .actions-row{
        margin-top:18px;
        display:flex;
        gap:12px;
        justify-content:flex-end;
        flex-wrap:wrap;
    }

    .hint{ color:#666; margin:6px 0 0 0; }

    .msg{
        margin-top:12px;
        font-weight:700;
        color:#c51616;
    }
    .ok{ color:#1a7f37; }

 
    body { background: linear-gradient(135deg, #f2ecff, #eef3ff) !important; }
</style>

<div class="page-card">

    <div class="title-row">
        <div>
            <h1 class="page-title">Programări</h1>
            <p class="hint">Listă programări (JOIN Pacienți + Medici) + Insert/Update/Delete</p>
        </div>

        <asp:Button ID="btnClear" runat="server" CssClass="btn-pro"
            Text="Adaugă programare"
            OnClick="btnClear_Click" />
    </div>

    <div class="grid-wrap">
        <asp:GridView ID="gvProgramari" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered"
            DataKeyNames="ID_Programare"
            OnSelectedIndexChanged="gvProgramari_SelectedIndexChanged">

            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="Selectează" />

                <asp:BoundField DataField="ID_Programare" HeaderText="ID" />
                <asp:BoundField DataField="Data_Programare" HeaderText="Data" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Ora_Programare" HeaderText="Ora" />
                <asp:BoundField DataField="Motiv" HeaderText="Motiv" />
                <asp:BoundField DataField="Pacient" HeaderText="Pacient" />
                <asp:BoundField DataField="Medic" HeaderText="Medic" />
            </Columns>
        </asp:GridView>
    </div>

    <asp:HiddenField ID="hfIdProgramare" runat="server" />

    <div class="form-grid">
        <asp:TextBox ID="txtData" runat="server" CssClass="input-pro" TextMode="Date" />
        <asp:TextBox ID="txtOra" runat="server" CssClass="input-pro" placeholder="Ora (ex: 10:00)" />

        <asp:TextBox ID="txtMotiv" runat="server" CssClass="input-pro" placeholder="Motiv" />
        <asp:DropDownList ID="ddlPacient" runat="server" CssClass="input-pro" />

        <asp:DropDownList ID="ddlMedic" runat="server" CssClass="input-pro" />
        <div></div>
    </div>

    <div class="actions-row">
        <asp:Button ID="btnInsert" runat="server" CssClass="btn-pro" Text="Salvează (Insert)"
            OnClick="btnInsert_Click" />
        <asp:Button ID="btnUpdate" runat="server" CssClass="btn-pro" Text="Actualizează (Update)"
            OnClick="btnUpdate_Click" />
        <asp:Button ID="btnDelete" runat="server" CssClass="btn-pro btn-danger-pro" Text="Șterge (Delete)"
            OnClick="btnDelete_Click" />
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="msg" />

</div>

</asp:Content>
