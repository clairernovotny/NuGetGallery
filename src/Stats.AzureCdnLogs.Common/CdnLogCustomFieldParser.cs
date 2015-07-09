// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stats.AzureCdnLogs.Common
{
    public class CdnLogCustomFieldParser
    {
        public static IDictionary<string, string> Parse(string customField)
        {
            var temp = customField;
            if (customField.StartsWith("\"") && customField.EndsWith("\""))
            {
                // remove surrounding quotes
                temp = customField.Substring(1, customField.Length - 2);
            }

            // extract all custom fields
            var dictionary = new Dictionary<string, string>();
            var customFields = Regex.Matches(temp, @"(?<key>[^\s]+[:]{1})[\s]{1}(?<value>([-]*\s)|(\w+\s)|([a-zA-Z0-9\/\.\s\(\;\)]*))");

            foreach (Match match in customFields)
            {
                string key = match.Groups["key"].Value.Replace(":", string.Empty).Trim();
                string value = match.Groups["value"].Value.Trim();

                if (dictionary.ContainsKey(key))
                {
                    // just overwrite it, avoid crashing the job if CDN configuration has twice the same key in the custom field
                    dictionary[key] = value;
                }
                else dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}