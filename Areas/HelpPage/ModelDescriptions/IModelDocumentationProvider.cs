using System;
using System.Reflection;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}