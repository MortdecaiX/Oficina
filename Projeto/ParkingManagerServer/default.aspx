<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ParkingManagerServer._default" %>



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
      <script>
          var idEstacionamento =2;
          var markers = [];
          var markerAddedEventListener = null;
          var dadosImagemEstacionamento = null;
          var marker1 = null;
          var marker2 = null;


          // Adds a marker to the map and push to the array.  
          function addMarker(location, id) {
              var marker = new google.maps.Marker({
                  position: location,
                  map: map,
                  title: id
              });
              markers.push(marker);

              google.maps.event.addListener(marker, 'click',  MarkerClicked);
              return marker;
          }

          var dadosUltimoPontoColocado = null;

          function MarkerClicked() {
              var marker = this;
              //alert("Tite for this marker is:" + this.title);
              var evt = new CustomEvent('MarkerClickedEvent', { detail: marker });
              window.dispatchEvent(evt);
          }

          var originalButtonValue = null;
          function Button1ClickEvent() {
              if (!document.getElementById("Button1").disabled) {
                  if (markerAddedEventListener == null) {
                      markerAddedEventListener = google.maps.event.addListener(map, 'click', function MapClickEvent(event) {

                          var tempUltimoPontoColocado = dadosUltimoPontoColocado;
                          

                          dadosUltimoPontoColocado = new Object();
                          dadosUltimoPontoColocado.Id = 0;
                          dadosUltimoPontoColocado.Localizacao = new Object();
                          dadosUltimoPontoColocado.Localizacao.Latitude = event.latLng.lat();
                          dadosUltimoPontoColocado.Localizacao.Longitude = event.latLng.lng();
                          dadosUltimoPontoColocado.Localizacao.Altitude = 0;
                          dadosUltimoPontoColocado.Conexoes = new Array();
                          if (tempUltimoPontoColocado != null) {
                              dadosUltimoPontoColocado.Conexoes.push(tempUltimoPontoColocado.Id);
                          } 
                          dadosUltimoPontoColocado.Entrada = false;
                          dadosUltimoPontoColocado.Saida = false;

                          $.ajax({
                              contentType: "application/json",
                              data: JSON.stringify(dadosUltimoPontoColocado),
                              type: "POST",
                              url: "api/EstacionamentoModel/" + idEstacionamento + "/AdicionarPonto",
                              success: function (data, status) {
                                  if (status == 'success') {
                                      dadosUltimoPontoColocado.Id = data.Id;

                                      dadosUltimoPontoColocado.Conexoes.forEach(function (item, index) {
                                         
                                          $.ajax({
                                              contentType: "application/json",
                                              type: "GET",
                                              url: "api/PontoModels/ConectarPontos/" + item + "/" + dadosUltimoPontoColocado.Id,
                                              success: function (data, status) {
                                                  if (status == 'success') {
                                                      //mostrar polylines ligando os dois pontos
                                                      if (tempUltimoPontoColocado != null) {
                                                          var flightPlanCoordinates = [
                                                               { lat: tempUltimoPontoColocado.Localizacao.Latitude, lng: tempUltimoPontoColocado.Localizacao.Longitude },
                                                               { lat: dadosUltimoPontoColocado.Localizacao.Latitude, lng: dadosUltimoPontoColocado.Localizacao.Longitude }
                                                          ];
                                                          var flightPath = new google.maps.Polyline({
                                                              path: flightPlanCoordinates,
                                                              geodesic: true,
                                                              strokeColor: '#FF0000',
                                                              strokeOpacity: 1.0,
                                                              strokeWeight: 2
                                                          });

                                                          flightPath.setMap(map);
                                                      }
                                                  }
                                              }
                                          });


                                      })
                                      addMarker(event.latLng, dadosUltimoPontoColocado.Id.toString());
                                  }
                              }
                          });
                          
                      });
                      DisableAllButtonsBut('Button1');
                      originalButtonValue = document.getElementById("Button1").value;
                      document.getElementById("Button1").value = "Parar";
                  }
                  else {
                      markerAddedEventListener.remove();
                      markerAddedEventListener = null;
                      EnableAllButtons();
                      document.getElementById("Button1").value = originalButtonValue;
                  }
              }
          }

          

          function DisableAllButtonsBut(butButtonId) {
              document.getElementById("Button1").disabled = true;
              document.getElementById("Button2").disabled = true;
              document.getElementById("upload").disabled = true;
              document.getElementById("Button4").disabled = true;
              document.getElementById("Button5").disabled = true;
              document.getElementById("Button6").disabled = true;
              document.getElementById("file").disabled = true;
              document.getElementById(butButtonId).disabled = false;
              
          }
          function EnableAllButtons() {
              document.getElementById("Button1").disabled = false;
              document.getElementById("Button2").disabled = false;
              document.getElementById("upload").disabled = false;
              document.getElementById("Button4").disabled = false;
              document.getElementById("Button5").disabled = false;
              document.getElementById("Button6").disabled = false;
              document.getElementById("file").disabled = false;
              
              

          }
          var demarcandoVaga = false;
          function Button2ClickEvent() {
              if (!demarcandoVaga) {
                  window.addEventListener('MarkerClickedEvent', MarkedClickedOnExpectVacanceOrigin);
                  demarcandoVaga = true;
                  DisableAllButtonsBut('Button2');
                  originalButtonValue = document.getElementById("Button2").value;
                  document.getElementById("Button2").value = 'Parar';
              } else {
                  demarcandoVaga = false;
                  EnableAllButtons();
                  if (markerAddedEventListener != null) {
                      markerAddedEventListener.remove();
                      markerAddedEventListener = null;
                      
                  }
                  document.getElementById("Button2").value = originalButtonValue;
              }
          }

          function MarkedClickedOnExpectVacanceOrigin (e) {
              alert('Marker Selecionado:' + e.detail.title + ". Agora clique onde ficara as vagas a partir deste marker");
              window.removeEventListener('MarkerClickedEvent', MarkedClickedOnExpectVacanceOrigin);

              markerAddedEventListener = google.maps.event.addListener(map, 'click', function MapClickEvent(event) {

                  addMarker(event.latLng);
                  alert('Vaga colocada a partir de:' + e.detail.title);
                  
              });
          }

          function Button3ClickEvent() {
              document.getElementById("demo").innerHTML = "Hello World";
          }
          function Button4ClickEvent() {
              document.getElementById("demo").innerHTML = "Hello World";
          }
          function Button5ClickEvent() {
              document.getElementById("demo").innerHTML = "Hello World";
          }
          function Button6ClickEvent() {
              document.getElementById("demo").innerHTML = "Hello World";
          }

        

          function keyEvent(event) {
              if (event.keyCode = 20) {
                  historicalOverlay.setMap(null);
              }
          }

          function metaKeyUp(event) {
              if (event.keyCode = 20) {
                  historicalOverlay.setMap(map);
              }
          }

