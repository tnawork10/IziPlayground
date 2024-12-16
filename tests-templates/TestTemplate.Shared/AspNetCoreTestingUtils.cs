using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Controllers;
using СommissioningService.IntegrationTests;

namespace IntegrationTests
{
    public class AspNetCoreTestingUtils
    {
        public static async Task Test<T>(WebApplicationFactory<T> factory) where T : class
        {
            await Task.CompletedTask;
            var lookups = GetEndpoints(factory);
            var infos = ToActionInfo(lookups);
            foreach (var info in infos)
            {
                TestEndpoint(info);
            }
        }

        public static IEnumerable<Endpoint> GetEndpoints<T>(WebApplicationFactory<T> factory) where T : class
        {
            var builder = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Disable startup discovery to prevent automatic startup
                        // services.AddSingleton<IStartupFilter>();
                    });
                });

            var server = builder.Server;
            var endpointDataSource = server.Services.GetRequiredService<EndpointDataSource>();
            var endpoints = endpointDataSource.Endpoints;
            return endpoints;
        }

        public static IEnumerable<ActionInfo> ToActionInfo(IEnumerable<Endpoint> endpoints)
        {
            return endpoints.Select(x => CreateActionInfo(x));
        }

        public static bool IsMinimalApi(Endpoint endpoint)
        {
            return endpoint.DisplayName == "HTTP:GET" && !endpoint.Metadata.Any(x => x is ControllerActionDescriptor);
        }

        public static bool IsControllerAction(Endpoint endpoint)
        {
            return endpoint.Metadata.Any(x => x is ControllerActionDescriptor);
        }

        public static ActionInfo CreateActionInfo(Endpoint endpoint)
        {
            MethodInfo? methodInfo = default;
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                var metaData = routeEndpoint.Metadata.GetRequiredMetadata<HttpMethodMetadata>();
                if (metaData.HttpMethods.Any(x => x == "GET"))
                {
                }
                else if (metaData.HttpMethods.Any(x => x == "POST"))
                {
                }

                var patter = routeEndpoint.RoutePattern;
                var descriptor = routeEndpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (descriptor != null)
                {
                    MethodInfo mi = descriptor.MethodInfo;
                    methodInfo = mi;
                }

                var iroute = endpoint.Metadata.FirstOrDefault(x => x is IRouteDiagnosticsMetadata) as IRouteDiagnosticsMetadata;
                var route = iroute?.Route ?? string.Empty;


                var runtimeMethodInfo = endpoint.Metadata.Where(x => x.GetType().IsSubclassOf(typeof(MethodInfo)));
                if (runtimeMethodInfo.Count() > 0)
                {
                    // internal RuntimeMethodInf, значит делегат 
                }

                ArgumentNullException.ThrowIfNull(methodInfo);

                return new ActionInfo()
                {
                    endpoint = routeEndpoint,
                    handler = methodInfo,
                    model = default!,
                    method = default!,
                };
            }
            else
            {
                throw new NotImplementedException(endpoint.GetType().FullName);
            }
        }

        public static async Task TestEndpoint(ActionInfo actionInfo)
        {
            ArgumentNullException.ThrowIfNull(actionInfo.handler);
            //var mi = actionInfo.handler;

            //if (mi.IsSingleArgument())
            //{
            //    await MethodSingleArgumentTester.TestAsync(actionInfo.target, mi, actionInfo.model!);
            //}
        }
    }
}

public class ActionInfo
{
    public Endpoint? endpoint;
    public HttpMethod? method;
    public MethodInfo? handler;
    public Type? model;
    public string? route;
    public string? pattern;
    public object? target;
}