<%@ Page Title="Pacienți" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Pacienti.aspx.cs"
    Inherits="Cabinet_Medical_Maria_Salau.Pacienti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .page-wrap {
        max-width: 1100px;
        margin: 30px auto;
    }

    .card-pro {
        background: #fff;
        border-radius: 18px;
        box-shadow: 0 10px 30px rgba(0,0,0,.12);
        padding: 22px;
        border-top: 6px solid #6a4bcc;
    }

    .title-row {
        display:flex;
        align-items:center;
        justify-content:space-between;
        gap:12px;
        margin-bottom: 14px;
    }

    .title-row h2 {
        margin:0;
        font-weight:800;
        color:#3d2a96;
    }

    .btn-pro {
        background:#6a4bcc;
        border:none;
        color:#fff;
        padding:10px 14px;
        border-radius:10px;
        font-weight:700;
        cursor:pointer;
    }
    .btn-pro:hover { background:#5331b3; }

    .btn-danger-pro{
        background:#b00020;
        border:none;
        color:#fff;
        padding:10px 14px;
        border-radius:10px;
        font-weight:700;
        cursor:pointer;
    }
    .btn-danger-pro:hover{ background:#8c0019; }

    .grid-wrap { margin-top: 10px; }

    .form-grid {
        display:grid;
        grid-template-columns: 1fr 1fr;
        gap:12px;
        margin-top: 14px;
    }

    .form-grid .full { grid-column: 1 / -1; }

    .input-pro, .select-pro {
        width:100%;
        padding:12px;
        border-radius:10px;
        border:2px solid #c7b5ff;
        background:#fbfaff;
        outline:none;
    }
    .input-pro:focus, .select-pro:focus {
        border-color:#6a4bcc;
        background:#fff;
        box-shadow: 0 0 6px rgba(106, 75, 204, .25);
    }

    .actions-row {
        display:flex;
        gap:10px;
        margin-top: 12px;
        justify-content:flex-end;
    }

    .msg {
        margin-top: 10px;
        font-weight:700;
        color:#b00020;
    }

    .hint {
        color:#666;
        margin-top:6px;
    }
</style>

<div class="page-wrap">
    <div class="card-pro">

        <div class="title-row">
            <div>
                <h2>Pacienți</h2>
                <div class="hint">Listă pacienți (JOIN cu medicul de familie) + Insert/Update/Delete</div>
            </div>

            <asp:Button ID="btnNew" runat="server" Text="Adaugă pacient" CssClass="btn-pro"
                OnClick="btnNew_Click" />
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>

        <div class="grid-wrap">
            <asp:GridView ID="gvPacienti" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered"
                DataKeyNames="ID_Pacient"
                OnSelectedIndexChanged="gvPacienti_SelectedIndexChanged">

                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="Selectează" />

                    <asp:BoundField DataField="ID_Pacient" HeaderText="ID" ReadOnly="True" />

                    <asp:BoundField DataField="Nume" HeaderText="Nume" />
                    <asp:BoundField DataField="Prenume" HeaderText="Prenume" />
                    <asp:BoundField DataField="CNP" HeaderText="CNP" />
                    <asp:BoundField DataField="Telefon" HeaderText="Telefon" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Sex" HeaderText="Sex" />

                    <asp:BoundField DataField="Medic" HeaderText="Medic (Nume Prenume)" />
                </Columns>

            </asp:GridView>
        </div>

       
        <asp:Panel ID="pnlForm" runat="server" Visible="false" style="margin-top:18px;">
            <hr />

            <asp:HiddenField ID="hfIDPacient" runat="server" />

            <div class="form-grid">
                <asp:TextBox ID="txtNume" runat="server" CssClass="input-pro" placeholder="Nume"></asp:TextBox>
                <asp:TextBox ID="txtPrenume" runat="server" CssClass="input-pro" placeholder="Prenume"></asp:TextBox>

                <asp:TextBox ID="txtCNP" runat="server" CssClass="input-pro" placeholder="CNP (13 cifre)"></asp:TextBox>
                <asp:TextBox ID="txtTelefon" runat="server" CssClass="input-pro" placeholder="Telefon"></asp:TextBox>

                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-pro" placeholder="Email"></asp:TextBox>

                <asp:DropDownList ID="ddlSex" runat="server" CssClass="select-pro">
                    <asp:ListItem Text="F" Value="F"></asp:ListItem>
                    <asp:ListItem Text="M" Value="M"></asp:ListItem>
                </asp:DropDownList>

                <asp:TextBox ID="txtDataNasterii" runat="server" CssClass="input-pro" placeholder="Data nașterii (YYYY-MM-DD)"></asp:TextBox>
                <asp:TextBox ID="txtDataInscriere" runat="server" CssClass="input-pro" placeholder="Data înscrierii (YYYY-MM-DD)"></asp:TextBox>

                <asp:TextBox ID="txtAdresa" runat="server" CssClass="input-pro full" placeholder="Adresă"></asp:TextBox>

                <asp:DropDownList ID="ddlMedic" runat="server" CssClass="select-pro full"></asp:DropDownList>
            </div>

            <div class="actions-row">
                <asp:Button ID="btnSave" runat="server" Text="Salvează" CssClass="btn-pro" OnClick="btnSave_Click" />
                <asp:Button ID="btnUpdate" runat="server" Text="Actualizează" CssClass="btn-pro" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Șterge" CssClass="btn-danger-pro" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('Sigur vrei să ștergi pacientul selectat?');" />
                <asp:Button ID="btnCancel" runat="server" Text="Renunță" CssClass="btn-pro" OnClick="btnCancel_Click" />
            </div>
        </asp:Panel>

    </div>
</div>

</asp:Content>
