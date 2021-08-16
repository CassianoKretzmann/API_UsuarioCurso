using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using System.Linq;

namespace curso.api.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CursoDBContext _contexto;

        public UsuarioRepository(CursoDBContext contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(User usuario)
        {
            _contexto.Add(usuario);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public User ObterUsuario(string login)
        {
           return _contexto.User.FirstOrDefault(u => u.Login == login);
        }
    }
}
