// <copyright file="ConstructorGenerator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.CodeGeneration
{
    public class ConstructorGenerator : MethodGeneratorBase
    {
        public ConstructorGenerator((string name, string type)[] parameters)
            : base(parameters)
        {
        }
    }
}
