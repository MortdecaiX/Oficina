﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ParkingManagerServer._default" %>



<!DOCTYPE html>
<html>
  <head>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no">
    <meta charset="utf-8">
    <title>Simple markers</title>
    <style>
      /* Always set the map height explicitly to define the size of the div
       * element that contains the map. */
      #map {
        height: 100%;
      }
      /* Optional: Makes the sample page fill the window. */
      html, body {
        height: 100%;
        margin: 0;
        padding: 0;
      }
    </style>
      
  </head>
  <body onkeydown="keyEvent(event)" onkeyup="metaKeyUp(event)">
      
      <div>
          <asp:Image ID="Image1" runat="server" />
          <input id="Button1" type="button" onclick="Button1ClickEvent()" value="Demarcar Caminho" />
          <input id="Button3" type="button" onclick="Button3ClickEvent()" value="Ligar Pontos" />
          <input id="Button2" type="button" onclick="Button2ClickEvent()" value="Demarcar Vagas" />
          
          
          <input id="file" type="file" />
          <input type="button" id="upload" value="Importar Planta" />
             

      </div>
      
    <div id="map"></div>
    <script>
        var map = null;
        var historicalOverlay;

        function initMap() {
           
            

            if ((window.location.protocol == "https") && navigator.geolocation) {
               
                    navigator.geolocation.getCurrentPosition(function (position) {

                        var myLatLng = { lat: position.coords.latitud, lng: position.coords.longitude };

                        map = new google.maps.Map(document.getElementById('map'), {
                            zoom: 4,
                            center: myLatLng
                        });




                    });
                
          } else {
              showOnDefaultLocation();
          }
        }

        function showOnDefaultLocation() {

            var myLatLng = { lat: -15.741186, lng: -47.954068 };

            map = new google.maps.Map(document.getElementById('map'), {
                zoom: 5,
                center: myLatLng
            });
        }

       

        

    </script>
    <script async defer
    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB_VmR84B8dxqCxnDEm5g-zLKcWX2cCOvg&callback=initMap">
    </script>
          
  </body>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="Scripts/parking.edit.tool.js"></script>
    <script>
        function exemplo() {
            $.ajax({
                contentType: "application/json",
                type: "GET",
                url: "api/EstacionamentoModels/2",
                success: function (data, status) {
                    if (status == 'success') {
                        carregarEstacionamento(data);
                    }
                }
            });
        }
        exemplo();
    </script>
</html>