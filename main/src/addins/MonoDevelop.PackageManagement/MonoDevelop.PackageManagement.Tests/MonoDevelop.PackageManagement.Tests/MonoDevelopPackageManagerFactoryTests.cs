﻿//
// MonoDevelopPackageManagerFactoryTests.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
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
using MonoDevelop.PackageManagement;
using NuGet;
using NUnit.Framework;
using MonoDevelop.PackageManagement.Tests.Helpers;

namespace MonoDevelop.PackageManagement.Tests
{
	[TestFixture]
	public class MonoDevelopPackageManagerFactoryTests
	{
		MonoDevelopPackageManagerFactory factory;
		IPackageManager packageManager;
		FakePackageRepository fakePackageRepository;
		FakeDotNetProject testProject;
		PackageManagementOptions options;
		FakePackageRepositoryFactory fakePackageRepositoryFactory;
		FakeProjectSystemFactory fakeProjectSystemFactory;

		void CreateFactory ()
		{
			options = new TestablePackageManagementOptions ();
			fakePackageRepositoryFactory = new FakePackageRepositoryFactory ();
			fakeProjectSystemFactory = new FakeProjectSystemFactory ();
			factory = new MonoDevelopPackageManagerFactory (fakePackageRepositoryFactory, fakeProjectSystemFactory, options);
		}

		void CreateTestProject ()
		{
			testProject = ProjectHelper.CreateTestProject ();
			var solution = new FakeSolution {
				BaseDirectory = @"c:\projects\MyProject\".ToNativePath ()
			};
			testProject.ParentSolution = solution;
		}

		void CreatePackageManager ()
		{
			fakePackageRepository = new FakePackageRepository ();
			packageManager = factory.CreatePackageManager (fakePackageRepository, testProject);
		}

		[Test]
		public void CreatePackageManager_ProjectAndSolutionHaveDifferentFolders_PackageManagerLocalRepositoryIsSharedRepository ()
		{
			CreateFactory ();
			CreateTestProject ();
			CreatePackageManager ();
			ISharedPackageRepository sharedRepository = packageManager.LocalRepository as ISharedPackageRepository;

			Assert.IsNotNull (sharedRepository);
		}

		[Test]
		public void CreatePackageManager_PackagesSolutionFolderDefinedInOptions_LocalRepositoryFileSystemIsPackageManagerFileSystem ()
		{
			CreateFactory ();
			CreateTestProject ();
			CreatePackageManager ();

			Assert.AreEqual (packageManager.FileSystem, fakePackageRepositoryFactory.FileSystemPassedToCreateSharedRepository);
		}
	}
}

