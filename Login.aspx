<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Cabinet_Medical_Maria_Salau.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
   
    .login-page {
        min-height: calc(100vh - 140px); 
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 40px 16px;
        background: radial-gradient(1200px 600px at 15% 10%, rgba(120, 86, 255, 0.22), transparent 60%),
                    radial-gradient(900px 500px at 85% 20%, rgba(60, 120, 255, 0.20), transparent 55%),
                    linear-gradient(135deg, #f3efff, #ece6ff 45%, #efeaff);
    }

    
    .login-card {
        width: 100%;
        max-width: 440px;
        background: #ffffff;
        border-radius: 18px;
        box-shadow: 0 18px 60px rgba(20, 10, 60, 0.14);
        overflow: hidden;
        border: 1px solid rgba(106, 75, 204, 0.18);
    }

    .login-header {
        padding: 22px 26px 16px 26px;
        background: linear-gradient(135deg, #5b35b9, #6a4bcc 55%, #2f6bff);
        color: #fff;
    }

    .login-header h1 {
        margin: 0;
        font-size: 22px;
        font-weight: 800;
        letter-spacing: 0.2px;
    }

    .login-header p {
        margin: 6px 0 0 0;
        opacity: 0.9;
        font-size: 13.5px;
    }

    .login-body {
        padding: 24px 26px 22px 26px;
    }

    .field {
        margin-bottom: 14px;
        text-align: left;
    }

    .field label {
        display: block;
        margin-bottom: 6px;
        font-size: 12.5px;
        font-weight: 700;
        color: #2d2356;
        opacity: 0.9;
    }

    .input-pro {
        width: 100%;
        padding: 12px 12px;
        border-radius: 12px;
        font-size: 14.5px;
        border: 1.8px solid rgba(106, 75, 204, 0.28);
        background: #fbfaff;
        transition: 0.18s ease;
    }

    .input-pro:focus {
        outline: none;
        background: #ffffff;
        border-color: rgba(47, 107, 255, 0.75);
        box-shadow: 0 0 0 4px rgba(47, 107, 255, 0.12);
    }

    .btn-pro {
        width: 100%;
        padding: 12px 14px;
        border: none;
        border-radius: 12px;
        color: #fff;
        font-size: 15px;
        font-weight: 800;
        cursor: pointer;
        background: linear-gradient(135deg, #6a4bcc, #2f6bff);
        box-shadow: 0 12px 26px rgba(47, 107, 255, 0.22);
        transition: transform 0.12s ease, filter 0.12s ease;
    }

    .btn-pro:hover {
        filter: brightness(1.03);
        transform: translateY(-1px);
    }

    .btn-pro:active {
        transform: translateY(0px);
    }

    .error-msg {
        display: block;
        margin-top: 12px;
        padding: 10px 12px;
        border-radius: 12px;
        background: rgba(197, 22, 22, 0.08);
        border: 1px solid rgba(197, 22, 22, 0.22);
        color: #b10000;
        font-weight: 700;
        text-align: center;
    }

    .error-msg:empty {
        display: none;
    }
</style>

<div class="login-page">
    <div class="login-card">

        <div class="login-header">
            <h1>Cabinet Medical MS</h1>
            <p>Autentificare medic pentru acces la aplicație</p>
        </div>

        <div class="login-body">

            <div class="field">
                <label for="<%= txtEmail.ClientID %>">Email</label>
                <asp:TextBox ID="txtEmail" runat="server"
                    CssClass="input-pro"
                    placeholder="ex: andrei.popescu@cabinet.ro"></asp:TextBox>
            </div>

            <div class="field">
                <label for="<%= txtCNP.ClientID %>">CNP</label>
                <asp:TextBox ID="txtCNP" runat="server"
                    TextMode="Password"
                    CssClass="input-pro"
                    placeholder="Introdu CNP"></asp:TextBox>
            </div>

            <asp:Button ID="btnLogin" runat="server"
                Text="Conectare"
                CssClass="btn-pro"
                OnClick="btnLogin_Click" />

            <asp:Label ID="lblMsg" runat="server" CssClass="error-msg"></asp:Label>

        </div>
    </div>
</div>

</asp:Content>
