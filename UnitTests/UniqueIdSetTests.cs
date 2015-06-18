﻿//
// UniqueIdSetTests.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (www.xamarin.com)
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
//

using System;

using NUnit.Framework;

using MailKit;

namespace UnitTests {
	[TestFixture]
	public class UniqueIdSetTests
	{
		[Test]
		public void TestSortedUniqueIdSet ()
		{
			UniqueId[] uids = {
				new UniqueId (1), new UniqueId (2), new UniqueId (3),
				new UniqueId (4), new UniqueId (5), new UniqueId (6),
				new UniqueId (7), new UniqueId (8), new UniqueId (9)
			};
			var list = new UniqueIdSet (uids, true);
			var actual = list.ToString ();

			Assert.AreEqual ("1:9", actual, "Incorrect initial value.");
			Assert.AreEqual (9, list.Count, "Incorrect initial count.");

			// Test Remove()

			list.Remove (uids[0]);
			actual = list.ToString ();

			Assert.AreEqual ("2:9", actual, "Incorrect results after Remove() #1.");
			Assert.AreEqual (8, list.Count, "Incorrect count after Remove() #1.");

			list.Remove (uids[uids.Length - 1]);
			actual = list.ToString ();

			Assert.AreEqual ("2:8", actual, "Incorrect results after Remove() #2.");
			Assert.AreEqual (7, list.Count, "Incorrect count after Remove() #2.");

			list.Remove (uids[4]);
			actual = list.ToString ();

			Assert.AreEqual ("2:4,6:8", actual, "Incorrect results after Remove() #3.");
			Assert.AreEqual (6, list.Count, "Incorrect count after Remove() #3.");

			// Test Add()

			list.Add (new UniqueId (5));
			actual = list.ToString ();

			Assert.AreEqual ("2:8", actual, "Incorrect results after Add() #1.");
			Assert.AreEqual (7, list.Count, "Incorrect count after Add() #1.");

			list.Add (new UniqueId (1));
			actual = list.ToString ();

			Assert.AreEqual ("1:8", actual, "Incorrect results after Add() #2.");
			Assert.AreEqual (8, list.Count, "Incorrect count after Add() #2.");

			list.Add (new UniqueId (9));
			actual = list.ToString ();

			Assert.AreEqual ("1:9", actual, "Incorrect results after Add() #3.");
			Assert.AreEqual (9, list.Count, "Incorrect count after Add() #3.");

			// Test RemoveAt()

			list.RemoveAt (0);
			actual = list.ToString ();

			Assert.AreEqual ("2:9", actual, "Incorrect results after RemoveAt() #1.");
			Assert.AreEqual (8, list.Count, "Incorrect count after RemoveAt() #1.");

			list.RemoveAt (7);
			actual = list.ToString ();

			Assert.AreEqual ("2:8", actual, "Incorrect results after RemoveAt() #2.");
			Assert.AreEqual (7, list.Count, "Incorrect count after RemoveAt() #2.");

			list.RemoveAt (3);
			actual = list.ToString ();

			Assert.AreEqual ("2:4,6:8", actual, "Incorrect results after RemoveAt() #3.");
			Assert.AreEqual (6, list.Count, "Incorrect count after RemoveAt() #3.");

			// Test adding a range of items

			list.AddRange (uids);
			actual = list.ToString ();

			Assert.AreEqual ("1:9", actual, "Incorrect results after AddRange().");
			Assert.AreEqual (9, list.Count, "Incorrect count after AddRange().");

			// Test clearing the list
			list.Clear ();
			Assert.AreEqual (0, list.Count, "Incorrect count after Clear().");
		}

