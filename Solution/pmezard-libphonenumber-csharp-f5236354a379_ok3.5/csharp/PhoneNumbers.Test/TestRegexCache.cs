﻿/*
 * Copyright (C) 2009 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace PhoneNumbers.Test
{
    [TestFixture]
    class TestRegexCache
    {
        private RegexCache regexCache;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            regexCache = new RegexCache(2);
        }

        [Test]
        public void TestRegexInsertion()
        {
            const String regex1 = "[1-5]";
            const String regex2 = "(?:12|34)";
            const String regex3 = "[1-3][58]";

            regexCache.GetPatternForRegex(regex1);
            Assert.That(regexCache.ContainsRegex(regex1));

            regexCache.GetPatternForRegex(regex2);
            Assert.That(regexCache.ContainsRegex(regex2));
            Assert.That(regexCache.ContainsRegex(regex1));

            regexCache.GetPatternForRegex(regex1);
            Assert.That(regexCache.ContainsRegex(regex1));

            regexCache.GetPatternForRegex(regex3);
            Assert.That(regexCache.ContainsRegex(regex3));

            Assert.False(regexCache.ContainsRegex(regex2));
            Assert.That(regexCache.ContainsRegex(regex1));
        }
    }
}