<%@ Page Language="C#" AutoEventWireup="true"  Inherits="ParkingManagerServer._default" %>
<%Server.Execute("Seguranca.aspx");%>
<html>
<head>
    <link rel="stylesheet" type="text/css" href="style.css">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <style>
    /* Always set the map height explicitly to define the size of the div
     * element that contains the map. */
    #map {
      width:75%;
      min-width:380px;
      height: 70%;
    float:left;
    }

    </style>

</head>
<body onkeydown="keyEvent(event)" onkeyup="metaKeyUp(event)">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="/Scripts/parking.edit.tool.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>

    <div class="fundoCorpo">
        <div class="titulo">
            <span id="spTitulo" class="titulo">Carregando Estacionamento...</span>
        </div>

        <div class="fundoCabecalho">


            <div class="blocoCabecalho PreenchidoTotalmente">
                <div id="map"></div>
                <div style="width:20%; min-width:200px; float:left; margin-left:5px">
                    
                    <input id="Button1" class="button button5" type="button" onclick="Button1ClickEvent()" value="Demarcar Caminho" />
                    <input id="Button3" class="button button5" type="button" onclick="Button3ClickEvent()" value="Ligar Pontos" />
                    <input id="Button2" class="button button5" type="button" onclick="Button2ClickEvent()" value="Demarcar Vagas" />
                    <input class="button button5" id="file" type="file" />
                    <input type="button" class="button button5" id="upload" value="Importar Planta" />


                </div>
               


            </div>


        </div>


    </div>
    <script>
        var map = null;
        var historicalOverlay;
        var estacionamentoSelecionado = JSON.parse($.cookie('estacionamento'));
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
            var id = gup("Id");
            if (id!=null) {
                    obterEstacionamento(id);
            }else{
                obterEstacionamento(estacionamentoSelecionado.Id);
        }
        }
        function gup(name, url) {
           if (!url) url = location.href;
           name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
           var regexS = "[\\?&]" + name + "=([^&#]*)";
           var regex = new RegExp(regexS);
           var results = regex.exec(url);
           return results == null ? null : results[1];
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
    <script>
        function obterEstacionamento(id) {
            $.ajax({
                contentType: "application/json",
                type: "GET",
                url: "api/EstacionamentoModels/"+id,
                success: function (data, status) {
                    if (status == 'success') {
                        estacionamentoSelecionado = data;
                        $.cookie("estacionamento", JSON.stringify(data));
                        carregarEstacionamento(estacionamentoSelecionado);
                        document.title = estacionamentoSelecionado.Nome;
                        document.getElementById('spTitulo').innerHTML ="Editando '"+ estacionamentoSelecionado.Nome+"'";
                    }
                },
                error: function (request, status, error) {
                    
                }
            });
        }

    </script>

</body>


</html>
