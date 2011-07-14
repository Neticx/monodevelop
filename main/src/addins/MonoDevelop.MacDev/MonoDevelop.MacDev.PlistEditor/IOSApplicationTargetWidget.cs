// 
// IOSApplicationTargetWidget.cs
//  
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
// 
// Copyright (c) 2011 Xamarin <http://xamarin.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using MonoDevelop.Core;

namespace MonoDevelop.MacDev.PlistEditor
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class IOSApplicationTargetWidget : Gtk.Bin
	{
		public IOSApplicationTargetWidget ()
		{
			this.Build ();
			
			Gtk.ListStore devices = new Gtk.ListStore (typeof (string));
			devices.AppendValues (GettextCatalog.GetString ("iPhone/iPod"));
			devices.AppendValues (GettextCatalog.GetString ("iPad"));
			devices.AppendValues (GettextCatalog.GetString ("Universal"));
			
			comboboxDevices.Model = devices;
		}
		
		public void Update (PDictionary NSDictionary)
		{
			var identifier = NSDictionary.Get<PString> ("CFBundleIdentifier");
			entryIdentifier.Text = identifier != null ? identifier : "";
			
			var version = NSDictionary.Get<PString> ("CFBundleVersion");
			entryVersion.Text = version != null ? version : "";
			
			var iphone = NSDictionary.Get<PArray> ("UISupportedInterfaceOrientations");
			var ipad   = NSDictionary.Get<PArray> ("UISupportedInterfaceOrientations~ipad");
			
			if (iphone != null && ipad != null) {
				comboboxDevices.Active = 2;
			} else if (ipad != null) {
				comboboxDevices.Active = 1;
			} else {
				comboboxDevices.Active = 0;
			}
		}
	}
}