		[Test]
		public void TestUnsortedUniqueIdSet ()
		{
			UniqueId[] uids = {
				new UniqueId (1), new UniqueId (2), new UniqueId (3),
				new UniqueId (4), new UniqueId (5), new UniqueId (6),
				new UniqueId (7), new UniqueId (8), new UniqueId (9)
			};
			var list = new UniqueIdSet (uids, false);
			var actual = list.ToString ();

			Assert.AreEqual ("1:9", actual, "Incorrect initial value.");
			Assert.AreEqual (9, list.Count, "Incorrect initial count.");

			// Test Remove()

			list.Remove (uids[0]);
			actual = list.ToString ();

			Assert.AreEqual ("2:9", actual, "Incorrect results after Remove() #1.");
			Assert.AreEqual (8, list.Count, "Incorrect count after Remove() #1.");

			list.Remove (uids[uids.Length - 1]);
			actual = list.ToString ();

			Assert.AreEqual ("2:8", actual, "Incorrect results after Remove() #2.");
			Assert.AreEqual (7, list.Count, "Incorrect count after Remove() #2.");

			list.Remove (uids[4]);
			actual = list.ToString ();

			Assert.AreEqual ("2:4,6:8", actual, "Incorrect results after Remove() #3.");
			Assert.AreEqual (6, list.Count, "Incorrect count after Remove() #3.");

			// Test Add()

			list.Add (new UniqueId (5));
			actual = list.ToString ();

			Assert.AreEqual ("2:4,6:8,5", actual, "Incorrect results after Add() #1.");
			Assert.AreEqual (7, list.Count, "Incorrect count after Add() #1.");

			list.Add (new UniqueId (1));
			actual = list.ToString ();

			Assert.AreEqual ("2:4,6:8,5,1", actual, "Incorrect results after Add() #2.");
			Assert.AreEqual (8, list.Count, "Incorrect count after Add() #2.");

			list.Add (new UniqueId (9));
			actual = list.ToString ();

			Assert.AreEqual ("2:4,6:8,5,1,9", actual, "Incorrect results after Add() #3.");
			Assert.AreEqual (9, list.Count, "Incorrect count after Add() #3.");

			// Test RemoveAt()

			list.RemoveAt (0);
			actual = list.ToString ();

			Assert.AreEqual ("3:4,6:8,5,1,9", actual, "Incorrect results after RemoveAt() #1.");
			Assert.AreEqual (8, list.Count, "Incorrect count after RemoveAt() #1.");

			list.RemoveAt (7);
			actual = list.ToString ();

			Assert.AreEqual ("3:4,6:8,5,1", actual, "Incorrect results after RemoveAt() #2.");
			Assert.AreEqual (7, list.Count, "Incorrect count after RemoveAt() #2.");

			list.RemoveAt (3);
			actual = list.ToString ();

			Assert.AreEqual ("3:4,6,8,5,1", actual, "Incorrect results after RemoveAt() #3.");
			Assert.AreEqual (6, list.Count, "Incorrect count after RemoveAt() #3.");

			// Test adding a range of items

			list.AddRange (uids);
			actual = list.ToString ();

			Assert.AreEqual ("3:4,6,8,5,1:2,7,9", actual, "Incorrect results after AddRange().");
			Assert.AreEqual (9, list.Count, "Incorrect count after AddRange().");

			// Test clearing the list
			list.Clear ();
			Assert.AreEqual (0, list.Count, "Incorrect count after Clear().");
		}

		[Test]
		public void TestNonSequentialUniqueIdSet ()
		{
			UniqueId[] uids = {
				new UniqueId (1), new UniqueId (3), new UniqueId (5),
				new UniqueId (7), new UniqueId (9)
			};
			var list = new UniqueIdSet (uids);
			var actual = list.ToString ();

			Assert.AreEqual ("1,3,5,7,9", actual);
		}

		[Test]
		public void TestComplexUniqueIdSet ()
		{
			UniqueId[] uids = {
				new UniqueId (1), new UniqueId (2), new UniqueId (3),
				new UniqueId (5), new UniqueId (6), new UniqueId (9),
				new UniqueId (10), new UniqueId (11), new UniqueId (12),
				new UniqueId (15), new UniqueId (19), new UniqueId (20)
			};
			var list = new UniqueIdSet (uids);
			var actual = list.ToString ();

			Assert.AreEqual ("1:3,5:6,9:12,15,19:20", actual);
		}

		[Test]
		public void TestReversedUniqueIdSet ()
		{
			UniqueId[] uids = {
				new UniqueId (20), new UniqueId (19), new UniqueId (15),
				new UniqueId (12), new UniqueId (11), new UniqueId (10),
				new UniqueId (9), new UniqueId (6), new UniqueId (5),
				new UniqueId (3), new UniqueId (2), new UniqueId (1)
			};
			var list = new UniqueIdSet (uids, false);
			var actual = list.ToString ();

			Assert.AreEqual ("20:19,15,12:9,6:5,3:1", actual, "Unexpected result for unsorted set.");

			list = new UniqueIdSet (uids, true);
			actual = list.ToString ();

			Assert.AreEqual ("1:3,5:6,9:12,15,19:20", actual, "Unexpected result for sorted set.");
		}

		[Test]
		public void TestParsingSimple ()
		{
			const string example = "1:3,5:6,9:12,15,19:20";
			UniqueIdSet uids;

			Assert.IsTrue (UniqueIdSet.TryParse (example, out uids), "Failed to parse uids.");
			Assert.AreEqual (example, uids.ToString ());
		}

		[Test]
		public void TestParsingReversedSet ()
		{
			var ids = new uint[] { 20, 19, 15, 12, 11, 10, 9, 6, 5, 3, 2, 1 };
			const string example = "20:19,15,12:9,6:5,3:1";
			UniqueIdSet uids;

			Assert.IsTrue (UniqueIdSet.TryParse (example, out uids), "Failed to parse uids.");
			Assert.AreEqual (example, uids.ToString ());
			Assert.AreEqual (ids.Length, uids.Count);

			for (int i = 0; i < uids.Count; i++)
				Assert.AreEqual (ids[i], uids[i].Id);
		}
	}
}
