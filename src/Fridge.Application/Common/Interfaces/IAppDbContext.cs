
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Domain.Entities.Fridge> Fridges { get; }

}