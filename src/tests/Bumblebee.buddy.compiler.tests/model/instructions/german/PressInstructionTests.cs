﻿using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.model.instructions.german {
    [TestFixture]
    public class PressInstructionTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void Evaluate_Throws(string instruction, Type expectedType) {
            Assert.Throws(expectedType, () => Evaluate(instruction));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public IInstructionEvaluationResult Evaluate(string instruction) {
            return new InstructionEvaluator().Evaluate(instruction,
                new InstructionTranslationInfo(new PressBuddyTranslationInstruction()),
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
                    yield return new TestCaseData("Drücke Tab zweimal.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Drücke Enter.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Drücke Alt+F einfach.").Returns(EvaluationResult.Ok);
                    yield return new TestCaseData("Drücke STRG+SHIFT+S.").Returns(EvaluationResult.Ok);

                    yield return new TestCaseData("Haue Enter.").Returns(new _ErrorEvaluationResult("1st word of instruction does not match expected token 'Drücke'. Word is 'Haue'"));
                    yield return new TestCaseData("Drücke <ListBox> Enter.").Returns(new _ErrorEvaluationResult("2nd word of instruction does not match expected token 'Parameter reference 'keyName' (key)'. Word is '<'"));
                }
            }
            
        }
    }
}