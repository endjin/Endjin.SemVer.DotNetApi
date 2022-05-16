// <copyright file="LibraryGenerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Emit;

    /// <summary>
    /// Represents a single generated library.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Tests use this to generate libraries with the characteristics they require, e.g., certain
    /// methods or properties being present on certain classes. By enabling such libraries to be
    /// generated problematically, we can define tests that verify our library comparison logic
    /// in many different ways without having to write and maintain by hand librararies that
    /// represent every possible kind of change we'd like to track.
    /// </para>
    /// </remarks>
    public class LibraryGenerator
    {
        private static readonly MetadataReference[] DefaultReferences;
        private static readonly CSharpCompilationOptions DefaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithOptimizationLevel(OptimizationLevel.Release);

        private readonly List<ClassGenerator> classesToInclude = new List<ClassGenerator>();
        private readonly string name;
        private readonly LibraryGeneratorSource source;

        static LibraryGenerator()
        {
            string pathToCoreTypesAssembly = typeof(object).Assembly.Location;
            DefaultReferences = new[]
            {
                MetadataReference.CreateFromFile(pathToCoreTypesAssembly),
            };
        }

        internal LibraryGenerator(string name, LibraryGeneratorSource source)
        {
            this.name = name;
            this.source = source;
        }

        public void AddClass(ClassGenerator classGenerator)
        {
            this.classesToInclude.Add(classGenerator);
        }

        public string Compile()
        {
            var compilation
                = CSharpCompilation.Create(
                    $"{this.name}.dll",
                    this.classesToInclude.Select(SyntaxTreeFromClass),
                    DefaultReferences,
                    DefaultCompilationOptions);

            string folder = this.source.GetFolderForLibraryGeneration(this.name);
            string dllPath = Path.Combine(folder, $"{this.name}.dll");
            EmitResult result = compilation.Emit(dllPath);

            if (!result.Success)
            {
                throw new InvalidOperationException($"Failed to compile {this.name}: {result}");
            }

            return dllPath;
        }

        private static SyntaxTree SyntaxTreeFromClass(ClassGenerator e)
        {
            int finalDotPosition = e.Name.LastIndexOf('.');
            string ns = finalDotPosition < 0 ? null : e.Name.Substring(0, finalDotPosition);
            string name = finalDotPosition < 0 ? e.Name : e.Name.Substring(finalDotPosition + 1);

            SyntaxToken publicModifier = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
            ClassDeclarationSyntax classDeclaration = SyntaxFactory
                .ClassDeclaration(name)
                .AddModifiers(publicModifier);

            foreach (ConstructorGenerator cg in e.Constructors)
            {
                ConstructorDeclarationSyntax ctor = SyntaxFactory
                    .ConstructorDeclaration(name)
                    .AddModifiers(publicModifier)
                    .WithBody(SyntaxFactory.Block());

                foreach ((string parameterName, string parameterType) in cg.Parameters)
                {
                    ParameterSyntax parameter = SyntaxFactory
                        .Parameter(SyntaxFactory.Identifier(parameterName))
                        .WithType(SyntaxFactory.ParseTypeName(parameterType));
                    ctor = ctor.AddParameterListParameters(parameter);
                }

                classDeclaration = classDeclaration.AddMembers(ctor);
            }

            foreach (PropertyGenerator pg in e.Properties)
            {
                TypeSyntax propertyType = SyntaxFactory.ParseTypeName(pg.Type);
                PropertyDeclarationSyntax property = SyntaxFactory
                    .PropertyDeclaration(propertyType, pg.Name)
                    .AddModifiers(publicModifier)
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration),
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration));

                classDeclaration = classDeclaration.AddMembers(property);
            }

            foreach (FieldGenerator fg in e.Fields)
            {
                TypeSyntax fieldType = SyntaxFactory.ParseTypeName(fg.Type);
                FieldDeclarationSyntax eventDecl = SyntaxFactory
                    .FieldDeclaration(SyntaxFactory.VariableDeclaration(fieldType).AddVariables(SyntaxFactory.VariableDeclarator(fg.Name)))
                    .AddModifiers(publicModifier);

                classDeclaration = classDeclaration.AddMembers(eventDecl);
            }

            foreach (EventGenerator pg in e.Events)
            {
                TypeSyntax eventType = SyntaxFactory.ParseTypeName(pg.Type);
                EventFieldDeclarationSyntax eventDecl = SyntaxFactory
                    .EventFieldDeclaration(SyntaxFactory.VariableDeclaration(eventType).AddVariables(SyntaxFactory.VariableDeclarator(pg.Name)))
                    .AddModifiers(publicModifier);

                classDeclaration = classDeclaration.AddMembers(eventDecl);
            }

            foreach (MethodGenerator mg in e.Methods)
            {
                TypeSyntax returnType = mg.ReturnType == TestMethodsReturnTypes.Void
                    ? SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword))
                    : SyntaxFactory.ParseTypeName(mg.ReturnType);
                MethodDeclarationSyntax method = SyntaxFactory
                    .MethodDeclaration(returnType, mg.Name)
                    .AddModifiers(publicModifier);

                foreach ((string parameterName, string parameterType) in mg.Parameters)
                {
                    ParameterSyntax parameter = SyntaxFactory
                        .Parameter(SyntaxFactory.Identifier(parameterName))
                        .WithType(SyntaxFactory.ParseTypeName(parameterType));
                    method = method.AddParameterListParameters(parameter);
                }

                BlockSyntax body = mg.ReturnType == TestMethodsReturnTypes.Void
                    ? SyntaxFactory.Block()
                    : SyntaxFactory.Block(SyntaxFactory.ReturnStatement(SyntaxFactory.DefaultExpression(returnType)));

                method = method.WithBody(body);

                classDeclaration = classDeclaration.AddMembers(method);
            }

            MemberDeclarationSyntax mainContent = ns == null
                ? (MemberDeclarationSyntax)classDeclaration
                : SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns)).AddMembers(classDeclaration);

            CompilationUnitSyntax compilationUnit = SyntaxFactory
                .CompilationUnit()
                .AddMembers(mainContent);

            return SyntaxFactory.SyntaxTree(compilationUnit);
        }
    }
}