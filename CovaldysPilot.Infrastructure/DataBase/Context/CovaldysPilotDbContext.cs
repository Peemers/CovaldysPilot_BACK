using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.DataBase.Context;

public class CovaldysPilotDbContext(DbContextOptions<CovaldysPilotDbContext> options) : DbContext(options);