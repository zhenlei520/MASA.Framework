namespace Masa.Contrib.Dispatcher.Events;

public static class DispatcherOptionsExtensions
{
    public static IDispatcherOptions UseEventBus(
        this IDispatcherOptions options)
        => options.UseEventBus(ServiceLifetime.Scoped);

    public static IDispatcherOptions UseEventBus(
        this IDispatcherOptions options,
        Action<EventBusBuilder> eventBusBuilder)
        => options.UseEventBus(eventBusBuilder, ServiceLifetime.Scoped);

    public static IDispatcherOptions UseEventBus(
        this IDispatcherOptions options,
        ServiceLifetime lifetime)
        => options.UseEventBus(null, lifetime);

    public static IDispatcherOptions UseEventBus(
        this IDispatcherOptions options,
        Action<EventBusBuilder>? eventBusBuilder,
        ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(options.Services, nameof(options.Services));

        eventBusBuilder?.Invoke(new EventBusBuilder(options.Services));
        options.Services.AddEventBus(options.Assemblies, lifetime);
        return options;
    }
}
