using System.Collections.Generic;
using EavSandbox.Web.EntityFramework.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EavSandbox.Web
{
    public class ContextFactory
    {
        private readonly Dictionary<string, EavContext> _contexts = new Dictionary<string, EavContext>();
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextFactory(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Provides appropriate context based on route data.
        /// </summary>
        public EavContext CreateContext()
        {
            var connectionName = (string)_httpContextAccessor.HttpContext.GetRouteData().Values["db"];
            if (!_contexts.TryGetValue(connectionName, out var context))
            {
                var connection = _configuration.GetConnectionString(connectionName);
                if (connection == null) throw new System.Exception($"Unknown database {connectionName} passed.");
                var options = new DbContextOptionsBuilder().UseSqlServer(connection).Options;
                context = new EavContext(options);
                context.Database.EnsureCreated();
            }

            return context;
        }
    }
}
