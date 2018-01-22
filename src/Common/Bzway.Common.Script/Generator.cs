using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.Loader;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Bzway.Common.Script
{
    public class Generator
    {
        public async void LoadFileAsync(IFileProvider fileProvider)
        {
            fileProvider.GetDirectoryContents("").FirstOrDefault();
            var task = Task.Run(() => Generate(fileProvider.GetFileInfo("").PhysicalPath));
            await task;
        }
        public void Watch(string viewDirectory)
        {
            IFileProvider fileProvider = new PhysicalFileProvider(viewDirectory);

            ChangeToken.OnChange(() => fileProvider.Watch("**/*.cshtml"), () => LoadFileAsync(fileProvider));
        }
        public Assembly Generate(string viewDirectory)
        {

            var tempPath = Path.Combine(viewDirectory, "~temp");
            foreach (var fileName in Directory.EnumerateFiles(viewDirectory, "*.cshtml", SearchOption.AllDirectories))
            {
                GenerateCodeFile(fileName, "AspNet.View", tempPath);
            }
            var parseOptions = CSharpParseOptions.Default;
            parseOptions = parseOptions.WithPreprocessorSymbols("NETCORE");

            var syntaxTrees = Directory.EnumerateFiles(tempPath, "*.cs").Select((path) => CSharpSyntaxTree.ParseText(File.ReadAllText(path), parseOptions, path, Encoding.UTF8)).ToList();

            var references = new List<MetadataReference>();
            //references.Add(MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.0.1\mscorlib.dll"));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.CoreLib")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime.Extensions")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.CoreLib")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Threading.Tasks")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Dynamic.Runtime")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.ViewFeatures")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Razor")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.CSharp")).Location));
            references.Add(MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location));

            var compilation = CSharpCompilation.Create("AspNet.View")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTrees);
            var dllPath = Path.Combine(tempPath, "AspNet.View.dll");
            var emitResult = compilation.Emit(dllPath, dllPath + ".pdb");
            if (!emitResult.Success || emitResult.Diagnostics.Length > 0)
            {
                throw new Exception(string.Join("\r\n", emitResult.Diagnostics.ToList()));
            }
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
        }
        static Dictionary<int, Func<dynamic, dynamic>> dictionary = new Dictionary<int, Func<dynamic, dynamic>>();
        public static Func<dynamic, dynamic> GenerateDynamicProcess(string code)
        {
            var key = code.GetHashCode();
            if (!dictionary.ContainsKey(key))
            {
                #region Compilation
                var temp = @"
using System;
//using Bzway.Database.Core;
namespace Bzway.ETL
{
    public class Process
    {
        public Func<dynamic, dynamic> Convert => (throw new NotImplementedException());
    }
}";
                temp = temp.Replace("throw new NotImplementedException()", code);
                var parseOptions = CSharpParseOptions.Default
                    .WithPreprocessorSymbols("NETCORE")
                    .WithLanguageVersion(LanguageVersion.Latest);

                var syntaxTrees = CSharpSyntaxTree.ParseText(temp, parseOptions);
                var references = new List<MetadataReference>();
                //references.Add(MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.0.1\mscorlib.dll"));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Core")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.CoreLib")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Data")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Data.Common")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime.Extensions")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq.Expressions")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.CoreLib")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Threading.Tasks")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Dynamic.Runtime")).Location));
                //references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.ViewFeatures")).Location));
                //references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Razor")).Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.CSharp")).Location));
                //foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
                //{
                //    references.Add(MetadataReference.CreateFromFile(item.Location));
                //}
                references.Add(MetadataReference.CreateFromFile(@"D:\Work\DMS\src\Tools\ETL\bin\Debug\netcoreapp2.0\Bzway.Database.Core.dll"));
                references.Add(MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location));

                var compilation = CSharpCompilation.Create("AspNet.View")
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(references)
                    .AddSyntaxTrees(syntaxTrees);
                var dllPath = Path.Combine("d:\\", "AspNet.View.dll");
                var emitResult = compilation.Emit(dllPath, dllPath + ".pdb");
                if (!emitResult.Success || emitResult.Diagnostics.Length > 0)
                {
                    throw new Exception(string.Join("\r\n", emitResult.Diagnostics.ToList()));
                }
                var process = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath).CreateInstance("Bzway.ETL.Process");
                MethodInfo method = process.GetType().GetMethod("get_Convert");
                Func<dynamic, dynamic> convert = (Func<dynamic, dynamic>)method.Invoke(process, null);
                dictionary.Add(key, convert);

                #endregion
            }
            return dictionary[key];
        }

        private void GenerateCodeFile(string cshtmlFilePath, string rootNamespace, string tempPath)
        {
            //var basePath = Path.GetDirectoryName(cshtmlFilePath);
            //var fileName = Path.GetFileName(cshtmlFilePath);
            //var className = fileName.Replace(basePath, "").Replace(".", "_");
            //var codeLang = new CSharpRazorCodeLanguage();
            //var host = new RazorEngineHost(codeLang);
            //host.DefaultBaseClass = "Bzway.Wechat.MessageServer.DynamicView";
            //host.GeneratedClassContext = new GeneratedClassContext(
            //    executeMethodName: GeneratedClassContext.DefaultExecuteMethodName,
            //    writeMethodName: GeneratedClassContext.DefaultWriteMethodName,
            //    writeLiteralMethodName: GeneratedClassContext.DefaultWriteLiteralMethodName,
            //    writeToMethodName: "WriteTo",
            //    writeLiteralToMethodName: "WriteLiteralTo",
            //    templateTypeName: "HelperResult",
            //    defineSectionMethodName: "DefineSection",
            //    generatedTagHelperContext: new GeneratedTagHelperContext());
            //var engine = new RazorTemplateEngine(host);

            //var cshtmlContent = File.ReadAllText(cshtmlFilePath);
            //cshtmlContent = ProcessFileIncludes(basePath, cshtmlContent);

            //var generatorResults = engine.GenerateCode(
            //        input: new StringReader(cshtmlContent),
            //        className: className,
            //        rootNamespace: Path.GetFileName(rootNamespace),
            //        sourceFileName: fileName);

            //var generatedCode = generatorResults.GeneratedCode;

            //// Make the generated class 'internal' instead of 'public'
            ////generatedCode = generatedCode.Replace("public class", "internal class");

            //if (!Directory.Exists(tempPath))
            //{
            //    Directory.CreateDirectory(tempPath);
            //}
            //File.WriteAllText(Path.Combine(tempPath, string.Format("{0}.cs", className)), generatedCode);
        }

        private string ProcessFileIncludes(string basePath, string cshtmlContent)
        {
            var startMatch = "<%$ include: ";
            var endMatch = " %>";
            var startIndex = 0;
            while (startIndex < cshtmlContent.Length)
            {
                startIndex = cshtmlContent.IndexOf(startMatch, startIndex);
                if (startIndex == -1)
                {
                    break;
                }
                var endIndex = cshtmlContent.IndexOf(endMatch, startIndex);
                if (endIndex == -1)
                {
                    throw new InvalidOperationException("Invalid include file format. Usage example: <%$ include: ErrorPage.js %>");
                }
                var includeFileName = cshtmlContent.Substring(startIndex + startMatch.Length, endIndex - (startIndex + startMatch.Length));
                Console.WriteLine("      Inlining file {0}", includeFileName);
                var includeFileContent = File.ReadAllText(Path.Combine(basePath, includeFileName));
                cshtmlContent = cshtmlContent.Substring(0, startIndex) + includeFileContent + cshtmlContent.Substring(endIndex + endMatch.Length);
                startIndex = startIndex + includeFileContent.Length;
            }
            return cshtmlContent;
        }

        private List<string> LoadAssembliesFromUsings(IList<SyntaxTree> syntaxTrees)
        {
            List<string> assemblyLoader = new List<string>();
            foreach (var tree in syntaxTrees)
            {
                foreach (var usingSyntax in ((CompilationUnitSyntax)tree.GetRoot()).Usings)
                {
                    var name = usingSyntax.Name;
                    var names = new List<string>();
                    while (name != null)
                    {
                        // The type is "IdentifierNameSyntax" if it's single identifier
                        // eg: System
                        // The type is "QualifiedNameSyntax" if it's contains more than one identifier
                        // eg: System.Threading
                        if (name is QualifiedNameSyntax)
                        {
                            var qualifiedName = (QualifiedNameSyntax)name;
                            var identifierName = (IdentifierNameSyntax)qualifiedName.Right;
                            names.Add(identifierName.Identifier.Text);
                            name = qualifiedName.Left;
                        }
                        else if (name is IdentifierNameSyntax)
                        {
                            var identifierName = (IdentifierNameSyntax)name;
                            names.Add(identifierName.Identifier.Text);
                            name = null;
                        }
                    }
                    if (names.Contains("src"))
                    {
                        // Ignore if it looks like a namespace from plugin 
                        continue;
                    }
                    names.Reverse();

                    for (int c = 1; c <= names.Count; ++c)
                    {
                        // Try to load the namespace as assembly
                        // eg: will try "System" and "System.Threading" from "System.Threading"
                        var usingName = string.Join(".", names.Take(c));

                        assemblyLoader.Add(usingName);
                    }
                }
            }
            return assemblyLoader;
        }
    }
}