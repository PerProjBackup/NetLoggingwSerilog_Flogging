<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BadAjaxPage.aspx.cs" Inherits="TodoForms.BadAjaxPage" 
    MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    

    <div class="row" style="padding:20px;">    
        <asp:UpdatePanel ID="UpdateArea1" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="Tb1" runat="server" Text="test"></asp:TextBox>
                <asp:Button runat="server" Text="Update" OnClick="UpdateClick"/>
                
                <div id="ajaxErrorMessage" class="alert alert-danger" style="display:none;"/>           

            </ContentTemplate>        
        </asp:UpdatePanel>
    </div>

<script type="text/javascript">
  var prm = Sys.WebForms.PageRequestManager.getInstance();
  if (prm != null) {
      prm.add_endRequest(function (sender, e) {
          if (sender._postBackSettings.panelsToUpdate != null) {
              if (e.get_error() != null) {                
                  var mesg = "<strong>Oh, no!</strong> Something bad just happened.  ";
                
                  var ex = e.get_error();
                  mesg += "(" + ex.message + ")";

                  mesg += "<br />ErrorId: <%= Session?.SessionID %>";
                  $("#ajaxErrorMessage").html(mesg);
                  $("#ajaxErrorMessage").show();
                  e.set_errorHandled(true);
              }
          }
      });
  };
</script>

</asp:Content>
