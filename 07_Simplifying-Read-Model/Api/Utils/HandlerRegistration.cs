using Logic.AppServices;
using Logic.Decorators;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Api.Utils;

public static class HandlerRegistration
{
    public static void AddHandlers(this IServiceCollection services)
    {
        List<Type> handlerTypes = typeof(ICommand).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList();

        foreach (Type type in handlerTypes)
        {
            AddHandler(services, type);
        }
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        Type typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
    }

    private static void AddHandler(IServiceCollection services, Type type)
    {
        object[] attributes = type.GetCustomAttributes(false);

        List<Type> pipeline = attributes
            .Where(x => x.GetType() != typeof(NullableAttribute) &&
                        x.GetType() != typeof(NullableContextAttribute))
            .Select(ToDecorator)
            .Concat(new[] { type })
            .Reverse()
            .ToList();

        Type interfaceType = type.GetInterfaces().Single(y => IsHandlerInterface(y));
        Func<IServiceProvider, object> factory = BuildPipeline(pipeline, interfaceType);

        services.AddScoped(interfaceType, factory);
    }

    private static Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
    {
        List<ConstructorInfo> ctors = pipeline
            .Select(x =>
            {
                Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                return type.GetConstructors().Single();
            })
            .ToList();

        object func(IServiceProvider provider)
        {
            object? current = null;

            foreach (ConstructorInfo ctor in ctors)
            {
                List<ParameterInfo> parameterInfos = [.. ctor.GetParameters()];

                object?[] parameters = GetParameters(parameterInfos, current, provider);

                current = ctor.Invoke(parameters);
            }

            return current ?? throw new InvalidOperationException();
        }

        return func;
    }

    private static object?[] GetParameters(List<ParameterInfo> parameterInfos, object? current, IServiceProvider provider)
    {
        var result = new object?[parameterInfos.Count];

        for (int i = 0; i < parameterInfos.Count; i++)
        {
            result[i] = GetParameter(parameterInfos[i], current, provider);
        }

        return result;
    }

    private static object? GetParameter(ParameterInfo parameterInfo, object? current, IServiceProvider provider)
    {
        Type parameterType = parameterInfo.ParameterType;

        if (IsHandlerInterface(parameterType))
            return current;

        object service = provider.GetRequiredService(parameterType);
        if (service != null)
            return service;

        throw new ArgumentException($"Type {parameterType} not found");
    }

    private static Type ToDecorator(object attribute)
    {
        Type type = attribute.GetType();

        // The IQueryHandler's arity (2) differs from a decorators' (1)!
        if (type == typeof(AuditLogAttribute))
            return typeof(AuditLoggingDecorator<>);

        // other attributes go here

        throw new ArgumentException(attribute.ToString());
    }
}
