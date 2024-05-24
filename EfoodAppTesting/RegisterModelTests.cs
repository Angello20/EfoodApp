using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc.Rendering;
using EfoodApp.Areas.Identity.Pages.Account;


namespace EfoodAppTesting
{
    public class RegisterModelTests
    {
        [Fact]
    public async Task OnGetAsync_PopulatesRolesCorrectly()
    {
        // Arrange
        var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            new Mock<IRoleStore<IdentityRole>>().Object,
            new IRoleValidator<IdentityRole>[0],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = DS.Role_Admin },
            new IdentityRole { Name = DS.Role_Seguridad },
            new IdentityRole { Name = DS.Role_Mantenimiento },
            new IdentityRole { Name = DS.Role_Consulta },
            // DS.Role_Cliente is intentionally left out to simulate your exclusion
        };

        roleManagerMock.Setup(rm => rm.Roles).Returns(roles.AsQueryable());

        var registerModel = new RegisterModel(
            new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null).Object,
            new Mock<IUserStore<IdentityUser>>().Object,
            new Mock<SignInManager<IdentityUser>>(new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null).Object, null, null, null).Object,
            new Mock<ILogger<RegisterModel>>().Object,
            new Mock<IEmailSender>().Object,
            roleManagerMock.Object
        );

        // Act
        await registerModel.OnGetAsync();

        // Assert
        Assert.NotNull(registerModel.Input.ListaRol);
        var roleList = registerModel.Input.ListaRol.ToList();
        Assert.Equal(4, roleList.Count); // Change this to the number of roles you expect
        Assert.DoesNotContain(roleList, r => r.Value == DS.Role_Cliente);
    }


    }
}
