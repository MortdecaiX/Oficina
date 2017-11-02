
          var dadosEstacionamento = new Object();
          dadosEstacionamento.Id = 2;
          var markers = [];
          var markerAddedEventListener = null;
          var dadosImagemEstacionamento = null;
          var marker1 = null;
          var marker2 = null;
          var autoLigarPontos = false;
          

          

          function carregarEstacionamento(estacionamento) {

                  dadosEstacionamento = estacionamento;
                  var latitude = estacionamento.Localizacao.Latitude;
                  var longitude = estacionamento.Localizacao.Longitude;
                  var altitude = estacionamento.Localizacao.Altitude;



                  var latlng = { lat: latitude, lng: longitude };


                  addMarker(latlng, estacionamento.Nome, "estacionamento", false);

              //Carregar Overlay
                  dadosImagemEstacionamento = new Object();
                  dadosImagemEstacionamento.url = estacionamento.ImagemURL;
                 
                  dadosImagemEstacionamento.swBoundLat = estacionamento.SWBoundImagem.Latitude;
                  dadosImagemEstacionamento.swBoundLng=estacionamento.SWBoundImagem.Longitude;

                  dadosImagemEstacionamento.neBoundLat = estacionamento.NEBoundImagem.Latitude;
                  dadosImagemEstacionamento.neBoundLng = estacionamento.NEBoundImagem.Longitude;
                  
                  mostrarGroundOverlay(dadosImagemEstacionamento);

                  mostrarPontosDeCaminho(estacionamento.Pontos, true);
                  
                   
          }

          function mostrarPontosDeCaminho(pontos, visivel){
              pontos.forEach(function (ponto, index) {
              
                  

                  var latitude = ponto.Localizacao.Latitude;
                  var longitude = ponto.Localizacao.Longitude;
                  var altitude =  ponto.Localizacao.Altitude;
                  var latlng = {lat: latitude,lng:longitude};
                  
                  var marker = addMarker(latlng, ponto.Id.toString(),"ponto",false);
                  marker.setVisible(visivel);
                  
                      
                  ponto.Conexoes.forEach(function(conexao, index) {
                      pontos.forEach(function (_ponto, index) {
                          
                          if (_ponto.Id == conexao)
                          {
                              var _latitude = _ponto.Localizacao.Latitude;
                              var _longitude =  _ponto.Localizacao.Longitude;
                              var _altitude =  _ponto.Localizacao.Altitude;

                              var flightPlanCoordinates = [
                                                   latlng,
                                                   { lat: _latitude, lng: _longitude }
                              ];
                              var flightPath = new google.maps.Polyline({
                                  path: flightPlanCoordinates,
                                  geodesic: true,
                                  strokeColor: '#357BEA',
                                  strokeOpacity: 1.0,
                                  strokeWeight: 2
                              });

                              flightPath.setMap(map);

                          }

                      });
                      });

                  
                  mostrarVagasConectadasNoPonto(ponto);



                  
              
              }
              
              );
              

              
          }

          function mostrarVagasConectadasNoPonto(ponto) {

              ponto.VagasConectadas.forEach(function (vaga, index) {
              
                  var latitude = vaga.Localizacao.Latitude;
                  var longitude =  vaga.Localizacao.Longitude;
                  var altitude =  vaga.Localizacao.Altitude;
                  var latlng = {lat:latitude,lng: longitude};
                  var tipoVaga = null;
                  switch (vaga.Tipo) {
                      default:
                          tipoVaga = "vaga"
                          break;
                      case 1:
                          tipoVaga = "vagaEspecial1"
                          break;
                      case 2:
                          tipoVaga = "vagaEspecial2"
                          break;
                  }

                  var marker = addMarker(latlng, "Vaga: " + vaga.Id, tipoVaga, false);

                  var _latitude = ponto.Localizacao.Latitude;
                  var _longitude = ponto.Localizacao.Longitude;
                  var _altitude = ponto.Localizacao.Altitude;


                  var flightPlanCoordinates = [
                                                   latlng,
                                                   { lat: _latitude, lng: _longitude }
                  ];
                  var flightPath = new google.maps.Polyline({
                      path: flightPlanCoordinates,
                      geodesic: true,
                      strokeColor: '#42D7F4',
                      strokeOpacity: 1.0,
                      strokeWeight: 2
                  });

                  flightPath.setMap(map);

              
              });

             
                  

              
          }

          // Adds a marker to the map and push to the array.  
          function addMarker(location, id, tipo, arrastavel) {

              
              var icons = {
                  estacionamento: {
                      icon: 'Assets/Images/parking.png'
                  },
                  novoPonto: {
                      icon: 'Assets/Images/placeholder-point_new.png'
                  },
                  ponto: {
                      icon: 'Assets/Images/placeholder-point.png'
                  },
                  novaVaga: {
                      icon: 'Assets/Images/parking-sign_new.png'
                  },
                  vaga: {
                      icon: 'Assets/Images/parking-sign.png'
                  },
                  vagaEspecial1: {
                      icon: 'Assets/Images/parking-sign_special_1.png'
                  },
                  vagaEspecial2: {
                      icon: 'Assets/Images/parking-sign_special_2.png'
                  },
                  neBound: {
                      icon: 'Assets/Images/move-pointer.png'
                  },
                  swBound:{
                      icon: 'Assets/Images/move-pointer.png'
                  }
              };

              if (typeof arrastavel == 'undefined') {
                  arrastavel = false;
              }
              if (typeof tipo == 'undefined') {
                  tipo = 'ponto';
              }
              if (typeof icons[tipo] == 'undefined') {
                  alert('O tipo de marcador ' + tipo + ' é desconhecido');
                  tipo = 'ponto';
              }
              

              var maker = null;
              
                   marker = new google.maps.Marker({
                      position: location,
                      map: map,
                      title: id,
                      icon: icons[tipo].icon,
                      draggable: arrastavel
                  });
              

              google.maps.event.addListener(marker, 'click', MarkerClicked);

              markers.push(marker);
              return marker;
          }

          var dadosUltimoPontoColocado = null;
          var runOnMarkerClicked = null;
          function MarkerClicked() {
              var marker = this;
              //alert("Tite for this marker is:" + this.title);
              var evt = new CustomEvent('MarkerClickedEvent', { detail: marker });
              window.dispatchEvent(evt);
              if (runOnMarkerClicked != null) {
                  runOnMarkerClicked(evt);
              }
          }

          var originalButtonValue = null;
          function Button1ClickEvent() {
              if (!document.getElementById("Button1").disabled) {
                  if (markerAddedEventListener == null) {
                      markerAddedEventListener = google.maps.event.addListener(map, 'click', function MapClickEvent(event) {
                          registrarPonto(event.latLng);
                          runOnMarkerClicked = function (evt) {
                              
                              if (dadosUltimoPontoColocado != null) {
                                  $.ajax({
                                  contentType: "application/json",
                                  type: "GET",
                                  url: "api/PontoModels/ConectarPontos/" + evt.detail.title + "/" + dadosUltimoPontoColocado.Id,
                                  success: function (data, status) {
                                      if (status == 'success') {
                                          //mostrar polylines ligando os dois pontos
                                         
                                              var flightPlanCoordinates = [
                                                   evt.detail.position,
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
                                              runOnMarkerClicked = null;
                                      }
                                  }
                              });
                          }
                          }
                      });
                      DisableAllButtonsBut('Button1');
                      originalButtonValue = document.getElementById("Button1").value;
                      document.getElementById("Button1").value = "Parar";
                  }
                  else {
                      markerAddedEventListener.remove();
                      markerAddedEventListener = null;
                      EnableAllButtons();
                      dadosUltimoPontoColocado = null;
                      document.getElementById("Button1").value = originalButtonValue;
                  }
              }
          }

          
          function registrarPonto(latLng) {
              var tempUltimoPontoColocado = dadosUltimoPontoColocado;


              dadosUltimoPontoColocado = new Object();
              dadosUltimoPontoColocado.Id = 0;
              dadosUltimoPontoColocado.Localizacao = new Object();
              dadosUltimoPontoColocado.Localizacao.Latitude = latLng.lat();
              dadosUltimoPontoColocado.Localizacao.Longitude = latLng.lng();
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
                  url: "api/EstacionamentoModel/" + dadosEstacionamento.Id + "/AdicionarPonto",
                  success: function (data, status) {
                      if (status == 'success') {
                          dadosUltimoPontoColocado.Id = data.Id;
                          addMarker(latLng, dadosUltimoPontoColocado.Id.toString(), "novoPonto", false);
                          if (autoLigarPontos) {
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
                                                  addMarker(latLng, dadosUltimoPontoColocado.Id.toString(), "novoPonto", false);
                                              }
                                          }
                                      }
                                  });



                              })
                          }
                          
                      }
                  }
              });

          }
          function DisableAllButtonsBut(butButtonId) {
              document.getElementById("Button1").disabled = true;
              document.getElementById("Button2").disabled = true;
              document.getElementById("Button3").disabled = true;
              document.getElementById("upload").disabled = true;

              document.getElementById("file").disabled = true;
              document.getElementById(butButtonId).disabled = false;
              
          }
          function EnableAllButtons() {
              document.getElementById("Button1").disabled = false;
              document.getElementById("Button2").disabled = false;
              document.getElementById("Button3").disabled = false;
              document.getElementById("upload").disabled = false;
          
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
                  window.removeEventListener('MarkerClickedEvent', MarkedClickedOnExpectVacanceOrigin);
                  document.getElementById("Button2").value = originalButtonValue;
              }
          }

          function MarkedClickedOnExpectVacanceOrigin (e) {
             
              if (markerAddedEventListener != null) {
                  markerAddedEventListener.remove();
                  markerAddedEventListener = null;

              }

              markerAddedEventListener = google.maps.event.addListener(map, 'click', function MapClickEvent(event) {

                  var vaga = new Object();
                  vaga.Id = 0;
                  vaga.Numero = 0;
                  vaga.Tipo = 0;
                  vaga.Ocupacao = null;
                  vaga.Localizacao = new Object();
                  vaga.Localizacao.Latitude = event.latLng.lat();
                  vaga.Localizacao.Longitude = event.latLng.lng();
                  vaga.Localizacao.Altitude = 0;
                  vaga.Pavimento = 0;
                  vaga.Reserva = null;

                  $.ajax({
                      contentType: "application/json",
                      data: JSON.stringify(vaga),
                      type: "POST",
                      url: "api/VagaModels?idPonto=" + e.detail.title,
                      success: function (data, status) {
                          if (status == 'success') {
                              
                              addMarker(event.latLng, "Vaga: " + data.Id,"novaVaga",false);
                              var _latitude = vaga.Localizacao.Latitude;
                              var _longitude = vaga.Localizacao.Longitude;
                              var _altitude = vaga.Localizacao.Altitude;


                              var flightPlanCoordinates = [
                                                                e.detail.position,
                                                               { lat: _latitude, lng: _longitude }
                              ];
                              var flightPath = new google.maps.Polyline({
                                  path: flightPlanCoordinates,
                                  geodesic: true,
                                  strokeColor: '#42D7F4',
                                  strokeOpacity: 1.0,
                                  strokeWeight: 2
                              });

                              flightPath.setMap(map);
                              
                          }
                      }
                  });

                  
                  //alert('Vaga colocada a partir de:' + e.detail.title);
                  
              });
          }
          var PontoOrigem = null;
          var PontoDestino = null;
          var ligandoPontos = false;
          var tituloOriginalButton3 = null;
          function Button3ClickEvent() {
              if (!ligandoPontos) {
                  ligandoPontos = true;
                  DisableAllButtonsBut("Button3");
                  tituloOriginalButton3 = document.getElementById("Button3").value;
                  document.getElementById("Button3").value = "Parar";
                  var acaoPrimaria = function (evt) {
                      PontoOrigem = evt.detail;
                      runOnMarkerClicked = function (evt) {
                          PontoDestino = evt.detail;
                          

                          $.ajax({
                              contentType: "application/json",
                              type: "GET",
                              url: "api/PontoModels/ConectarPontos/" + PontoOrigem.title + "/" + PontoDestino.title,
                              success: function (data, status) {
                                  if (status == 'success') {
                                      //mostrar polylines ligando os dois pontos
                                      
                                          var flightPlanCoordinates = [
                                               PontoOrigem.position,
                                               PontoDestino.position
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
                          });


                          idPontoOrigem = null;
                          idPontoDestino = null;
                          runOnMarkerClicked = acaoPrimaria;
                      };
                  };
                  runOnMarkerClicked = acaoPrimaria
              } else {
                  EnableAllButtons();
                  document.getElementById("Button3").value = tituloOriginalButton3;
                  idPontoOrigem = null;
                  idPontoDestino = null;
                  runOnMarkerClicked = null;
                  ligandoPontos = false;
              }
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





          			var mapBounds = map.getBounds();
          			obj.swBoundLat = mapBounds.getSouthWest().lat();
          			obj.swBoundLng = mapBounds.getSouthWest().lng();
          			obj.neBoundLat = mapBounds.getNorthEast().lat();
          			obj.neBoundLng = mapBounds.getNorthEast().lng();

          			$.ajax({
          				contentType: "application/json",
          				data: JSON.stringify(obj),
          				type: "POST",
          				url: "api/EstacionamentoModels/" + dadosEstacionamento.Id + "/Imagem",
          				success: function (data, status) {
          					if (status == 'success') {

          						dadosImagemEstacionamento = JSON.parse(data);
          						mostrarGroundOverlay(dadosImagemEstacionamento);

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
          function mostrarGroundOverlay(dadosImagemEstacionamento) {

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
          	map.fitBounds(bounds);
          	if (marker1 != null) {
          		marker1.setMap(null);
          	}
          	//marker1 = new google.maps.Marker({
          	//	position: swBound,
          	//	map: map,
          	//	title: "swBound",
          	//	draggable: true
          	//});
          	marker1 = addMarker(swBound, "Clique e arraste para ajustar a imagem", "swBound", true);
          	google.maps.event.addListener(marker1, 'dragend', swBoundMarkerLocationChanged);
          	if (marker2 != null) {
          		marker2.setMap(null);
          	}
          	//marker2 = new google.maps.Marker({
          	//	position: neBound,
          	//	map: map,
          	//	title: "neBound",
          	//	draggable: true
          	//});
          	marker2 = addMarker(neBound, "Clique e arraste para ajustar a imagem", "neBound", true);
          	google.maps.event.addListener(marker2, 'dragend', neBoundMarkerLocationChanged);
          	historicalOverlay = new google.maps.GroundOverlay(
                        dadosImagemEstacionamento.url,
                        bounds);
          	historicalOverlay.setMap(map);


          }
          function neBoundMarkerLocationChanged(event) {
          	var bounds = historicalOverlay.getBounds();
          	var swBound = bounds.getSouthWest();

          	var neBound = event.latLng;

          	dadosImagemEstacionamento.neBoundLat = neBound.lat();
          	dadosImagemEstacionamento.neBoundLng = neBound.lng();

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
          	var swBound = event.latLng;

          	var neBound = bounds.getNorthEast();

          	dadosImagemEstacionamento.swBoundLat = swBound.lat();
          	dadosImagemEstacionamento.swBoundLng = swBound.lng();

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
          			url: "api/EstacionamentoModels/" + dadosEstacionamento.Id + "/Imagem",
          			success: function (data, status) {
          				if (status == 'success') {
          					dadosImagemEstacionamento = JSON.parse(data);
          				}
          			}
          		});
          	}
          }

          
