// <copyright file="ConstructorGenerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    public class ConstructorGenerator : MethodGeneratorBase
    {
        public ConstructorGenerator((string Name, string Type)[] parameters)
            : base(parameters)
        {
        }
    }
}