using Microsoft.Extensions.Logging;
using System;
using ToDo_Auth_DAL.DbContext;
using ToDo_Auth_DAL.IRepositories;
using ToDo_Auth_DAL.Models;

namespace ToDo_Auth_DAL.Repositories
{
    public class ToDoRepository: IToDoRepository<AuthToDoUserEntity>
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger _logger;

        public ToDoRepository(ILogger<AuthToDoUserEntity> logger)
        {
            _logger = logger;
        }

        public async Task<AuthToDoUserEntity> Create(AuthToDoUserEntity appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Add<AuthToDoUserEntity>(appuser);
                    await _appDbContext.SaveChangesAsync();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(AuthToDoUserEntity appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Remove(appuser);
                    if (obj != null)
                    {
                        _appDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<AuthToDoUserEntity> GetAll()
        {
            try
            {
                var obj = _appDbContext.AppUsers.ToList();
                if (obj != null)
                    return obj;
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AuthToDoUserEntity GetById(int Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == Id);
                    if (Obj != null)
                        return Obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(AuthToDoUserEntity appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Update(appuser);
                    if (obj != null)
                        _appDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
