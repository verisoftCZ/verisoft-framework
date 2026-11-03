using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.ConfigurationStorage;

namespace Verisoft.DemoApi.Host.Controllers.V2;

[ApiVersion("2.0")]
[Route("v{v:apiVersion}/component-configurations")]
public sealed class ComponentConfigurationsController(IConfigurationStorage storage) : ConfigurationStorageControllerBase(storage);