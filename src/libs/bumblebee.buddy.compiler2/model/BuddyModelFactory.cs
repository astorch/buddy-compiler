using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace bumblebee.buddy.compiler2.model {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BuddyModelFactory {
        private readonly StringBuilder iStringBuilder = new StringBuilder(120);

        private string Compose(object obj1, object[] objects) {
            iStringBuilder.Clear();
            iStringBuilder.Append(obj1.ToString());
            Array.ForEach(objects, _ => iStringBuilder.Append(_));
            return iStringBuilder.ToString();
        }

        private bool IsEmptySequence(object[] seqElements) {
            if (seqElements == null) return true;
            if (seqElements.Length == 0) return true;
            if (Array.TrueForAll(seqElements, item => item == null)) return true;
            return false;
        }

        private object Compose<TElement>(object e1, object[] set) where TElement : class {
            if (IsEmptySequence(set)) return new[] {(TElement) e1};
            TElement[] resultSet = new TElement[set.Length + 1];
            resultSet[0] = (TElement) e1;
            Array.Copy(set, 0, resultSet, 1, set.Length);
            return resultSet;
        }

        public object linefeed(object cr, object nl) {
            return "\r\n";
        }

        public object string_char_seq(object chr, object[] chrSeq) {
            return Compose(chr, chrSeq);
        }

        public object string_content(object dquote1, object content, object dquote2) {
            return iStringBuilder.Clear().Append('"').Append(content.ToString()).Append('"').ToString();
        }

        public object string_empty(object dquote1, object dquote2) {
            return "\"\"";
        }

        public object reference_parameter(object dlr, object parameterName) {
            return new ParameterReference(parameterName.ToString());
        }

        public object reference_alias(object lab, object aliasName, object rab) {
            return new AliasReference(aliasName.ToString());
        }

        public object reference_string(object str) {
            return new StringReference(str.ToString());
        }

        public object reference_string_seq_link(object comma, object ws, object refStr) {
            return new StringReference(refStr.ToString());
        }

        public object reference_string_seq(object str, object[] seqItems) {
            return Compose<StringReference>(new StringReference(str.ToString()), seqItems);
        }

        public object reference_subselect(object prefix, object ws, object seqItems) {
            return new SubselectReference(null) {References =  (AbstractReference[]) seqItems};
        }

        public object reference_unit(object lsb, object name, object rsb) {
            return new UnitInvocationReference(name.ToString());
        }

        public object reference_unit_parameter_assign(object name, object ws1, object eq, object ws2, object value) {
            return new UnitInvocationParameter {Name = name.ToString(), Value = value};
        }

        public object reference_unit_parameter_seq_link(object comma, object ws, object assign) {
            return assign;
        }

        public object reference_unit_parameter_seq(object assign, object[] assignSeq) {
            return Compose<UnitInvocationParameter>(assign, assignSeq);
        }

        public object reference_unit_parameters(object prefix, object ws, object seqItems) {
            return new UnitInvocationParameterReference(null) {Parameters = (UnitInvocationParameter[]) seqItems};
        }

        public object name(object letter, object[] letterSeq) {
            return Compose(letter, letterSeq);
        }

        public object word(object letter, object[] letterSeq) {
            return Compose(letter, letterSeq);
        }

        public object word_link(object succ, object word) {
            return iStringBuilder.Clear().Append(succ.ToString()).Append(word.ToString()).ToString();
        }

        public object word_seq(object word, object[] words) {
            return Compose(word, words);
        }

        public object alias_name_fragment(object letter, object[] chrs) {
            return Compose(letter, chrs);
        }

        public object alias_name(object fragment1, object colon, object fragment2) {
            return iStringBuilder.Clear().Append(fragment1.ToString()).Append(':').Append(fragment2).ToString();
        }

        public object parameter_name(object letter, object[] letterSeq) {
            return Compose(letter, letterSeq);
        }

        public object parameter_assign(object name, object ws1, object eq, object ws2, object str) {
            return new ScenarioParameter {Name = name.ToString(), Value = str.ToString()};
        }

        public object parameter_assign_link(object comma, object ws, object paramAssign) {
            return paramAssign;
        }

        public object parameter_assign_seq(object paramAssign, object ws, object[] paramAssignSeq) {
            return Compose<ScenarioParameter>(paramAssign, paramAssignSeq);
        }

        public object scenario_name(object letter, object[] letterSeq) {
            return Compose(letter, letterSeq);
        }

        public object scenario_parameter_seq(object lbr, object paramSeq, object rbr) {
            return paramSeq;
        }

        public object directive_word(object word) {
            if (word is AbstractReference) return word;

            string str = word.ToString();
            if (str[0] == '"') return new StringReference(str);
            
            return new WordReference(word.ToString());
        }

        public object directive_word_successor(object ws, object word) {
            return word;
        }

        public object directive_word_seq(object word, object[] words) {
            return Compose<AbstractReference>(word, words);
        }

        public object directive(object sentence, object dot) {
            return new DirectiveContext {References = (AbstractReference[]) sentence};
        }

        public object directive_link(object lf, object sentence) {
            return sentence;
        }

        public object directive_seq(object directive, object[] directiveSeq) {
            return Compose<DirectiveContext>(directive, directiveSeq);
        }

        public object steps(object prefix, object lf, object directiveSet) {
            return new StepsContext {Directives = (DirectiveContext[]) directiveSet};
        }

        public object import_name_fragment(object letter, object[] letterSeq) {
            return Compose(letter, letterSeq);
        }

        public object import_name(object name1, object dot1, object name2, object dot2,
            object name3, object dot3, object name4) {
            return iStringBuilder.Clear()
                .Append(name1.ToString()).Append('.')
                .Append(name2.ToString()).Append('.')
                .Append(name3.ToString()).Append('.')
                .Append(name4.ToString())
                .ToString();
        }

        public object import_statement(object prefix, object ws, object name, object lf) {
            return new Import {Name = name.ToString()};
        }

        public object version_fragment(object digit, object dot, object[] digitSeq) {
            iStringBuilder.Clear();
            Array.ForEach(digitSeq, _ => iStringBuilder.Append(_));
            string trailingDigits = iStringBuilder.ToString();
            return iStringBuilder.Clear()
                .Append(digit.ToString())
                .Append('.')
                .Append(trailingDigits)
                .ToString();
        }

        public object version_single(object value) {
            return value;
        }

        public object version_link(object minus, object version) {
            return version;
        }

        public object version_range(object single, object extension) {
            return new Version(single.ToString(), (extension ?? single).ToString());
        }

        public object imports(object import, object[] importSeq) {
            return Compose<Import>(import, importSeq);
        }

        public object scenario(object prefix, object ws, object name, object paramSet, object linefeed) {
            return new ScenarioContext {
                Name = name.ToString(),
                ScenarioParameters = (ScenarioParameter[]) paramSet
            };
        }

        public object usecase(object prefix, object ws, object name, object linefeed) {
            return new UsecaseContext {Name = name.ToString()};
        }

        public object version(object lbr, object versionAssign, object rbr) {
            return versionAssign;
        }

        public object version_intro(object ws, object version) {
            return version;
        }

        public object application(object prefix, object ws, object name, object version, object linefeed) {
            return new ApplicationContext {Name = name.ToString(), Version = (Version) version};
        }

        public object testcase(object application, object usecase, object scenario, object imports, object steps) {
            return new TestcaseContext {
                Application = (ApplicationContext) application,
                Usecase = (UsecaseContext) usecase,
                Scenario =  (ScenarioContext) scenario,
                Imports = (Import[]) imports,
                Steps =  (StepsContext) steps
            };
        }

        // http://www.fileformat.info/info/unicode/category/Lm/list.htm
    }
}