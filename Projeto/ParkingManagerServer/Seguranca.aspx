<%@ Page Language="C#" AutoEventWireup="true"  Inherits="ParkingManagerServer._default" %>
<%

    if (Request.Cookies["usuario"] == null)
    {
        Server.Transfer("tela_login.aspx", true);
    }

     %>