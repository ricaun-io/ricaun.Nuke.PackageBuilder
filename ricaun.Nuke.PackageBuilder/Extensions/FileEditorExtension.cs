using Nuke.Common.IO;
using System;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// Provides extension methods for editing file contents by replacing specific keys with provided values.
    /// </summary>
    public static class FileEditorExtension
    {
        private const string AppNameKey = "$AppName$";
        private const string AppVersionKey = "$AppVersion$";
        private const string AppPublisherKey = "$AppPublisher$";
        private const string CompanyKey = "$Company$";
        private const string YearKey = "$Year$";

        /// <summary>
        /// Replaces the <c>$AppName$</c> key in the file with the specified value.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <param name="value">The value to replace the key with.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyAppName(this AbsolutePath path, string value)
        {
            return UpdateKeyValue(path, AppNameKey, value);
        }

        /// <summary>
        /// Replaces the <c>$AppVersion$</c> key in the file with the specified value.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <param name="value">The value to replace the key with.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyAppVersion(this AbsolutePath path, string value)
        {
            return UpdateKeyValue(path, AppVersionKey, value);
        }

        /// <summary>
        /// Replaces the <c>$AppPublisher$</c> key in the file with the specified value.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <param name="value">The value to replace the key with.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyAppPublisher(this AbsolutePath path, string value)
        {
            return UpdateKeyValue(path, AppPublisherKey, value);
        }

        /// <summary>
        /// Replaces the <c>$Company$</c> key in the file with the specified value.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <param name="value">The value to replace the key with.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyCompany(this AbsolutePath path, string value)
        {
            return UpdateKeyValue(path, CompanyKey, value);
        }

        /// <summary>
        /// Replaces the <c>$Year$</c> key in the file with the current year.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyYear(this AbsolutePath path)
        {
            return UpdateKeyValue(path, YearKey, DateTime.Now.Year.ToString());
        }

        /// <summary>
        /// Replaces a single key in the file with the specified value.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <param name="keyValue">The key to replace.</param>
        /// <param name="newValue">The value to replace the key with.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyValue(this AbsolutePath path, string keyValue, string newValue)
        {
            return UpdateKeyValues(path, new[] { keyValue }, new[] { newValue });
        }

        /// <summary>
        /// Replaces multiple keys in the file with the specified values.
        /// </summary>
        /// <param name="path">The file path to update.</param>
        /// <param name="keyValues">An array of keys to replace.</param>
        /// <param name="newValues">An array of values to replace the keys with.</param>
        /// <returns>The updated <see cref="AbsolutePath"/>.</returns>
        public static AbsolutePath UpdateKeyValues(this AbsolutePath path, string[] keyValues, string[] newValues)
        {
            if (path.FileExists())
            {
                var oldText = path.ReadAllText();
                var newText = oldText;
                for (int i = 0; i < keyValues.Length; i++)
                {
                    var value = newValues[i];
                    if (string.IsNullOrWhiteSpace(value))
                        continue;

                    newText = newText.Replace(keyValues[i], value, System.StringComparison.InvariantCultureIgnoreCase);
                }

                if (newText != oldText)
                    return path.WriteAllText(newText);
            }
            return path;
        }
    }
}
