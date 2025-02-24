using PetInsurancePlatform.Insurance.Application;
using System.Reflection;

namespace PetInsurancePlatform.Insurance.Endpoints;

public static class InsuranceEndpoints
{
    public static readonly IEnumerable<Assembly> Assemblies =
        [
            Assembly.GetExecutingAssembly(),
            AssemblyReference.Assembly
        ];
}
