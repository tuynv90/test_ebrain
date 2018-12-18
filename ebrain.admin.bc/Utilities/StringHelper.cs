﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ebrain.admin.bc.Utilities
{
    public static class StringHelper
    {
        static string PATH_DOCUMENTS = "{0}/uploads/documents/{1}.{2}";
        static string PATH_PROFIERS = "{0}/uploads/profiers/{1}{2}";
        static string PATH_LOGOS = "{0}/uploads/logos/{1}{2}";
        static string PATH_STUDENTS = "{0}/uploads/students/{1}{2}";

        static IHostingEnvironment _env;

        private static string WebRootPath(string pathRoot, string fileName, string branchId)
        {
            var path = string.Format(pathRoot, _env.WebRootPath,
                       branchId.Replace("-", string.Empty),
                       fileName.GetFileName());

            return path;
        }

        public static string WebRootPathDocumentDownload(this string fileName, string branchId, IHostingEnvironment env)
        {
            if (string.IsNullOrEmpty(fileName)) return "javascript:;";
            _env = env;
            var path = string.Format(PATH_DOCUMENTS, string.Empty,
                       branchId.Replace("-", string.Empty),
                       fileName.GetFileName());
            return path;
        }

        public static string WebRootPathLogo(this string fileName)
        {
            var path = string.IsNullOrEmpty(fileName) ? Constants.IMAGE_DEFAULT : string.Format(PATH_LOGOS, string.Empty, string.Empty, fileName.GetFileName());
            return path;
        }

        public static string WebRootPathLogo(this string fileName, IHostingEnvironment env)
        {
            var path = string.IsNullOrEmpty(fileName) ? Constants.IMAGE_DEFAULT : string.Format(PATH_LOGOS, string.Empty, string.Empty, fileName.GetFileName());
            path = env.WebRootPath + path;
            return path;
        }



        public static string WebRootPathStudent(this string fileName)
        {
            var path = string.Format(PATH_STUDENTS, string.Empty, string.Empty, fileName.GetFileName());
            return path;
        }

        public static string WebRootPathProfiler(this string fileName)
        {
            var path = string.Format(PATH_PROFIERS, string.Empty, string.Empty, fileName.GetFileName());
            return path;
        }

        public static string WebRootPathDocument(this string fileName, string branchId, IHostingEnvironment env)
        {
            _env = env;
            return WebRootPath(PATH_DOCUMENTS, fileName, branchId);
        }

        public static string WebRootPathProfier(this string fileName, string branchId, IHostingEnvironment env)
        {
            _env = env;
            return WebRootPath(PATH_PROFIERS, $"{branchId}-{fileName}", string.Empty);
        }

        public static string WebRootPathLogo(this string fileName, string branchId)
        {
            return WebRootPath(PATH_LOGOS, fileName, branchId);
        }

        public static string WebRootPathStudent(this string fileName, string branchId, IHostingEnvironment env)
        {
            _env = env;
            return WebRootPath(PATH_STUDENTS, $"{branchId}-{fileName}", string.Empty);
        }

        public static string GetFileName(this string fileName)
        {
            return System.IO.Path.GetFileName(fileName);
        }

        public static void WriteAllBytes(this byte[] imageBytes, string filePath)
        {
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            System.IO.File.WriteAllBytes(filePath, imageBytes);
        }

        public static string ConvertImageToBase64(this string filePath)
        {
            string base64ImageRepresentation = string.Empty;
            if (IsExistFile(filePath))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                base64ImageRepresentation = Convert.ToBase64String(imageArray);
            }
            return base64ImageRepresentation;
        }


        public static bool IsExistFile(this string path)
        {
            return File.Exists(path);
        }

        #region IntParseFast
        /// <summary>
        /// http://www.dotnetperls.com/int-parse-optimization
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IntParseFast(this string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char letter = value[i];
                result = 10 * result + (letter - 48);
            }
            return result;
        }
        /// <summary>
        /// test is number with 1000000 items
        /// IsDigitsOnly(Code below) : 384588
        /// TryParse:     639583
        /// Regex:        1329571
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static int IntParseFastWithCheck(string value)
        {
            if (value != null)
                value = value.Trim();
            return IntParseFast(value);
        }
        #endregion

        /// <summary>
        /// http://www.dotnetperls.com/int-parse-optimization
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long LongParseFast(this string value)
        {
            long result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char letter = value[i];
                result = 10 * result + (letter - 48);
            }
            return result;
        }

        /// <returns></returns>
        public static long longParseFastWithCheck(this string value)
        {
            if (value == null)
                return 0;
            if (value != null)
                value = value.Trim();

            return LongParseFast(value);
        }
        /// <summary>
        /// Dùng để cast những đối tượng còn nghi ngờ không phải số
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckStringIsNumber(this string value)
        {
            if (value != null)
                value = value.Trim();

            #region Check In Valid
            if (string.IsNullOrEmpty(value))
                return false;

            #region test is number with 1000000 items
            if (IsDigitsOnly(value) == false)
                return false;
            #endregion
            #endregion
            return true;
        }

        public static string ConvertDecimalToCurrency(this decimal? value)
        {
            value = !value.HasValue ? 0 : value.Value;
            return String.Format("{0:0,0.0}", value);
        }

        public static string FormatYYMMDD(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : DateTime.Now.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        }

        public static string FormatHHMM(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("HH:mm") : DateTime.Now.ToString("HH:mm");
        }

        public static Guid ConvertStringToGuid(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Guid outGuid;
                if (Guid.TryParse(value, out outGuid))
                {
                    return outGuid;
                }
            }
            return Guid.NewGuid();
        }
    }
}
