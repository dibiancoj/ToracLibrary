using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.SessionState
{

    /// <summary>
    /// Holds the result of the RetrieveSessionDataAcrossUsers
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    public class RetrieveSessionDataAcrossUsersResult
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SessionVariableNameToSet">Holds the session variable name. Ie Session[SessionVariableName]</param>
        /// <param name="SessionVariableValueToSet">Holds the actual value of the session variable</param>
        public RetrieveSessionDataAcrossUsersResult(string SessionVariableNameToSet, object SessionVariableValueToSet)
        {
            //set the session name
            SessionVariableName = SessionVariableNameToSet;

            //set the session variable value
            SessionVariableValue = SessionVariableValueToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the session variable name. Ie Session[SessionVariableName]
        /// </summary>
        public string SessionVariableName { get; }

        /// <summary>
        /// Holds the actual value of the session variable
        /// </summary>
        public object SessionVariableValue { get; }

        #endregion

    }

}
