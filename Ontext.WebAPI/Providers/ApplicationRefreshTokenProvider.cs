using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;
using Ontext.Server.Core.Utils;

namespace Ontext.WebAPI.Providers
{
    public class ApplicationRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            var task = CreateAsync(context);
            task.Wait();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken()
            {
                Id = HashHelper.GetHash(refreshTokenId),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            await context.OwinContext.Get<IUnitOfWork>().GetCustomRepository<RefreshTokenRepository>().AddRefreshToken(token);
            context.OwinContext.Get<IUnitOfWork>().SaveChanges();

            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            var task = ReceiveAsync(context);
            task.Wait();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = HashHelper.GetHash(context.Token);

            var refreshToken = await context.OwinContext.Get<IUnitOfWork>().GetCustomRepository<RefreshTokenRepository>().FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                await context.OwinContext.Get<IUnitOfWork>().GetCustomRepository<RefreshTokenRepository>().RemoveRefreshToken(hashedTokenId);
                context.OwinContext.Get<IUnitOfWork>().SaveChanges();
            }
        }
    }
}