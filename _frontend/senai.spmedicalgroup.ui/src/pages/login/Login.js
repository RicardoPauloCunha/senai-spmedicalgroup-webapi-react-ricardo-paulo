import React, { Component } from "react";
import Axios from "axios";
import { parseJwt } from "../../services/auth";

class Login extends Component {
    constructor() {
        super();

        this.state = {
            email: "",
            senha: ""
        }

        this.atualizarEmail = this.atualizarEmail.bind(this);
        this.atualizarSenha = this.atualizarSenha.bind(this);
    }

    atualizarEmail(event){
        this.setState({email: event.target.value});
    }

    atualizarSenha(event) {
        this.setState({senha: event.target.value});
    }

    efetuarLogin(event){
        event.preventDefault();

        Axios.post('http://localhost:5000/api/Login', {
            email : this.state.email,
            senha : this.state.senha
        })
        .then(data => {
            if(data.status === 200) {
                localStorage.setItem("usuarioautenticado-token-spmedgroup", data.data.token);
                if(parseJwt().UsuarioTipo == "1"){
                    this.props.history.push("/Dashboard");
                } else if (parseJwt().UsuarioTipo == "2"){
                    this.props.history.push("/ConsultasMedico");
                } else if (parseJwt().UsuarioTipo == "3"){
                    this.props.history.push("/ConsultasPaciente");
                }
            };
        })
        .catch(erro => console.log(erro))
    }

    render() {
        return (
            <form onSubmit={this.efetuarLogin.bind(this)}>
                <input type="email" placeholder="Email:" value={this.state.email} onChange={this.atualizarEmail}/>
                <input type="password" placeholder="Senha" value={this.state.senha} onChange={this.atualizarSenha}/>

                <button type="submit">Entrar</button>
            </form>
        )
    }
}

export default Login;