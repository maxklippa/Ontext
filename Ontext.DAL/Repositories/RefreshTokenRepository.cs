using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.DAL.Repositories
{
    public class RefreshTokenRepository : CustomRepository<RefreshTokenRepository, RefreshToken>
    {
        public RefreshTokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task AddRefreshToken(RefreshToken token)
        {
            var existingToken = DbSet.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                await RemoveRefreshToken(existingToken);
            }

            DbSet.Add(token);
        }

        public async Task RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await DbSet.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                DbSet.Remove(refreshToken);
            }
        }

        public Task RemoveRefreshToken(RefreshToken refreshToken)
        {
            var t = new Task(() => DbSet.Remove(refreshToken));
            t.Start();
            return t;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await DbSet.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return DbSet.ToList();
        }
    }
}