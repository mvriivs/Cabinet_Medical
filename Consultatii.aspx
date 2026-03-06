<%@ Page Title="Consultații" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Consultatii.aspx.cs"
    Inherits="Cabinet_Medical_Maria_Salau.Consultatii" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .page { padding: 24px 0 40px; }
    .card {
        background: #fff;
        border-radius: 18px;
        box-shadow: 0 10px 30px rgba(0,0,0,0.10);
        padding: 28px;
        border-top: 6px solid #6a4bcc;
    }
    .header{
        display:flex; align-items:flex-end; justify-content:space-between;
        gap:18px; margin-bottom:18px;
    }
    .title h1{
        margin:0; font-size:36px; font-weight:900; color:#3d2aa0; letter-spacing:0.3px;
    }
    .title p{ margin:6px 0 0; color:#666; font-size:15px; }

    .btn-primary-pro{
        background: linear-gradient(135deg, #6a4bcc, #5331b3);
        border:none; color:#fff; padding:12px 18px; border-radius:12px;
        font-weight:800; cursor:pointer; transition:0.2s;
    }
    .btn-primary-pro:hover{ transform: translateY(-2px); }

    .btn-danger-pro{
        background:#b00020; border:none; color:#fff; padding:12px 18px;
        border-radius:12px; font-weight:800; cursor:pointer; transition:0.2s;
    }
    .btn-danger-pro:hover{ transform: translateY(-2px); background:#8a0019; }

    .grid-wrap{
        margin-top:16px; overflow:auto; border-radius:14px; border:1px solid #e6e6ef;
    }

    .form{
        margin-top: 20px; background:#fbfaff; border:2px solid #e7ddff;
        border-radius:16px; padding:18px;
    }

    .row2{
        display:grid; grid-template-columns: 1fr 1fr; gap:14px;
    }
    @media(max-width:900px){
        .row2{ grid-template-columns:1fr; }
        .header{ flex-direction:column; align-items:flex-start; }
    }

    .field label{
        display:block; font-weight:800; color:#3d2aa0;
        margin:10px 0 6px; font-size:13px;
    }
    .input{
        width:100%; padding:12px 12px; border-radius:12px;
        border:2px solid #c7b5ff; background:#fff; outline:none;
    }
    .input:focus{
        border-color:#6a4bcc; box-shadow:0 0 0 3px rgba(106,75,204,0.15);
    }

    .actions{ margin-top:14px; display:flex; gap:10px; flex-wrap:wrap; }

    .msg{ display:block; margin-top:10px; font-weight:700; }
    .msg.ok{ color:#0a7a2f; }
    .msg.err{ color:#b00020; }
</style>

<div class="page">
    <div class="card">

        <div class="header">
            <div class="title">
                <h1>Consultații</h1>
                <p>Listă consultații (JOIN prin Programări → Pacienți + Medici) + Insert / Update / Delete</p>
            </div>

            <asp:Button ID="btnNew" runat="server" Text="Adaugă consultație"
                CssClass="btn-primary-pro" OnClick="btnNew_Click" />
        </div>

        <div class="grid-wrap">
            <asp:GridView ID="gvConsultatii" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped"
                DataKeyNames="ID_Consultatie"
                OnSelectedIndexChanged="gvConsultatii_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="Selectează" />
                    <asp:BoundField DataField="ID_Consultatie" HeaderText="ID" />
                    <asp:BoundField DataField="Data_Consultatie" HeaderText="Data" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Diagnostic" HeaderText="Diagnostic" />
                    <asp:BoundField DataField="Tensiune_Arteriala" HeaderText="Tensiune" />
                    <asp:BoundField DataField="Greutate" HeaderText="Greutate" />
                    <asp:BoundField DataField="Puls" HeaderText="Puls" />
                    <asp:BoundField DataField="Pacient" HeaderText="Pacient" />
                    <asp:BoundField DataField="Medic" HeaderText="Medic" />
                </Columns>
            </asp:GridView>
        </div>

        <asp:Panel ID="pnlForm" runat="server" CssClass="form" Visible="false">
            <div class="row2">

                <div class="field">
                    <label>Programare (Pacient • Medic • Data/Oră)</label>
                    <asp:DropDownList ID="ddlProgramare" runat="server" CssClass="input"></asp:DropDownList>
                </div>

                <div class="field">
                    <label>Data consultației</label>
                    <asp:TextBox ID="txtData" runat="server" CssClass="input" TextMode="Date"></asp:TextBox>
                </div>

                <div class="field">
                    <label>Diagnostic</label>
                    <asp:TextBox ID="txtDiagnostic" runat="server" CssClass="input" placeholder="Ex: Stare normală"></asp:TextBox>
                </div>

                <div class="field">
                    <label>Tensiune arterială</label>
                    <asp:TextBox ID="txtTA" runat="server" CssClass="input" placeholder="Ex: 120/80"></asp:TextBox>
                </div>

                <div class="field">
                    <label>Greutate</label>
                    <asp:TextBox ID="txtGreutate" runat="server" CssClass="input" placeholder="Ex: 72.5"></asp:TextBox>
                </div>

                <div class="field">
                    <label>Puls</label>
                    <asp:TextBox ID="txtPuls" runat="server" CssClass="input" placeholder="Ex: 72"></asp:TextBox>
                </div>

            </div>

            <div class="actions">
                <asp:Button ID="btnInsert" runat="server" Text="Salvează (Insert)"
                    CssClass="btn-primary-pro" OnClick="btnInsert_Click" />
                <asp:Button ID="btnUpdate" runat="server" Text="Actualizează (Update)"
                    CssClass="btn-primary-pro" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Șterge (Delete)"
                    CssClass="btn-danger-pro" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('Sigur vrei să ștergi consultația selectată?');" />
                <asp:Button ID="btnCancel" runat="server" Text="Renunță"
                    CssClass="btn-danger-pro" OnClick="btnCancel_Click" />
            </div>

            <asp:HiddenField ID="hfIdConsultatie" runat="server" />
            <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>
        </asp:Panel>

    </div>
</div>

</asp:Content>
