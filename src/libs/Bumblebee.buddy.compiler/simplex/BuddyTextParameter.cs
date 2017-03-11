using System;

namespace Bumblebee.buddy.compiler.simplex {
    /// <summary>
    /// Defines a buddy text parameter.
    /// </summary>
    public class BuddyTextParameter {
        /// <summary>
        /// Creates a new instance with the given arguments. The argument <paramref name="name"/> must not be NULL!
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="defaultValue">Declared default value of the parameter</param>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is NULL</exception>
        public BuddyTextParameter(string name, string defaultValue) {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            Name = name;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Returns the name of the parameter or does set it.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the declared default value of the parameter or does set it.
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            BuddyTextParameter nstc = obj as BuddyTextParameter;
            if (nstc == null) return false;

            if (!Equals(Name, nstc.Name)) return false;
            if (!Equals(DefaultValue, nstc.DefaultValue)) return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            int result = 31;

            result = 17*result + Name.GetHashCode();
            result = 17*result + (DefaultValue == null ? 0 : DefaultValue.GetHashCode());
            
            return result;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() {
            string result = string.Format("{0} Name: '{1}', DefaultValue: '{2}'", GetType().Name, Name, DefaultValue);
            return result;
        }
    }
}