﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace AutomatedImageImporter
{
    public static class ZipHelper
    {
        public static ZipEntry Find(this ZipFile zf, string fileName)
        {
            fileName = fileName.ToLower();
            var files = zf.Where(z => z.FileName.ToLower() == "\\" + fileName);
            if (files.Count() == 1)
                return files.Single();
            files = zf.Where(z => z.FileName.ToLower() == fileName);
            if (files.Count() == 1)
                return files.Single();
            return null;
        }

        public static string GetStringContents(this ZipEntry ze)
        {
            return Encoding.ASCII.GetString(ze.GetBytes());
        }

        public static byte[] GetBytes(this ZipEntry ze)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ze.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] results = new byte[ms.Length];
                ms.Read(results, 0, (int)ms.Length);
                return results;
            }
        }

        public static Image GetImage(this ZipEntry ze)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ze.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return Image.FromStream(ms);
            }
        }
    }

}