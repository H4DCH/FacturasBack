using Facturas.Tests.Mocks;
using Facturas2.Controllers.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task  SiUsuarioEsAdmin() 
        {//Preparacion
            var Aut = new AuthorizationServiceMock();
            Aut.Resultado = AuthorizationResult.Success();
            var rootController = new RootController(Aut);
            rootController.Url = new UrlHelperMock();

            //Ejecucion
            var resultado =await rootController.Get();

            //Verificacion
            Assert.AreEqual(3, resultado.Value.Count());
        
        }
        [TestMethod]
        public async Task SiUsuarioNoEsAdmin()
        {//Preparacion
            var Aut = new AuthorizationServiceMock();
            Aut.Resultado = AuthorizationResult.Failed();
            var rootController = new RootController(Aut);
            rootController.Url = new UrlHelperMock();

            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(1, resultado.Value.Count());

        }
        [TestMethod]
        public async Task SiUsuarioNoEsAdminUsandoMOQ()
        {//Preparacion
            var mock = new Mock<IAuthorizationService>();
            mock.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mock.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<String>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var MockURL = new Mock<IUrlHelper>();
            MockURL.Setup(x => 
            x.Link(It.IsAny<String>(),
            It.IsAny<object>()))
                .Returns(String.Empty);
            var rootController = new RootController(mock.Object);
            rootController.Url = MockURL.Object; 

            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(1, resultado.Value.Count());

        }
    }
}
