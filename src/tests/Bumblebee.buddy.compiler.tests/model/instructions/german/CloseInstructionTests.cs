using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class CloseInstructionTests {

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCases")]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new CloseBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {
            public IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Schließe Fenster.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Schließe Anwendung.").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData((string) null).Throws(typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty).Throws(typeof(ArgumentNullException));

                    yield return new TestCaseData("Schließe ").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token 'Parameter reference 'ref' (keyword)'. Word is '⌜'"));
                    yield return new TestCaseData("Schließe <Anwendung.").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token 'Parameter reference 'ref' (keyword)'. Word is '<'"));
                }
            }
        }
    }
}