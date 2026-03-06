<%@ Page Title="Statistici" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Statistici.aspx.cs"
    Inherits="Cabinet_Medical_Maria_Salau.Statistici" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .page { padding: 24px 0 40px; }
    .card{
        background:#fff; border-radius:18px; box-shadow:0 10px 30px rgba(0,0,0,0.10);
        padding:28px; border-top:6px solid #6a4bcc;
    }
    .title h1{ margin:0; font-size:36px; font-weight:900; color:#3d2aa0; }
    .title p{ margin:8px 0 0; color:#666; }

    .filters{
        margin-top:18px;
        background:#fbfaff;
        border:2px solid #e7ddff;
        border-radius:16px;
        padding:16px;
        display:grid;
        grid-template-columns: 1.2fr 1fr 1fr 1fr;
        gap:12px;
        align-items:end;
    }
    @media(max-width:1100px){ .filters{ grid-template-columns: 1fr; } }

    .field label{ display:block; font-weight:800; color:#3d2aa0; margin:0 0 6px; font-size:13px; }
    .input{
        width:100%; padding:12px; border-radius:12px; border:2px solid #c7b5ff;
        background:#fff; outline:none;
    }
    .input:focus{ border-color:#6a4bcc; box-shadow:0 0 0 3px rgba(106,75,204,0.15); }

    .btn{
        padding:12px 16px; border-radius:12px; border:none; cursor:pointer;
        font-weight:900; color:#fff; background:linear-gradient(135deg,#6a4bcc,#2f6bff);
    }
    .btn:hover{ transform: translateY(-1px); }

    .section{ margin-top:22px; }
    .section h2{
        margin:0 0 10px; font-size:20px; font-weight:900; color:#3d2aa0;
    }
    .hint{ color:#666; margin:0 0 10px; font-size:14px; }

    .grid-wrap{
        overflow:auto; border-radius:14px; border:1px solid #e6e6ef;
        background:#fff;
    }
    .msg{
        display:block; margin-top:12px; font-weight:800;
        padding:10px 12px; border-radius:12px;
        border:1px solid rgba(197,22,22,0.22);
        background: rgba(197,22,22,0.08);
        color:#b10000;
    }
    .msg.ok{
        border:1px solid rgba(0,150,0,0.20);
        background: rgba(0,150,0,0.08);
        color:#1b6e1b;
    }
</style>

<div class="page">
    <div class="card">
        <div class="title">
            <h1>Statistici</h1>
            <p>Interogări simple (JOIN) + interogări complexe (subcereri) cu parametri variabili.</p>
        </div>

        <div class="filters">
            <div class="field">
                <label>Medic (opțional)</label>
                <asp:DropDownList ID="ddlMedic" runat="server" CssClass="input" />
            </div>

            <div class="field">
                <label>Data început (pentru filtre)</label>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="input" TextMode="Date" />
            </div>

            <div class="field">
                <label>Data sfârșit (pentru filtre)</label>
                <asp:TextBox ID="txtTo" runat="server" CssClass="input" TextMode="Date" />
            </div>

            <div class="field">
                <label>Prag (ex: min consultații / pacient)</label>
                <asp:TextBox ID="txtMin" runat="server" CssClass="input" Text="2" />
            </div>

            <div class="field">
                <asp:Button ID="btnRun" runat="server" Text="Rulează statistici"
                    CssClass="btn" OnClick="btnRun_Click" />
            </div>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="msg" />

      
        <div class="section">
            <h2>1) Interogări SIMPLE (JOIN) – minim 6</h2>
            <p class="hint">Afișate ca tabele (nu „pagină de interogări” brută), integrate în aplicație.</p>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">S1. Programări (Pacient + Medic) în interval</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvS1" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">S2. Lista pacienți + medic de familie (JOIN)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvS2" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">S3. Consultații (JOIN prin Programări → Pacienți + Medici)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvS3" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">S4. Rețete + pacient + medic (JOIN)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvS4" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">S5. Tratamente pe rețete (JOIN Retete_Tratamente + Tratamente)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvS5" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">S6. Nr. consultații / medic (JOIN + GROUP BY)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvS6" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>
        </div>

        
        <div class="section">
            <h2>2) Interogări COMPLEXE (SUBCERERI) – minim 4</h2>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">C1. Medici cu nr. consultații peste media tuturor medicilor (subquery AVG)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvC1" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">C2. Pacienți fără nicio consultație (NOT EXISTS)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvC2" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">C3. Medicamentul cel mai prescris (subquery MAX)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvC3" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>

            <h3 style="margin:14px 0 8px; font-weight:900; color:#3d2aa0;">C4. Pacienți cu ≥ Prag consultații în interval (subquery + parametru)</h3>
            <div class="grid-wrap">
                <asp:GridView ID="gvC4" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
            </div>
        </div>

    </div>
</div>

</asp:Content>
