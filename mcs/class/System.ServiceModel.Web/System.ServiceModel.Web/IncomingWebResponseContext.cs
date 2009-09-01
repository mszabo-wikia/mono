//
// IncomingWebResponseContext.cs
//
// Author:
//	Atsushi Enomoto  <atsushi@ximian.com>
//
// Copyright (C) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Globalization;
using System.Net;
using System.ServiceModel.Channels;

namespace System.ServiceModel.Web
{
	public class IncomingWebResponseContext
	{
		HttpResponseMessageProperty hp;

		internal IncomingWebResponseContext (OperationContext context)
		{
			if (context.IncomingMessageProperties != null)
				hp = (HttpResponseMessageProperty) context.IncomingMessageProperties [HttpResponseMessageProperty.Name];
			else
				hp = new HttpResponseMessageProperty ();
		}

		public long ContentLength {
			get {
				string s = hp.Headers.Get ("Content-Length");
				return s != null ? long.Parse (s, CultureInfo.InvariantCulture) : 0;
			}
		}

		public string ContentType {
			get { return hp.Headers.Get ("Content-Type"); }
		}

		public string ETag {
			get { return hp.Headers.Get ("ETag"); }
		}

		public WebHeaderCollection Headers {
			get { return hp.Headers; }
		}

		public string Location {
			get { return hp.Headers.Get ("Location"); }
		}

		public HttpStatusCode StatusCode {
			get { return hp.StatusCode; }
		}

		public string StatusDescription {
			get { return hp.StatusDescription; }
		}
	}
}
