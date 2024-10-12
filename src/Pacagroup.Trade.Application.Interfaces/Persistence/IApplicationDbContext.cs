using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pacagroup.Trade.Domain.Entities;

namespace Pacagroup.Trade.Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
