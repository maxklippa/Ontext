using System;
using System.Data.Entity;
using Ontext.DAL.Models;

namespace Ontext.DAL.Context.Contracts
{
    /// <summary>
    /// Ontext Database context interface
    /// </summary>
    public interface IOntextDbContext : IDisposable
    {
        /// <summary>
        /// Gets generic db context instance
        /// </summary>
        DbContext DbContext { get; }
    }
}