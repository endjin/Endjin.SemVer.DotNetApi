// <copyright file="LibraryGeneratorSource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Testing.CodeGen
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Generates libraries to test the LibraryComparison class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This creates a temporary folder for all generated libraries to live in, which it deletes
    /// when disposed.
    /// </para>
    /// </remarks>
    public class LibraryGeneratorSource : IDisposable
    {
        private readonly Dictionary<string, ClassGenerator> classGenerators = new Dictionary<string, ClassGenerator>();
        private readonly string libGenPath;

        private int libFolderLastId;

        public LibraryGeneratorSource()
        {
            this.libGenPath = Path.Combine(Path.GetTempPath(), @"endjin\apicompare\codegen\" + Guid.NewGuid());
        }

        public IReadOnlyDictionary<string, ClassGenerator> ClassGenerators => this.classGenerators;

        public LibraryGenerator AddLibrary(string name)
        {
            return new LibraryGenerator(name, this);
        }

        public ClassGenerator AddClassGenerator(string classGeneratorId, string className)
        {
            var cg = new ClassGenerator(className);
            this.classGenerators.Add(classGeneratorId, cg);
            return cg;
        }

        public void Dispose()
        {
            if (Directory.Exists(this.libGenPath))
            {
                Directory.Delete(this.libGenPath, true);
            }
        }

        internal string GetFolderForLibraryGeneration(string name)
        {
            int folderId = ++this.libFolderLastId;
            string path = Path.Combine(this.libGenPath, $"{name}-{folderId}");
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
