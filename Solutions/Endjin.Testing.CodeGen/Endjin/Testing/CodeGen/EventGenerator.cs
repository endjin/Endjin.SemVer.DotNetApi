// <copyright file="EventGenerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    public class EventGenerator
    {
        public EventGenerator(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; }

        public string Type { get; }
    }
}