</script>
  </head>
  <body onkeydown="keyEvent(event)" onkeyup="metaKeyUp(event)">
      
      <div>
          <asp:Image ID="Image1" runat="server" />
          <input id="Button1" type="button" onclick="Button1ClickEvent()" value="Demarcar Caminho" />
          <input id="Button2" type="button" onclick="Button2ClickEvent()" value="Demarcar Vagas" />
          
          <input id="file" type="file" />
          <input type="button" id="upload" value="Importar Planta" />
             
          <input id="Button4" type="button" value="Girar à Esquerda" />
          <input id="Button5" type="button" value="Girar à Direita" />
          <input id="Button6" type="button" value="Inverter" />
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
    <script>
       

        $(document).ready(function () {
            $('#upload').on('click', function () {
                // Define a boundary, I stole this from IE but you can use any string AFAIK
                

                

                var file = document.getElementById("file").files[0];
                
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    body = (reader.result);
                    var obj = new Object();
                    obj.Data = body;
                    obj.Filename = file.name;


                   


                    var mapBounds =  map.getBounds();
                    obj.swBoundLat = mapBounds.getSouthWest().lat();
                    obj.swBoundLng = mapBounds.getSouthWest().lng();
                    obj.neBoundLat = mapBounds.getNorthEast().lat();
                    obj.neBoundLng = mapBounds.getNorthEast().lng();

                    $.ajax({
                        contentType: "application/json",
                        data: JSON.stringify(obj),
                        type: "POST",
                        url: "http://parkingmanagerserver.azurewebsites.net/api/EstacionamentoModels/" + idEstacionamento + "/Imagem",
                        success: function (data, status) {
                            if (status == 'success') {

                                dadosImagemEstacionamento = JSON.parse(data);

                                var returnedDada = dadosImagemEstacionamento.url;
                                if (historicalOverlay != null) {
                                    historicalOverlay.setMap(null);
                                    historicalOverlay = null;
                                }
                                var swLat = dadosImagemEstacionamento.swBoundLat;
                                var swLng = dadosImagemEstacionamento.swBoundLng;

                                var neLat = dadosImagemEstacionamento.neBoundLat;
                                var neLng = dadosImagemEstacionamento.neBoundLng;

                                var swBound = new google.maps.LatLng(swLat, swLng);
                                var neBound = new google.maps.LatLng(neLat, neLng);
                                var bounds = new google.maps.LatLngBounds(swBound, neBound);
                                
                                if (marker1 != null) {
                                    marker1.setMap(null);
                                }
                                marker1 = new google.maps.Marker({
                                    position: swBound,
                                    map: map,
                                    title: "swBound",
                                    draggable: true
                                });
                                google.maps.event.addListener(marker1, 'dragend', swBoundMarkerLocationChanged);
                                if (marker2 != null) {
                                    marker2.setMap(null);
                                }
                                marker2 = new google.maps.Marker({
                                    position: neBound,
                                    map: map,
                                    title: "neBound",
                                    draggable: true
                                });
                                google.maps.event.addListener(marker2, 'dragend', neBoundMarkerLocationChanged);
                                historicalOverlay = new google.maps.GroundOverlay(
                                            returnedDada,
                                            bounds);
                                historicalOverlay.setMap(map);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            console.log(errorThrown + "\n" + textStatus + "\n\n" + XMLHttpRequest);
                        }
                    });

                };
                reader.onerror = function (error) {
                    console.log('Error: ', error);
                };


                
            });
        });

        function neBoundMarkerLocationChanged(event) {
            var bounds = historicalOverlay.getBounds();
            var swBound = bounds.getSouthWest();
            

         
            var neBound = bounds.getNorthEast();
            neBound = event.latLng;
            if (dadosImagemEstacionamento != null) {
                dadosImagemEstacionamento.swBoundLat = swBound.lat();
                dadosImagemEstacionamento.swBoundLng = swBound.lng();
                dadosImagemEstacionamento.neBoundLat = neBound.lat();
                dadosImagemEstacionamento.neBoundLng = neBound.lat();
            }
            AtualizarDadosImagemEstacionamento();
            bounds = new google.maps.LatLngBounds(swBound, neBound);
            historicalOverlay.setMap(null);
            historicalOverlay = new google.maps.GroundOverlay(
                                            historicalOverlay.getUrl(),
                                            bounds);
            historicalOverlay.setMap(map);

            

        }
        function swBoundMarkerLocationChanged(event) {
            var bounds = historicalOverlay.getBounds();
            var swBound = bounds.getSouthWest();
            swBound = event.latLng;

           
            var neBound = bounds.getNorthEast();
            

            if (dadosImagemEstacionamento != null) {
                dadosImagemEstacionamento.swBoundLat = swBound.lat();
                dadosImagemEstacionamento.swBoundLng = swBound.lng();
                dadosImagemEstacionamento.neBoundLat = neBound.lat();
                dadosImagemEstacionamento.neBoundLng = neBound.lat();
            }
            AtualizarDadosImagemEstacionamento();
            bounds = new google.maps.LatLngBounds(swBound, neBound);
            historicalOverlay.setMap(null);
            historicalOverlay = new google.maps.GroundOverlay(
                                            historicalOverlay.getUrl(),
                                            bounds);
            historicalOverlay.setMap(map);
            
        }


        function AtualizarDadosImagemEstacionamento() {
            if (dadosImagemEstacionamento != null) {
                $.ajax({
                    contentType: "application/json",
                    data: JSON.stringify(dadosImagemEstacionamento),
                    type: "POST",
                    url: "api/EstacionamentoModels/" + idEstacionamento + "/Imagem",
                    success: function (data, status) {
                        if (status == 'success') {
                            dadosImagemEstacionamento = JSON.parse(data);
                        }
                    }
                });
            }
        }
    </script>
</html>