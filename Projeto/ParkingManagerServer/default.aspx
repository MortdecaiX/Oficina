<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ParkingManagerServer._default" %>
<% Server.Execute("Seguranca.aspx"); %>

<html>
<head>
    <link rel="stylesheet" type="text/css" href="style.css">
    <style>
    /* Always set the map height explicitly to define the size of the div
     * element that contains the map. */
    #map {
      width:auto;
      min-width:380px;
      height: 70%;
    
    }

    </style>
    <title>Meus Estacionamentos</title>
</head>
<body onkeydown="keyEvent(event)" onkeyup="metaKeyUp(event)">

    
    <div class="fundoCorpo">
        <div class="titulo">
            <span class="titulo">Meus Estacionamentos</span>
        </div>

        <div class="fundoCabecalho">


            <div class="blocoCabecalho PreenchidoTotalmente">
                <div style="min-width:200px; width: 100%;">   
                        
                     <input id="Button1" style="width:23%;  float:right; margin-left:1%;" class="button button5" type="button" onclick="btNovoEstClickEvent()" value="Novo" />
                    <input id="Button2" style="width:23%; clear:right; float:right; margin-left:1%;" class="button button5" type="button" onclick="sair()" value="Sair" />

                   
                    </div>
                
               
               <div id="map"></div>
                


            </div>


        </div>


    </div>
    <script>

        function sair() {
            document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });
            location.reload();
        }
        var map = null;
        var historicalOverlay;
        var clickedMarker = null;
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
            google.maps.event.addListener(map, 'click', function (event) {

                var marker = new google.maps.Marker({ position: event.latLng, map: map });
                if (clickedMarker != null) {
                    clickedMarker.setMap(null);
                }
                clickedMarker = marker;
                marker.addListener('click', function () {
                    clickedMarker = marker;
                });
            });
            carregarEstacionamentosUsuario();
           
        }

        function showOnDefaultLocation() {

            var myLatLng = { lat: -15.741186, lng: -47.954068 };

            map = new google.maps.Map(document.getElementById('map'), {
                zoom: 5,
                center: myLatLng
            });
        }

        function btNovoEstClickEvent() {
            if (clickedMarker != null) {
                document.location = "/tela_cadastro_estacionamento.aspx?lat=" + clickedMarker.position.lat()+"&lng="+clickedMarker.position.lng();
            } else {
                alert('Clique antes no local onde ficará o novo estacionamento!');
            }
        }

        function carregarEstacionamentosUsuario() {
            var urlTarget = "api/EstacionamentoModels/Usuario/"+JSON.parse($.cookie("usuario")).Id;
            $.ajax({
                contentType: "application/json",
                type: "GET",
                url: urlTarget,
                success: function (dados, status) {
                    if (status == 'success') {
                        dados.forEach(function (element, index, array) {
                            var marker = new google.maps.Marker({ title: element.Nome, position: { lat: element.Localizacao.Latitude, lng: element.Localizacao.Longitude }, map: map });

                            marker.addListener('click', function () {
                                clickedMarker = marker;
                                
                                document.location = "/tela_edicao_estacionamento.aspx?Id="+element.Id;
                            });
                        });
                        
                    }
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                }
            });


        }


    </script>

    <script async defer
            src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB_VmR84B8dxqCxnDEm5g-zLKcWX2cCOvg&callback=initMap">
    </script>

</body>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
<script src="http://parkingmanagerserver.azurewebsites.net/Scripts/parking.edit.tool.js"></script>


</html>
