﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MvcAuthorize.Tests.Mocks
{
	/// <summary>
	/// Used for testing. When the action is executed the Result type should be match 
	/// with the <see cref="TResult"/> generic parameter.
	/// </summary>
	/// <typeparam name="TResult">Expected Result type.</typeparam>
    /// <remarks>Via http://darioquintana.com.ar/blogging/2009/05/23/aspnet-mvc-testing-a-custom-authorize-filters/</remarks>
	public class ActionInvokerExpecter<TResult> : ControllerActionInvoker where TResult : ActionResult
	{
		/// <summary>
		/// The point here is just to invoke que Action and do nothing with the result.
		/// </summary>
		/// <param name="controllerContext"></param>
		/// <param name="actionResult"></param>
		protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
		{
			Assert.IsInstanceOfType(actionResult, typeof (TResult));
		}
	}
}