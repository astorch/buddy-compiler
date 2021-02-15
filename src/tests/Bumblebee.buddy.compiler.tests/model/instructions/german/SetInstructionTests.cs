using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class SetInstructionTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void Evaluate_Throws(string instruction, Type expectedType) {
            Assert.Throws(expectedType, () => Evaluate(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new SetBuddyTranslationInstruction()),
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
                    yield return new TestCaseData("Setze $pass <TextboxPass>.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Setze \"123\" <TextboxPass>.").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData("Setze $pass").Returns(new _ErrorEvaluationResult("3rd word of instruction does not match expected token '<'. Word is '⌜'"));
                    yield return new TestCaseData("Setze $pass in rein.").Returns(new _ErrorEvaluationResult("3rd word of instruction does not match expected token '<'. Word is 'in'"));
                }
            }
        }
    }
}