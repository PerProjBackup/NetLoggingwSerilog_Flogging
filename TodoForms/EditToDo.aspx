<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditToDo.aspx.cs" Inherits="TodoForms.EditToDo" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-top:20px;">
        <div class="row">
            <div class="col-md-1">ToDo text</div>
            <div class="col-md-10"><input runat="server" type="text" ID="ToDoText" /></div>
        </div>
        <div class="row">
            <asp:HiddenField ID="ToDoId" runat="server"/>
            <div class="col-md-1">Completed</div>
            <div class="col-md-10"><asp:CheckBox runat="server" type="checkbox" ID="Completed" /></div>
        </div>
        <div class="row" style="padding-top:20px;">
            <asp:Button runat="server" OnClick="Unnamed_Click" Text="Back To ToDo List" CssClass="btn btn-default"/>
            <asp:Button runat="server" Text="Save" ID="SaveNewToDoButton" OnClick="UpdateToDoButton_Click" CssClass="btn btn-success" />
        </div>
    </div>
</asp:Content>