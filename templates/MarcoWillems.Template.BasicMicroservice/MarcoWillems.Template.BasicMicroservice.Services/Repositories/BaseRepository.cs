using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MarcoWillems.Template.BasicMicroservice.Database.Context;
using MarcoWillems.Template.BasicMicroservice.Services.Attributes;
using MarcoWillems.Template.BasicMicroservice.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MarcoWillems.Template.BasicMicroservice.Services.Repositories
{
    [DiClass]
    public abstract class BaseRepository<T>
        where T : class
    {
        internal readonly CustomDbContext _db;
        internal readonly ClaimsPrincipal? _user;

        public BaseRepository(CustomDbContext db,
            IUserPrincipalAccessor userPrincipalAccessor)
        {
            _db = db;
            _user = userPrincipalAccessor?.User;
        }

        protected IQueryable<T> All => _db.Set<T>();

        protected IQueryable<T> AllNoTracking => All.AsNoTracking();

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public void Edit(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task RefreshAsync(T entity)
        {
            await _db.Entry(entity).ReloadAsync();
        }

        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
