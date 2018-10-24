﻿/*
 * Original work Copyright (c) 2016 Lokra Studio
 * Url: https://github.com/lokra/seaweedfs-client
 * Modified work Copyright 2017 smartbox software solutions
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

namespace Bzway.Framework.DistributedFileSystemClient.Utils
{
    public static class TimeToLiveBuilder
    {
        public static string BuildMinutes(int count)
        {
            return count + "m";
        }

        public static string BuildHours(int count)
        {
            return count + "h";
        }

        public static string BuildDays(int count)
        {
            return count + "d";
        }

        public static string BuildWeeks(int count)
        {
            return count + "w";
        }

        public static string BuildMonths(int count)
        {
            return count + "M";
        }

        public static string BuildYears(int count)
        {
            return count + "y";
        }
    }
}
