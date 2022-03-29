﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TSharp.Core.Util.TypeConvert;
using System.Text;
using System.IO;

namespace TSharp.Core
{
    /// <summary>
    /// Class Context
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        IApplicationState Application { get; }
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        ISessionState Session { get; }
        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        IRequestState Request { get; }
    }
}
