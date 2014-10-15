using System;
using System.Collections.Generic;
using System.Linq;

namespace Arana.VersionManager.Helpers {
    public class StringHelpers {
        public static string NonNullOrEmpty(params string[] values) {
            return values.FirstOrDefault(value => !string.IsNullOrEmpty(value));
        }
    }
}