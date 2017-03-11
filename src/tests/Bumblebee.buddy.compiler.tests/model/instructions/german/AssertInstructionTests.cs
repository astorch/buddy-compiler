using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;
using Bumblebee.buddy.compiler.model.evaluation;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class AssertInstructionTests {

        [Test, TestCaseSource(typeof(TestCaseFactory), "TestCases")]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new AssertBuddyTranslationInstruction()),
                new Dictionary<string, IPatternParameter>());
        }

        class TestCaseFactory {
            public static IEnumerable<TestCaseData> TestCases {
                get {
                    yield return new TestCaseData("Pr�fe <ButtonAnmelden> ausw�hlbar.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Pr�fe <Button:Anmelden> sichtbar.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Pr�fe <TextBox:Passwort> \"abc\" enth�lt.").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData((string)null).Throws(typeof(ArgumentNullException));
                    yield return new TestCaseData(string.Empty).Throws(typeof(ArgumentNullException));

                    yield return new TestCaseData("Pr�fe ob <ButtonAnmelden> ausw�hlbar.").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token '<'. Word is 'ob'"));
                }
            }
        }
    }
}