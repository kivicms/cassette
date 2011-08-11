﻿using System.IO;
using Cassette.CoffeeScript;
using Moq;
using Should;
using Xunit;
using Cassette.Utilities;

namespace Cassette.ModuleProcessing
{
    public class CompileCoffeeScriptAsset_Tests
    {
        [Fact]
        public void TransformCallsCoffeeScriptCompiler()
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.SourceFilename).Returns("test.coffee");

            var sourceInput = "source-input";
            var compilerOutput = "compiler-output";
            var compiler = StubCompiler(sourceInput, compilerOutput);

            var transformer = new CompileCoffeeScriptAsset(compiler);

            var getResultStream = transformer.Transform(
                () => sourceInput.AsStream(),
                asset.Object
            );

            using (var reader = new StreamReader(getResultStream()))
            {
                reader.ReadToEnd().ShouldEqual(compilerOutput);
            }
        }

        ICoffeeScriptCompiler StubCompiler(string expectedSourceInput, string compilerOutput)
        {
            var compiler = new Mock<ICoffeeScriptCompiler>();
            compiler.Setup(c => c.Compile(expectedSourceInput, "test.coffee"))
                    .Returns(compilerOutput);
            return compiler.Object;
        }
    }
}