using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class EntityExtensions
    {
        public static IEnumerable<INamedTypeSymbol> GetEntityAndElementConstructs(this IAssemblySymbol assembly)
        {
            return assembly.GlobalNamespace.FindRecursive(HasEntityAttribute).Concat(assembly.GlobalNamespace.FindRecursive(HasElementAttribute)).ToList();
        }

        public static bool HasEntityAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsEntityAttribute);
        }

        public static bool HasElementAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsElementAttribute);
        }

        public static bool HasHandlersAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsHandlersAttribute);
        }

        public static AttributeData GetElementAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Single(IsElementAttribute);
        }

        public static AttributeData GetEntityAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Single(IsEntityAttribute);
        }

        public static bool IsEntityAttribute(this AttributeData attribute)
        {
            return attribute.IsAttribute("EntityAttribute");
        }

        public static bool IsElementAttribute(this AttributeData attribute)
        {
            return attribute.IsAttribute("ElementAttribute");
        }

        public static bool IsHandlersAttribute(this AttributeData attribute)
        {
            return attribute.IsAttribute("HandlersAttribute");
        }

        public static bool IsAttribute(this AttributeData attribute, string name)
        {
            return attribute.AttributeClass != null && attribute.AttributeClass.Name.EndsWith(name);
        }
    }
}
