using curso.api.Business.Entities;

namespace curso.api.Business.Repositories
{
    public interface IUsuarioRepository
    {
        void Adicionar(User usuario);
        void Commit();
        User ObterUsuario(string login);
    }
}
