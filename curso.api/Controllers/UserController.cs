using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Filters;
using curso.api.Models;
using curso.api.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace curso.api.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;

        public UserController(
            IUsuarioRepository usuarioRepository, 
            IAuthenticationService authenticationService)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
        }

        ///<summary>
        /// Este serviço permite a validação de login do usuario.
        ///</summary>
        ///<param name = "loginViewModelInput" >View model de login</param>
        ///<returns>Retorna o token e o usuario</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar o usuário", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCamposViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput)
        {
            User user = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);

            if (user == null)
            {
                return BadRequest("Houve um erro ao tentar acessar.");
            }

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = user.Codigo,
                Login = user.Login,
                Email = user.Email
            };

            var token = _authenticationService.GerarToken(usuarioViewModelOutput);

            return Ok(new 
            {
                Token = token,
                Usuario = usuarioViewModelOutput
            });
        }

        ///<summary>
	    /// Este serviço permite registrar um novo usuario.
        ///</summary>
        ///<param name="registroViewModelInput">View model de registro de login</param>
        ///<returns>Retorna o usuario criado</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao registrar o usuário", Type = typeof(RegistroViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCamposViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistroViewModelInput registroViewModelInput)
        {
            var usuario = new User();
            usuario.Login = registroViewModelInput.Login;
            usuario.Senha = registroViewModelInput.Senha;
            usuario.Email = registroViewModelInput.Email;

            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Commit();

            return Created("", registroViewModelInput);
        }
    }
}
