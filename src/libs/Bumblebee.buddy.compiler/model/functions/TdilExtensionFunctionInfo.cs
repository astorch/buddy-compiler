using System;

namespace Bumblebee.buddy.compiler.model.functions {
    /// <summary> Describes a TDIL extension function. </summary>
    public class TdilExtensionFunctionInfo {
        
        /// <summary>
        /// Creates a new instance with the given arguments. Using this constructor indicates that 
        /// the function does not have arguments.
        /// </summary>
        /// <param name="regularExpression">Regular expression describing the function</param>
        /// <param name="resultFormat">Resulting TDIL function invocation format</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL or empty</exception>
        public TdilExtensionFunctionInfo(string regularExpression, string resultFormat) 
            : this(regularExpression, resultFormat, Array.Empty<TdilExtensionFunctionArgumentInfo>()) {
            // Nothing to do here
        }
        
        /// <summary>
        /// Creates a new instance with the given arguments.
        /// </summary>
        /// <param name="regularExpression">Regular expression describing the function</param>
        /// <param name="resultFormat">Resulting TDIL function invocation format</param>
        /// <param name="arguments">Set invocation arguments</param>
        public TdilExtensionFunctionInfo(string regularExpression, string resultFormat, TdilExtensionFunctionArgumentInfo[] arguments) {
            if (string.IsNullOrEmpty(regularExpression)) throw new ArgumentNullException(nameof(regularExpression), "Must not be NULL or empty");
            if (string.IsNullOrEmpty(resultFormat)) throw new ArgumentNullException(nameof(resultFormat), "Must not be NULL or empty");
            
            RegularExpression = regularExpression;
            ResultFormat = resultFormat;
            Arguments = arguments;
        }

        /// <summary>
        /// Returns the name of the function or does set it.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the description of the function or does set it.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns the regular expression of the function.
        /// </summary>
        public string RegularExpression { get; }

        /// <summary>
        /// Returns the resulting TDIL function invocation format.
        /// </summary>
        public string ResultFormat { get; }

        /// <summary>
        /// Returns the invocation arguments.
        /// </summary>
        public TdilExtensionFunctionArgumentInfo[] Arguments { get; }
        
    }
}