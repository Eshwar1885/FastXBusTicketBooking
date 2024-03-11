using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class AdminRepository : IRepository<int, Admin>
    {
        private readonly FastXContext _context;

        public AdminRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<Admin> Add(Admin item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Task<Admin> Delete(int key)
        {
            throw new NotImplementedException();
        }
        //----------------------------------

        //public async Task<Admin> Delete(int key)
        //{
        //    var admin = await GetAsync(key);
        //    _context?.Admins.Remove(admin);
        //    _context.SaveChanges();
        //    return admin;
        //}
        //public async Task<Admin> Delete(string key)
        //{
        //    var admin = await GetAsync(key);
        //    _context?.Admins.Remove(admin);
        //    _context.SaveChanges();
        //    return admin;
        //}
        //---------------------------------

        public Task<List<Admin>> GetAsync()
        {
            throw new NotImplementedException();
        }
        //public async Task<Admin> GetAsync(string key)
        //{
        //    var admin = _context.Admins.SingleOrDefault(u => u.Username == key);
        //    return admin;
        //}
        public Task<Admin> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> Update(Admin item)
        {
            throw new NotImplementedException();
        }
    }




    //public class AdminRepository<T> : IAdminRepository<T> where T : class
    //{
    //    private readonly DbContext _context;
    //    private readonly DbSet<T> _dbSet;

    //    public AdminRepository(DbContext context)
    //    {
    //        _context = context;
    //        _dbSet = context.Set<T>();
    //    }

    //    public async Task<T> GetByIdAsync(object id)
    //    {
    //        return await _dbSet.FindAsync(id);
    //    }

    //    public async Task DeleteAsync(object id)
    //    {
    //        var entity = await _dbSet.FindAsync(id);
    //        if (entity != null)
    //            _dbSet.Remove(entity);
    //    }

    //    public async Task SaveChangesAsync()
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //}
}
