using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class MethodExtensions
    {
        public static IEnumerable<IMethodSymbol> GetNestedMethods(this INamedTypeSymbol type)
        {
            foreach (var method in type.GetMembers().OfType<IMethodSymbol>().Where(m => m.DeclaredAccessibility == Accessibility.Public))
            {
                yield return method;
            }

            foreach (var @interface in type.AllInterfaces)
            {
                foreach (var method in @interface.GetMembers().OfType<IMethodSymbol>().Where(m => m.DeclaredAccessibility == Accessibility.Public))
                {
                    yield return method;
                }
            }
        }
    }
}
