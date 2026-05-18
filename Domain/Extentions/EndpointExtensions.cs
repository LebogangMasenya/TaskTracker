using System.Reflection;


// REFELECTION: ability to read ad act on the app dictionary while the app is running , going under the hood at runtime 
public static class EndpointExtension
{
    public static void MapAllEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IEndpoint).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract );
        // fetching the curruntly running .dll file and fetches types/methods


        foreach (var type in endpointTypes)
        {
            var method = type.GetMethod("MapEndpoints", BindingFlags.Public | BindingFlags.Static);
            method?.Invoke(null, new object[] {app});
            // first param ull bcz the tragetmethod is static (if it were an instance, we would pass an object in)
            // the new object[] passes the running web app context instance directly into the methods param list
        }
    }
}