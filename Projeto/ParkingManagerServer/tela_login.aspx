<html>
    <head>
	<link rel="stylesheet" type="text/css" href="style.css?t=<%= DateTime.Now.Ticks %>"  />
	    <meta name="viewport" content="width=device-width, initial-scale=1">
    </head>

    <body>
	<div class="fundoCorpo"> 
        <div class="titulo">
            <span class="titulo">Login</span>
        </div>

        <div class="fundoCabecalho">
            
            
            <div class="blocoCabecalho Preenchido">
                <div class="Cabecalho">
                    <span class="Cabecalho">Email:</span>
                    <input id="txEmail" name="Email" type="text" class="Cabecalho"/>
                </div>
				<div class="Cabecalho">
                    <span class="Cabecalho">Senha:</span>
                    <input id="txSenha" name="Senha" type="text" class="Cabecalho"/>
                </div>
				<button id="btLogin" onClick="logon();" class="button button5">Entrar</button>
                <button onClick="" class="button button5">Cadastrar</button>
            </div>
            
             
        </div>

</div>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
        <script>
    function logon() {

        var dados = {};
        dados["Email"] = document.getElementById("txEmail").value;
        dados["Senha"] = document.getElementById("txSenha").value;
        document.getElementById("txEmail").disabled = true;
        document.getElementById("txSenha").disabled = true;
        document.getElementById("btLogin").value = "carregando...";
        $.ajax({
            contentType: "application/json",
            data: JSON.stringify(dados),
            type: "POST",
            url: "/api/UsuarioModels/Logon",
            success: function (data, status) {
                if (status == 'success') {
                    document.getElementById("btLogin").value = "Entrar";
                    document.getElementById("txEmail").disabled = false;
                    document.getElementById("txSenha").disabled = false;
                    if (data != null) {
                        $.cookie("usuario", JSON.stringify(data));
                        document.location = "Default.aspx";
                    } else {
                        alert("Email ou Senha inválidos")
                    }
                }
            }
        });
    }

        </script>
    </body>
    
</html>
