using System;
using System.Collections;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.instructions.german;
using Bumblebee.buddy.compiler.model.patternparameters;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests {
    [TestFixture]
    public class InstructionFormatterTests {

        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.ThrowCases))]
        public void ToString_Throws(
            IBuddyTranslationInstruction translationInstruction, IDictionary<string, IPatternParameter> parameters,
            Type expectedType
        ) {
            Assert.Throws(expectedType, () => ToString(translationInstruction, parameters));
        }
        
        [TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCases))]
        public string ToString(IBuddyTranslationInstruction translationInstruction, IDictionary<string, IPatternParameter> parameters) {
            InstructionFormatter instructionFormatter = new InstructionFormatter();
            return instructionFormatter.ToString(translationInstruction, parameters);
        }

        class TestCaseFactory {
            public static IEnumerable TestCases {
                get {
                    // Assert
                    yield return new TestCaseData(new AssertBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "TextBox:Name"},
                        new TestParamInfo {Name = "condition", Type = "condition", Value = "sichtbar"},
                    })).SetName("assert IsVisible")
                       .Returns("assert(TextBox:Name, IsVisible, True)");

                    yield return new TestCaseData(new AssertBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "TextBox:Name"},
                        new TestParamInfo {Name = "condition", Type = "condition", Value = "klickbar"},
                    })).SetName("assert IsEnabled")
                       .Returns("assert(TextBox:Name, IsEnabled, True)");

                    yield return new TestCaseData(new AssertBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "TextBox:Name"},
                        new TestParamInfo {Name = "value", Type = "string", Value = "\"abcd\""},
                        new TestParamInfo {Name = "condition", Type = "condition", Value = "enthält"},
                    })).SetName("assert Text")
                       .Returns("assert(TextBox:Name, Text, \"abcd\")");

                    // Click
                    yield return new TestCaseData(new ClickBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "ButtonAnmelden"},
                        new TestParamInfo {Name = "kind", Type = "frequence", Value = "einfach"}
                    })).SetName("click single")
                       .Returns("click(ButtonAnmelden, , Single)");

                    yield return new TestCaseData(new ClickBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "Button:Abmelden"},
                        new TestParamInfo {Name = "kind", Type = "frequence", Value = "doppelt"}
                    })).SetName("click double")
                       .Returns("click(Button:Abmelden, , Double)");

                    yield return new TestCaseData(new ClickBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "Button:Abmelden"},
                        new TestParamInfo {Name = "kind", Type = "frequence", Value = PatternParameter.Unset}
                    })).SetName("click unset")
                       .Returns("click(Button:Abmelden, , Single)");

                    // Close
                    yield return new TestCaseData(new CloseBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "ref", Type = "keyword", Value = "Anwendung"}
                    })).SetName("close default")
                       .Returns("close(_Application, , Default)");

                    // Select
                    yield return new TestCaseData(new SelectBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "ListboxPinr"},
                        new TestParamInfo {Name = "value", Type = "param", Value = "\"000\""}
                    })).SetName("select Value text")
                       .Returns("select(ListboxPinr, Value, \"000\")");

                    yield return new TestCaseData(new SelectBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "ListboxBuero"},
                        new TestParamInfo {Name = "value", Type = "param", Value = "$buero"}
                    })).SetName("select Value parameter")
                       .Returns("select(ListboxBuero, Value, buero)");

                    // Set
                    yield return new TestCaseData(new SetBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "TextboxPass"},
                        new TestParamInfo {Name = "value", Type = "string", Value = "\"Passw001\""}
                    })).SetName("set Text text")
                       .Returns("set(TextboxPass, Text, \"Passw001\")");

                    yield return new TestCaseData(new SetBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "TextboxName"},
                        new TestParamInfo {Name = "value", Type = "param", Value = "$name"}
                    })).SetName("set Text parameter")
                       .Returns("set(TextboxName, Text, name)");

                    // Start
                    yield return new TestCaseData(new StartBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "path", Type = "string", Value = "\"C:\\EGUB\\EGUB.exe\""}
                    })).SetName("start with path")
                       .Returns("processHandle1 = start(,, \"C:\\EGUB\\EGUB.exe\")");
                    
                    // Switch
                    yield return new TestCaseData(new SwitchBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "path", Type = "string", Value = "\"C:\\EGUB\\HIT.exe\""}
                    })).SetName("Switch with path")
                        .Returns("setsut(,, \"C:\\EGUB\\HIT.exe\")");
                    
                    yield return new TestCaseData(new SwitchBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                            new TestParamInfo {Name = "path", Type = "string", Value = "\"HIT\""}
                        })).SetName("Switch with constant")
                        .Returns("setsut(,, \"HIT\")");

                    // Wait
                    yield return new TestCaseData(new WaitBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "WindowAnmeldung"},
                        new TestParamInfo {Name = "condition", Type = "condition", Value = "sichtbar"}
                    })).SetName("wait IsVisible")
                       .Returns("wait(WindowAnmeldung, IsVisible, True, 60000)");

                    yield return new TestCaseData(new WaitBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "WindowAnmeldung"},
                        new TestParamInfo {Name = "condition", Type = "condition", Value = "verschwunden"}
                    })).SetName("wait IsVisible not")
                       .Returns("wait(WindowAnmeldung, IsVisible, False, 60000)");

                    yield return new TestCaseData(new WaitBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "alias", Type = "alias", Value = "ButtonLogin"},
                        new TestParamInfo {Name = "condition", Type = "condition", Value = "klickbar"}
                    })).SetName("wait IsEnabled")
                       .Returns("wait(ButtonLogin, IsEnabled, True, 60000)");

                    // Execute
                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Anmeldung"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = PatternParameter.Unset}
                    })).SetName("gosub simple name")
                       .Returns("gosub Anmeldung:");

                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Meine Daten bearbeiten"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = PatternParameter.Unset}
                    })).SetName("gosub name with spaces")
                       .Returns("gosub Meine_Daten_bearbeiten:");

                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Daten zurücksetzen"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = "(\"Administrator\")"}
                    })).SetName("gosub parameter single text")
                       .Returns("gosub Daten_zurücksetzen:(\"Administrator\")");

                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Daten zurücksetzen"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = "(\"0815\", \"-Ohne-\")"}
                    })).SetName("gosub parameter double text")
                       .Returns("gosub Daten_zurücksetzen:(\"0815\", \"-Ohne-\")");

                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Daten zurücksetzen"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = "($group)"}
                    })).SetName("gosub parameter single value reference")
                       .Returns("gosub Daten_zurücksetzen:(group)");

                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Daten zurücksetzen"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = "($group, $type)"}
                    })).SetName("gosub parameter double value reference")
                       .Returns("gosub Daten_zurücksetzen:(group, type)");

                    yield return new TestCaseData(new ExecuteBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "unitName", Type = "unitReference", Value = "Daten zurücksetzen"},
                        new TestParamInfo {Name = "parameterSet", Type = "parameterSet", Value = "($group, \"-Ohne-\")"}
                    })).SetName("gosub parameter double mixed mode")
                       .Returns("gosub Daten_zurücksetzen:(group, \"-Ohne-\")");

                    // Press
                    yield return new TestCaseData(new PressBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "keyName", Type = "key", Value = "Tab"},
                        new TestParamInfo {Name = "kind", Type = "frequence", Value = "einfach"}
                    })).SetName("press key single, simple")
                       .Returns("pressKey(, KeyType, \"TAB\", Single)");

                    yield return new TestCaseData(new PressBuddyTranslationInstruction(), MapToParameters(new List<TestParamInfo> {
                        new TestParamInfo {Name = "keyName", Type = "key", Value = "Strg+Shift+S"},
                        new TestParamInfo {Name = "kind", Type = "frequence", Value = "doppelt"}
                    })).SetName("press key double, combined")
                       .Returns("pressKey(, KeyType, \"STRG+SHIFT+S\", Double)");
                }
            }

            public static IEnumerable ThrowCases {
                get {
                    yield return new TestCaseData(null, null, typeof(ArgumentNullException));
                    yield return new TestCaseData(new WaitBuddyTranslationInstruction(), null, typeof(ArgumentNullException));
                }
            }

            private static IDictionary<string, IPatternParameter> MapToParameters(IList<TestParamInfo> parameters) {
                Dictionary<string, IPatternParameter> resultSet = new Dictionary<string, IPatternParameter>();
                for (int i = -1; ++i != parameters.Count;) {
                    TestParamInfo param = parameters[i];

                    AbstractPatternParameter patternParameter = new PatternParameterFactory().CreatePatternParameter(param.Name, param.Type);
                    patternParameter.SetValue(param.Value);
                    resultSet.Add(param.Name, patternParameter);
                }
                return resultSet;
            }

            class TestParamInfo {
                public string Name { get; set; }
                public string Type { get; set; }
                public string Value { get; set; }
            }
        }
    }
}