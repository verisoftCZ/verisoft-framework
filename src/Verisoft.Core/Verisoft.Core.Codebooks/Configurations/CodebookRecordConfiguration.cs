namespace Verisoft.Core.Codebooks.Configurations;

public class CodebookRecordConfiguration
{
    public required Type CodebookRecordType { get; set; }

    public string CodebookRecordTableName => CodebookRecordType.Name;

    public string CodebookRecordEndpointName => CodebookRecordType.Name;

    public List<CustomEndpointDefinition> CustomEndpoints { get; } = [];

    public CodebookRecordRequiredPermissionConfiguration CodebookRecordRequiredPermissionConfiguration { get; set; } = new();

    public Type? CustomRepositoryType { get; set; }

    public Type? CustomRepositoryInterfaceType { get; set; }

    public Type? CustomCodebookRecordServiceType { get; set; }

    public Type? CustomCodebookRecordServiceInterfaceType { get; set; }

    public Type? CustomStringValueServiceType { get; set; }

    public Type? CustomStringValueServiceInterfaceType { get; set; }

    public Type? CustomTranslationRepositoryType { get; set; }

    public Type? CustomTranslationRepositoryInterfaceType { get; set; }

    public CodebookRecordConfiguration WithCustomEndpoint(
        string route,
        HttpMethod method,
        Delegate handler,
        string? name = null,
        string? description = null)
    {
        CustomEndpoints.Add(new CustomEndpointDefinition
        {
            Route = route,
            Method = method,
            Handler = handler,
            Name = name,
            Description = description,
        });

        return this;
    }

    public CodebookRecordConfiguration WithResponseType<T>(int statusCode)
    {
        var lastEndpoint = CustomEndpoints.LastOrDefault();
        lastEndpoint?.ResponseTypes.Add((typeof(T), statusCode));

        return this;
    }

    public CodebookRecordConfiguration AddCustomPermission(string methodName, params string[] permissions)
    {
        if (string.IsNullOrWhiteSpace(methodName))
        {
            throw new ArgumentException("Method name cannot be empty.", nameof(methodName));
        }

        if (permissions?.Length is null or 0)
        {
            return this;
        }

        var permConfig = CodebookRecordRequiredPermissionConfiguration;

        if (permConfig.CustomPermissions.TryGetValue(methodName, out var existingPermissions))
        {
            permConfig.CustomPermissions[methodName] = [.. (existingPermissions ?? []).Union(permissions)];
        }
        else
        {
            permConfig.CustomPermissions.Add(methodName, [.. permissions.Distinct()]);
        }

        return this;
    }

    public CodebookRecordConfiguration AddGlobalEditPermission(params string[] permissions)
    {
        if (permissions?.Length is null or 0)
        {
            return this;
        }

        var permConfig = CodebookRecordRequiredPermissionConfiguration;
        permConfig.GlobalEditPermissions = [.. permConfig.GlobalEditPermissions.Union(permissions)];
        return this;
    }

    public CodebookRecordConfiguration AddGlobalReadPermission(params string[] permissions)
    {
        if (permissions?.Length is null or 0)
        {
            return this;
        }

        var permConfig = CodebookRecordRequiredPermissionConfiguration;
        permConfig.GlobalReadPermissions = [.. permConfig.GlobalReadPermissions.Union(permissions)];
        return this;
    }

    public CodebookRecordConfiguration WithRepository<TRepositoryInterface, TRepository>()
    where TRepositoryInterface : class
    where TRepository : class
    {
        CustomRepositoryInterfaceType = typeof(TRepositoryInterface);
        CustomRepositoryType = typeof(TRepository);
        return this;
    }

    public CodebookRecordConfiguration WithRepository<TRepository>()
        where TRepository : class
    {
        CustomRepositoryType = typeof(TRepository);
        return this;
    }

    public CodebookRecordConfiguration WithTranslationRepository<TTranslationRepositoryInterface, TTranslationRepository>()
        where TTranslationRepositoryInterface : class
        where TTranslationRepository : class
    {
        CustomTranslationRepositoryInterfaceType = typeof(TTranslationRepositoryInterface);
        CustomTranslationRepositoryType = typeof(TTranslationRepository);
        return this;
    }

    public CodebookRecordConfiguration WithTranslationRepository<TTranslationRepository>()
        where TTranslationRepository : class
    {
        CustomTranslationRepositoryType = typeof(TTranslationRepository);
        return this;
    }

    public CodebookRecordConfiguration WithStringValueService<TServiceInterface, TService>()
        where TServiceInterface : class
        where TService : class
    {
        CustomStringValueServiceInterfaceType = typeof(TServiceInterface);
        CustomStringValueServiceType = typeof(TService);
        return this;
    }

    public CodebookRecordConfiguration WithStringValueService<TService>()
        where TService : class
    {
        CustomStringValueServiceType = typeof(TService);
        return this;
    }

    public CodebookRecordConfiguration WithCodebookRecordService<TCodebookRecordServiceInterface, TCodebookRecordService>()
        where TCodebookRecordServiceInterface : class
        where TCodebookRecordService : class
    {
        CustomCodebookRecordServiceInterfaceType = typeof(TCodebookRecordServiceInterface);
        CustomCodebookRecordServiceType = typeof(TCodebookRecordService);
        return this;
    }

    public CodebookRecordConfiguration WithCodebookRecordService<TCodebookRecordService>()
        where TCodebookRecordService : class
    {
        CustomCodebookRecordServiceType = typeof(TCodebookRecordService);
        return this;
    }
}
