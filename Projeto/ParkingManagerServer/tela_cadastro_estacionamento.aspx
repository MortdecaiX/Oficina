<%@ Page Language="C#" AutoEventWireup="true"  Inherits="ParkingManagerServer._default" %>
<%Server.Execute("Seguranca.aspx");%>
<html>
     <head>
	<link rel="stylesheet" type="text/css" href="style.css">
    </head>  
    <body>
		<div class="fundoCorpo">
        <div class="titulo">
            <span class="titulo">Novo Estacionamento</span>
        </div>

        <div class="fundoCabecalho">
            
            
            <div class="blocoCabecalho Esquerda">
                <div class="Cabecalho">
                    <span name="Nome" class="Cabecalho">Nome do Estacionamento:</span>
                    <input id="txNome"  type="text" class="Cabecalho"></input>
					
                </div>
				<button id="btSalvar" onclick="salvar();" class="button button5">Salvar e Editar</button>

            </div>
            
               
        </div>


         </div>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>

        <script>
       function salvar() {

           var dados = {};
           
           dados["Nome"] = document.getElementById("txNome").value;
           if (dados["Nome"] == null || dados["Nome"]=="") {
               alert('O nome do estacionamento é obrigatório!');
               return;
           }
           document.getElementById("txNome").disabled = true;
        dados["Localizacao"] = { Latitude: Number(gup("lat")), Longitude: Number(gup("lng")), Altitude: 0 };
        dados["Responsavel"] = JSON.parse($.cookie('usuario'));
        document.getElementById("btSalvar").value = "carregando...";
        $.ajax({
            contentType: "application/json",
            data: JSON.stringify(dados),
            type: "POST",
            url: "api/EstacionamentoModels",
            success: function (data, status) {
                if (status == 'success') {
                    document.getElementById("btSalvar").value = "Salvar e Editar";
                    document.getElementById("txNome").disabled = false;

                    if (data != null) {
                        $.cookie("estacionamento", JSON.stringify(data));
                        document.location = "/tela_edicao_estacionamento.aspx";
                    } else {
                        alert("Falha ao cadastrar estacionamento!");
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
