using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class SelectInstructionTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void Evaluate_Throws(string instruction, Type expectedType) {
            Assert.Throws(expectedType, () => Evaluate(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new SelectBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {

            public static IEnumerable<TestCaseData> ThrowCases {
                get {
                    yield return new TestCaseData((string) null, typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty, typeof(ArgumentNullException));
                }
            }
            
            public static IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Wähle $piNr <Listbox:Pinr>.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Wähle \"001\" <Listbox:Pinr>.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Wähle <Listbox:Pinr> \"801\".").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData(@"Wähle ""000"" in <ListboxPinr> aus.").Returns(EvaluationResult.Ok); // XXX Suspect
                }
            }
        }
        
    }
}