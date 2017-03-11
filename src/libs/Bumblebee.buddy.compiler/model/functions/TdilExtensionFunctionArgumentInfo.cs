using System;

namespace Bumblebee.buddy.compiler.model.functions {
    /// <summary>
    /// Describes an invocation argument of a TDIL extension function.
    /// </summary>
    public class TdilExtensionFunctionArgumentInfo {
        /// <summary>
        /// Creates a new instance with the given values.
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <param name="defaultValue">Value of the argument</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public TdilExtensionFunctionArgumentInfo(string name, string defaultValue)
            : this(name, defaultValue, null) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given values.
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <param name="defaultValue">Value of the argument</param>
        /// <param name="adjustment">Argument adjustment id</param>
        public TdilExtensionFunctionArgumentInfo(string name, string defaultValue, string adjustment) {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name", "Must not be NULL or empty");
            if (defaultValue == null) throw new ArgumentNullException("defaultValue", "Must not be NULL");
            
            Name = name;
            DefaultValue = defaultValue;
            Adjustment = adjustment;
        }

        /// <summary>
        /// Returns the name of the argument.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the default value.
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Returns the adjustment.
        /// </summary>
        public string Adjustment { get; private set; }
    }
}