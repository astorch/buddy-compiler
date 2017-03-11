namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Declares that the instruction may occur in variant forms. So the translation offers to normalize it.
    /// </summary>
    public interface IVariantInstruction : IBuddyTranslationInstruction {
        /// <summary>
        /// Returns the normalized word sequence for given <paramref name="words"/>.
        /// </summary>
        /// <param name="words">Word sequence to normalize</param>
        /// <returns>Normalized word sequence</returns>
        string[] Normalize(string[] words);
    }
}