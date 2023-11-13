using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Super_Cartes_Infinies.Data
{
    public class CosmosUserStore : UserStore<IdentityUser, IdentityRole, ApplicationDbContext>
    {
        public CosmosUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        public override async Task<IList<string>> GetRolesAsync(IdentityUser user,
                                                                CancellationToken cancellationToken =
                                                                    new CancellationToken())
        {
            // force the enumerable to execute rather than joining
            var roles = await Context.Set<IdentityRole>().ToDictionaryAsync(r => r.Id, cancellationToken);

            var userRolesIds = await Context.Set<IdentityUserRole<string>>()
                                         .Where(ur => ur.UserId == user.Id)
                                         .Select(ur => ur.RoleId)
                                         .ToListAsync(cancellationToken);

            return userRolesIds.Select(ur => roles[ur].Name).ToList();
        }
    }
}
