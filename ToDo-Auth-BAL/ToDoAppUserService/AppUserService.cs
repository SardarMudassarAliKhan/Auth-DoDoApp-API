using ToDo_Auth_DAL.IRepositories;
using ToDo_Auth_DAL.Models;

namespace ToDo_Auth_BAL.ToDoAppUserService
{
    internal class AppUserService
    {
        public readonly IToDoRepository<AuthToDoUserEntity> _repository;
        public AppUserService(IToDoRepository<AuthToDoUserEntity> repository)
        {
            _repository = repository;
        }
        //Create Method 
        public async Task<AuthToDoUserEntity> AddUser(AuthToDoUserEntity appUser)
        {
            try
            {
                if (appUser == null)
                {
                    throw new ArgumentNullException(nameof(appUser));
                }
                else
                {
                    return await _repository.Create(appUser);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteUser(int Id)

        {
            try
            {
                if (Id != 0)
                {
                    var obj = _repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
                    _repository.Delete(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateUser(int Id)

        {
            try
            {
                if (Id != 0)
                {
                    var obj = _repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
                    if (obj != null)
                        _repository.Update(obj);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<AuthToDoUserEntity> GetAllUser()
        {
            try
            {
                return _repository.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
