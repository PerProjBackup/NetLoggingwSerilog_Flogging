<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToDos.aspx.cs" Inherits="TodoForms.ToDos" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-top:30px;">

        <div style="padding:20px;">
            <a href="/CreateToDo.aspx">Create New ToDo</a>
        </div>

        <asp:GridView ID="gvToDos" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField = "Item" HeaderText = "To Do" />
                <asp:BoundField DataField = "Completed" HeaderText = "Completed" />
                <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="EditToDo.aspx?Id={0}" />             
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

