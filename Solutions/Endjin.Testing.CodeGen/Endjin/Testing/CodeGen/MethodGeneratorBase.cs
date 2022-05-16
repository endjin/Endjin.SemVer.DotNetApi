// <copyright file="MethodGeneratorBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    public abstract class MethodGeneratorBase
    {
        protected MethodGeneratorBase(params (string Name, string Type)[] parameters)
        {
            this.Parameters = parameters;
        }

        public (string Name, string Type)[] Parameters { get; }
    }
}
