using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines one or more roles that a user can have in the sync engine
    /// </summary>
    [Flags]
    public enum SyncServiceRole
    {
        /// <summary>
        /// No roles
        /// </summary>
        None = 0,

        /// <summary>
        /// The administrator role
        /// </summary>
        Administrator = 1,

        /// <summary>
        /// The operator role
        /// </summary>
        Operator = 2,

        /// <summary>
        /// The joiner role
        /// </summary>
        Joiner = 4,

        /// <summary>
        /// The password set and change role
        /// </summary>
        PasswordSetAndChange = 8,

        /// <summary>
        /// The browser role
        /// </summary>
        Browser = 16,
    }
}
