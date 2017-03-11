using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class SetInstructionTests {
        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCases")]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new SetBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {
            public IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Setze $pass <TextboxPass>.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Setze \"123\" <TextboxPass>.").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData((string)null).Throws(typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty).Throws(typeof(ArgumentNullException));

                    yield return new TestCaseData("Setze $pass").Returns(new _ErrorEvaluationResult("3rd word of instruction does not match expected token '<'. Word is '⌜'"));
                    yield return new TestCaseData("Setze $pass in rein.").Returns(new _ErrorEvaluationResult("3rd word of instruction does not match expected token '<'. Word is 'in'"));
                }
            }
        }
    }
}