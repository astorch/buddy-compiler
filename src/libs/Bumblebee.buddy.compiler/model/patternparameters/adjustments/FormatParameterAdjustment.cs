using System.Text.RegularExpressions;

namespace Bumblebee.buddy.compiler.model.patternparameters.adjustments {
    public class FormatParameterAdjustment : IParameterAdjustment {

        /// <inheritdoc />
        public string Id { get { return "format"; } }

        /// <inheritdoc />
        public bool Process(string instruction, out string processedInstruction, out string value) {
            value = null;
            processedInstruction = null;

            Match match = Regex.Match(instruction, "Form \"(?<value>[^\"]+)\"");
            if (!match.Success) return false;

            value = match.Groups["value"].Value;

            string part1 = instruction.Substring(0, match.Index);
            string part2 = instruction.Substring(match.Index + match.Length + 1);
            processedInstruction = string.Concat(part1, part2);
            
            return true;
        }
    }
}