namespace ToDo_Auth_DAL.IRepositories
{
    public interface IToDoRepository <T> where T : class
    {
        public Task<T> Create(T _object);

        public void Delete(T _object);

        public void Update(T _object);

        public IEnumerable<T> GetAll();

        public T GetById(int Id);
    }
}
