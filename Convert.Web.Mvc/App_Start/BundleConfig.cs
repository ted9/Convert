﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebApplication3
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
 
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                       "~/app/app.js")
                       .IncludeDirectory("~/app/common", "*.js", true)
                       .IncludeDirectory("~/app/services","*.js", true)
                       .IncludeDirectory("~/app/config","*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}