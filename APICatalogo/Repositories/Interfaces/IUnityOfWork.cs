namespace APICatalogo.Repositories.Interfaces
{
    public interface IUnityOfWork
    {
        //Eu poderia implementar o repositório genérico mas perderia a capacidade de implementar métodos personalizados
        //IRepository<Produto> ProdutoRepository { get; }
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}
