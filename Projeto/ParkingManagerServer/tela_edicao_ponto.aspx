<%@ Page Language="C#" AutoEventWireup="true"  Inherits="ParkingManagerServer._default" %>
<%Server.Execute("Seguranca.aspx");
    
    
    %>
<html>
      <head>
	<link rel="stylesheet" type="text/css" href="style.css">
     <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    </head>  
    <body>
	<div class="fundoCorpo">
        <div class="titulo">
            <span id="descricao" class="titulo">Edição de Ponto</span>
        </div>

        <div class="fundoCabecalho">
            
            
            <div class="blocoCabecalho Preenchido">

                <div class="Cabecalho">
                    <span class="Cabecalho">Tipo de Ponto:</span>
                    <input type="checkBox" id="cbEntrada"  >Entrada               
                    <input type="checkBox" id="cbcbSaida" >Saída<br />
                    
                </div>
				<button onclick="apagar();"  class="button button5">Apagar</button>
			<button onclick="salvar();" class="button button5">Salvar</button>

            </div>
        </div>

</div>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
        <script>
          


            var path = "api/PontoModels/" + gup("Id");
            $.ajax({
                contentType: "application/json",
                type: "GET",
                url: path,
                success: function (data, status) {
                    if (status == 'success') {
                        if (data != null) {

                            document.querySelector('input[id="cbEntrada"]').checked =  data.Entrada;
                            document.querySelector('input[id="cbcbSaida"]').checked =data.Saida;
                         
                        } else {
                            alert("Falha ao tentar obter informações do ponto!");
                        }
                    }
                }
            });


            
            
            
            function apagar() {
                var path = "api/PontoModels/" + gup("Id");
                $.ajax({
                    contentType: "application/json",
                    type: "DELETE",
                    url: path,
                    success: function (data, status) {
                        if (status == 'success') {
                            if (data != null) {
                                document.location = "/tela_edicao_estacionamento.aspx?Id=" + gup("IdEstacionamento")
                            } else {
                                alert("Falha ao tentar salvar as modificações!");
                            }
                        }
                    }
                });

            }
            

            function salvar() {

               var e_entrada= document.querySelector('input[id="cbEntrada"]').checked;
                var e_saida =document.querySelector('input[id="cbcbSaida"]').checked;
                
                var path = "api/PontoModels/" + gup("Id") + "/ModificarTipo?entrada=" + e_entrada + "&saida=" + e_saida;
            $.ajax({
                contentType: "application/json",
                type: "GET",
                url: path,
                success: function (data, status) {
                    if (status == 'success') {
                         if (data != null) {
                             document.location = "/tela_edicao_estacionamento.aspx?Id="+gup("IdEstacionamento")
                        } else {
                            alert("Falha ao tentar salvar as modificações!");
                        }
                    }
                }
            });

            }
                function gup(name, url) {
                    if (!url) url = location.href;
                    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                    var regexS = "[\\?&]" + name + "=([^&#]*)";
                    var regex = new RegExp(regexS);
                    var results = regex.exec(url);
                    return results == null ? null : results[1];
                }
        </script>
    </body>
</html>
