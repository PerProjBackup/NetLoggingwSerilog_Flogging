<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechnicalError.aspx.cs" 
    Inherits="TodoForms.TechnicalError" MasterPageFile="~/Site.Master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Oops!!
    </h1>
    <div class="alert alert-danger">
        <strong>Please try again.</strong>  If you continue to receive this error and 
        require assistance, contact our support team!.
        <div style="font-size:12px;">ErrorId = <%= Session.SessionID %></div>
    </div>
</asp:Content>