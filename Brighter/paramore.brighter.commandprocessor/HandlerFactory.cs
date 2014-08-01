// ***********************************************************************
// Assembly         : paramore.brighter.commandprocessor
// Author           : ian
// Created          : 07-01-2014
//
// Last Modified By : ian
// Last Modified On : 07-10-2014
// ***********************************************************************
// <copyright file="HandlerFactory.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Licence
/* The MIT License (MIT)
Copyright � 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the �Software�), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */
#endregion

using System;
using System.Linq;

/// <summary>
/// The commandprocessor namespace.{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
/// </summary>
namespace paramore.brighter.commandprocessor
{
    /// <summary>
    /// Class HandlerFactory.{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    internal class HandlerFactory<TRequest> where TRequest : class, IRequest
    {
        private readonly RequestHandlerAttribute attribute;
        private readonly IAmAHandlerFactory factory;
        private readonly Type messageType;
        private IRequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerFactory{TRequest}"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="requestContext">The request context.</param>
        public HandlerFactory(RequestHandlerAttribute attribute, IAmAHandlerFactory factory, IRequestContext requestContext)
        {
            this.attribute = attribute;
            this.factory = factory;
            this.requestContext = requestContext;
            messageType = typeof(TRequest);
        }

        /// <summary>
        /// Creates the request handler.
        /// </summary>
        /// <returns>IHandleRequests&lt;TRequest&gt;.</returns>
        public IHandleRequests<TRequest> CreateRequestHandler()
        {
            var handlerType = attribute.GetHandlerType().MakeGenericType(messageType);
            var handler = (IHandleRequests<TRequest>) factory.Create(handlerType);
            //Lod the context befor the initializer - in case we want to use the context from within the initializer
            handler.Context = requestContext;
            handler.InitializeFromAttributeParams(attribute.InitializerParams());
            return handler;
        }
    }
}