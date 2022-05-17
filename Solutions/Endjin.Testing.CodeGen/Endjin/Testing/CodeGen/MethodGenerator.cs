// <copyright file="MethodGenerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    public class MethodGenerator : MethodGeneratorBase
    {
        public MethodGenerator(
            string name,
            string returnType,
            params (string Name, string Type)[] parameters)
            : base(parameters)
        {
            this.Name = name;
            this.ReturnType = returnType;
        }

        public string Name { get; }

        public string ReturnType { get; }
    }
}