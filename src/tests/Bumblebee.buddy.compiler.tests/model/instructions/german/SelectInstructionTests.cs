using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class SelectInstructionTests {

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCases")]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new SelectBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {
            public static IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Wähle $piNr <Listbox:Pinr>.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Wähle \"001\" <Listbox:Pinr>.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Wähle <Listbox:Pinr> \"801\".").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData((string)null).Throws(typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty).Throws(typeof(ArgumentNullException));

                    yield return new TestCaseData(@"Wähle ""000"" in <ListboxPinr> aus.").Returns(EvaluationResult.Ok); // XXX Suspect
                }
            }
        }
    }
}