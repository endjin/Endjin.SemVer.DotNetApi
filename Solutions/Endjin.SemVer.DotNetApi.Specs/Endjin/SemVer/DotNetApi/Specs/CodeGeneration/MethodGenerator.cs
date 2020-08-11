// <copyright file="MethodGenerator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.CodeGeneration
{
    public class MethodGenerator : MethodGeneratorBase
    {
        public MethodGenerator(
            string name,
            string returnType,
            params (string name, string type)[] parameters)
            : base(parameters)
        {
            this.Name = name;
            this.ReturnType = returnType;
        }

        public string Name { get; }

        public string ReturnType { get; }
    }
}
