using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class SymbolExtensions
    {
        public static IEnumerable<INamedTypeSymbol> FindRecursive(this INamespaceOrTypeSymbol source, Func<INamedTypeSymbol, bool> condition)
        {
            if (source is INamedTypeSymbol target && condition(target)) yield return target;

            foreach (var next in source.GetMembers().OfType<INamespaceOrTypeSymbol>())

            foreach (var recursive in FindRecursive(next, condition)) yield return recursive;
        }
    }
}
