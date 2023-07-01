using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core.Module.Extensions
{
    public static class MethodExtensions
    {
        public static string ResolveEntityMethodHandlerDefinition(this IMethodSymbol method, string rootType, string rootName)
        {
            var parameters = new[]{$"{rootType} {rootName}"}.Concat(method.Parameters.Select(p => $"{(p.RefKind == RefKind.Ref ? "ref " : string.Empty)}{p.Type} {p.Name}"));

            var arguments = method.Parameters.Select(p => $"{(p.RefKind == RefKind.Ref ? "ref " : string.Empty)}{p.Name}");

            var attributes = method.GetAttributes().SelectMany(a => ResolveAttribute(a));

            return $@"{string.Join(string.Empty, attributes.Select(a => $@"
    [{a}]"))}
    public static {method.ReturnType} {method.Name}({string.Join(", ", parameters)})
    {{
        {(method.ReturnsVoid ? string.Empty : "return ")}{rootName}.{method.Name}({string.Join(", ", arguments)});
    }}";
        }

        private static IEnumerable<string> ResolveAttribute(AttributeData attribute, int width = 0, int height = 0)
        {
            if (width == 0 && height == 0 && !IsNestedCoreAttribute(attribute.AttributeClass) || IsCoreAttribute(attribute.AttributeClass)) return new[] { $"{attribute}" };

            return ResolveAttribute(attribute.AttributeClass, attribute.AttributeConstructor, ResolveConstructorArguments(attribute), width, height);
        }

        private static IEnumerable<string> ResolveAttribute(ITypeSymbol @class, IMethodSymbol constructor, IEnumerable<string> arguments, int width, int height)
        {
            if (!IsNestedCoreAttribute(@class)) yield break;

            var otherAttributes = @class.GetAttributes();

            foreach (var other in otherAttributes.SelectMany(a => ResolveAttribute(a, width + 1, height)))
            {
                yield return other;
            }

            if (IsCoreAttribute(@class))
            {
                var result = $"{@class}({string.Join(", ", arguments)})";

                yield return result;
            }
            else
            {
                var inherited = ResolveInheritedAttribute(@class, constructor, ResolveConstructorDictionary(constructor, arguments), width, height);

                if(inherited == null) yield break;

                foreach (var attribute in inherited)
                {
                    yield return attribute;
                }
            }
        }
        private static IEnumerable<string> ResolveInheritedAttribute(ITypeSymbol @class, IMethodSymbol constructor, Dictionary<string, string> incoming, int width, int height)
        {
            if (@class.BaseType == null) return null;

            var outgoing = ResolveBaseConstructorDictionary(constructor, @class.BaseType, out var baseConstructor);

            if(outgoing == null) return null;

            foreach (var argument in incoming)
            {
                outgoing[argument.Key] = argument.Value;
            }

            return ResolveAttribute(@class.BaseType, baseConstructor, outgoing.Values.ToArray(), width, height + 1);
        }

        private static Dictionary<string, string> ResolveBaseConstructorDictionary(IMethodSymbol constructor, INamedTypeSymbol baseClass, out IMethodSymbol baseConstructor)
        {
            baseConstructor = baseClass.Constructors[0];

            var baseArguments = ResolveConstructorArguments(constructor);

            return baseArguments == null ? null : ResolveConstructorDictionary(baseConstructor, baseArguments);
        }

        private static Dictionary<string, string> ResolveConstructorDictionary(IMethodSymbol constructor, IEnumerable<string> arguments)
        {
            return constructor.Parameters.Zip(arguments, (p, a) => (p, a))
                .ToDictionary(g => g.p.Name, g => g.a);
        }

        private static IEnumerable<string> ResolveConstructorArguments(ISymbol constructor)
        {
            if (constructor.DeclaringSyntaxReferences.Length != 1) return null;

            var syntax = (ConstructorDeclarationSyntax)constructor.DeclaringSyntaxReferences[0].GetSyntax();

            return syntax.Initializer == null ? null : ResolveConstructorArguments(syntax.Initializer.ArgumentList.Arguments);
        }

        private static IEnumerable<string> ResolveConstructorArguments(AttributeData attribute)
        {
            var syntax = attribute.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;

            if (syntax?.ArgumentList == null) yield break;

            foreach (var argument in ResolveConstructorArguments(syntax.ArgumentList.Arguments))
            {
                yield return argument;
            }
        }

        private static IEnumerable<string> ResolveConstructorArguments(object arguments)
        {
            return arguments.ToString().Replace(" ", string.Empty).Split(',');
        }

        private static bool IsNestedCoreAttribute(ITypeSymbol attributeClass)
        {
            if (attributeClass == null) return false;

            if (IsCoreAttribute(attributeClass)) return true;

            return attributeClass.BaseType != null && IsNestedCoreAttribute(attributeClass.BaseType);
        }

        private static bool IsCoreAttribute(ISymbol attributeClass)
        {
            if (attributeClass == null) return false;

            return attributeClass.ContainingNamespace.ToString() == "Core.Abstract.Attributes";
        }
    }
}
