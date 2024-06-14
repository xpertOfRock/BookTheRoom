namespace BookTheRoom.Tests
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "BookTheRoom.Domain";
        private const string ApplicationNamespace = "BookTheRoom.Application";
        private const string InfrastructureNamespace = "BookTheRoom.Infrastructure";
        private const string IdentityServerNamespace = "BookTheRoom.IdentityServer";
        private const string WebAPINamespace = "BookTheRoom.WebAPI";        
        private const string WebUINamespace = "BookTheRoom.WebUI";
       
        [Fact]
        public void Domain_ShouldNot_HaveDependencyOnAnyProjects()
        {
            var domainAssembly = typeof(ArchitectureTests).Assembly;
            var domainTypes = domainAssembly.GetTypes().Where(dt => dt.Namespace != null && dt.Namespace.StartsWith(DomainNamespace)).ToList();

            bool hasUnexpectedDependency = false;

            foreach (var domainType in domainTypes)
            {
                var referencedAssemblies = domainType.Assembly.GetReferencedAssemblies();

                foreach (var referencedAssembly in referencedAssemblies)
                {
                    if (referencedAssembly.FullName.StartsWith(ApplicationNamespace) ||
                        referencedAssembly.FullName.StartsWith(InfrastructureNamespace) ||
                        referencedAssembly.FullName.StartsWith(WebUINamespace) ||
                        referencedAssembly.FullName.StartsWith(WebAPINamespace) ||
                        referencedAssembly.FullName.StartsWith(IdentityServerNamespace))
                    {
                        hasUnexpectedDependency = true;
                        break;
                    }
                }

                if (hasUnexpectedDependency)
                    break;
            }

            Assert.False(hasUnexpectedDependency, $"Domain should not have dependencies on otheer projects.");
        }

        [Fact]
        public void Application_ShouldOnly_HaveDependencyOnDomainProject()
        {   
            var applicationAssembly = typeof(ArchitectureTests).Assembly;
            var applicationTypes = applicationAssembly.GetTypes().Where(dt => dt.Namespace != null && dt.Namespace.StartsWith(ApplicationNamespace)).ToList();

            bool hasUnexpectedDependency = false;

            foreach (var applicationType in applicationTypes)
            {
                var referencedAssemblies = applicationType.Assembly.GetReferencedAssemblies();

                foreach (var referencedAssembly in referencedAssemblies)
                {
                    if (referencedAssembly.FullName.StartsWith(InfrastructureNamespace) ||
                        referencedAssembly.FullName.StartsWith(WebUINamespace) ||
                        referencedAssembly.FullName.StartsWith(WebAPINamespace) ||
                        referencedAssembly.FullName.StartsWith(IdentityServerNamespace))
                    {
                        hasUnexpectedDependency = true;
                        break;
                    }
                }

                if (hasUnexpectedDependency)
                    break;
            }

            Assert.False(hasUnexpectedDependency, $"Application should only have dependency on {DomainNamespace}.");
        }
        [Fact]
        public void Infrastructure_ShouldOnly_HaveDependencyOnDomainAndApplicationProjects()
        {
            var infrastructureAssembly = typeof(ArchitectureTests).Assembly;
            var infrastructureTypes = infrastructureAssembly.GetTypes().Where(dt => dt.Namespace != null && dt.Namespace.StartsWith(InfrastructureNamespace)).ToList();

            bool hasUnexpectedDependency = false;

            foreach (var infrastructureType in infrastructureTypes)
            {
                var referencedAssemblies = infrastructureType.Assembly.GetReferencedAssemblies();

                foreach (var referencedAssembly in referencedAssemblies)
                {
                    if (referencedAssembly.FullName.StartsWith(ApplicationNamespace) ||
                        referencedAssembly.FullName.StartsWith(DomainNamespace))
                    {
                        continue;
                    }
                    else
                    {
                        hasUnexpectedDependency = true;
                        break;
                    }
                }

                if (hasUnexpectedDependency)
                    break;
            }

            Assert.False(hasUnexpectedDependency, $"Infrastructure should only have dependency on {DomainNamespace} and {ApplicationNamespace}.");
        }

        [Fact]
        public void WebUI_ShouldOnly_HaveDependencyOnApplicationAndInfrastructureProjects()
        {
            var webUIAssembly = typeof(ArchitectureTests).Assembly;
            var webUITypes = webUIAssembly.GetTypes().Where(dt => dt.Namespace != null && dt.Namespace.StartsWith(WebUINamespace)).ToList();

            bool hasUnexpectedDependency = false;
            foreach (var webUIType in webUITypes)
            {
                var referencedAssemblies = webUIType.Assembly.GetReferencedAssemblies();

                foreach (var referencedAssembly in referencedAssemblies)
                {
                    if (referencedAssembly.FullName.StartsWith(ApplicationNamespace) ||
                        referencedAssembly.FullName.StartsWith(InfrastructureNamespace))
                    {
                        continue;
                    }
                    else
                    {
                        hasUnexpectedDependency = true;
                        break;
                    }
                }

                if (hasUnexpectedDependency)
                    break;
            }

            Assert.False(hasUnexpectedDependency, $"WebUI should only have dependency on {ApplicationNamespace} and {InfrastructureNamespace}.");
        }
    }
}
