<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateToDo.aspx.cs" Inherits="TodoForms.CreateToDo" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-top:20px;">
        <div class="row">
            <div class="col-md-2">ToDo text</div>
            <div class="col-md-10"><input runat="server" type="text" ID="ToDoText" /></div>
        </div>
        <div class="row" style="padding-top:10px;">
            <asp:Button runat="server" OnClick="Unnamed_Click" Text="Back To ToDo List" CssClass="btn btn-default"/>
            <asp:Button runat="server" Text="Save" ID="SaveNewToDoButton" OnClick="SaveNewToDoButton_Click" CssClass="btn btn-success" />
        </div>
    </div>
</asp:Content>