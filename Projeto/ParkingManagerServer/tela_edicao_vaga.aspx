<%@ Page Language="C#" AutoEventWireup="true"  Inherits="ParkingManagerServer._default" %>
<%Server.Execute("Seguranca.aspx");%>
<html>
      <head>
	<link rel="stylesheet" type="text/css" href="style.css">
     <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    </head>  
    <body>
	<div class="fundoCorpo">
        <div class="titulo">
            <span id="descricao" class="titulo">Edição da Vaga</span>
        </div>

        <div class="fundoCabecalho">
            
            
            <div class="blocoCabecalho Preenchido">

                <div class="Cabecalho">
                    <span class="Cabecalho">Tipo de Vaga:</span>
                    <input type="radio" value="Livre" name="tipoVaga">Livre<br />
                
                    <input type="radio" value="Moto" name="tipoVaga">Moto<br />
                    <input type="radio" value="Idoso" name="tipoVaga">Idoso<br />
                
                    <input type="radio" value="Especial" name="tipoVaga">Especial<br />
               
                    <input type="radio" value="Reservada" name="tipoVaga">Reservada<br />
                </div>
				<button onclick="apagar();"  class="button button5">Apagar</button>
			<button onclick="salvar();" class="button button5">Salvar</button>

            </div>
        </div>

</div>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
        <script>
            var tipoVaga = gup("tipo");
            switch (tipoVaga) {
                case "0":document.querySelector('input[value="Livre"]').checked = true; break;
                case "1" :document.querySelector('input[value="Idoso"]').checked = true; break;
                case "2" :document.querySelector('input[value="Especial"]').checked = true; break;
                case "3" :document.querySelector('input[value="Moto"]').checked = true; break;
                case "4" :document.querySelector('input[value="Reservada"]').checked = true; break;
                default: document.querySelector('input[value="Livre"]').checked = true; break;
            }
            
            function apagar() {
                var path = "api/VagaModels/" + gup("Id");
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

                

                var tipo = -1;
                var valor = document.querySelector('input[name="tipoVaga"]:checked').value;
                switch (valor) {
                    case "Livre": tipo = 0; break;
                    case "Idoso": tipo = 1; break;
                    case "Especial": tipo = 2; break;
                    case "Moto": tipo = 3; break;
                    case "Reservada": tipo = 4; break;
                    default: tipo = 0; break;
                }
                var path = "api/VagaModels/" + gup("Id") + "/ModificarTipo?tipoVaga=" + tipo;
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
