﻿using PluginCore;
using ProjectManager.Projects.Haxe;
using System.IO;
using System.Collections.Generic;

namespace DSLCompletion
{
    /// <summary>
    /// Tries to get some completion info out of FlashDevelop without using the haxe compiler
    /// </summary>
    class FallbackCompletionHandler : ICompletionHandler
    {
        public void GetPosition(string type, PositionCallback callback)
        {
            var hxproj = PluginBase.CurrentProject as HaxeProject;
            var haxeContext = (AS2Context.Context)ASCompletion.Context.ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);

            #region Workaround for bug
            haxeContext.CurrentModel = new ASCompletion.Model.FileModel();
            if (AS2Context.Context.Panel.InvokeRequired)
            {
                AS2Context.Context.Panel.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    haxeContext.CurrentFile = "";
                });
            }
            #endregion

            var classes = haxeContext.GetAllProjectClasses();

            #region Reset workaround for bug
            haxeContext.CurrentFile = null;
            #endregion

            var split = new List<string>(type.Split('.'));
            var clazz = split[split.Count - 1];
            split.RemoveAt(split.Count - 1);
            var package = string.Join(".", split.ToArray());

            foreach (var model in classes.Items)
            {
                if (type == model.Name)
                {
                    var t = model.GetType();

                    var classModel = haxeContext.GetModel(package, clazz, "");
                    
                    if (classModel.InFile != null)
                    {
                        callback(new PositionResult(classModel.InFile.FileName, classModel.LineFrom, true));
                    }
                }
            }
        }

        public void GetFile(string file, StringCallback callback)
        {
            var hxproj = (PluginBase.CurrentProject as HaxeProject);
            var paths = ProjectManager.PluginMain.Settings.GlobalClasspaths;
            paths.AddRange(hxproj.Classpaths);
            paths.Reverse(); //later class paths have priority

            var classPaths = paths.ToArray();

            foreach (string cp in classPaths)
            {
                var path = Path.Combine(cp, file);
                if (File.Exists(path))
                {
                    callback(path);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets completion options for the given path
        /// </summary>
        /// <param name="path">The path that is currently being written. It has to end with a dot</param>
        /// <returns>a list of possible completion options or null if none were found</returns>
        public void GetCompletion(string path, ListCallback callback)
        {
            var list = new List<string>();

            var hxproj = PluginBase.CurrentProject as HaxeProject;
            var haxeContext = (AS2Context.Context)ASCompletion.Context.ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);

            #region Workaround for bug
            haxeContext.CurrentModel = new ASCompletion.Model.FileModel();
            if (AS2Context.Context.Panel.InvokeRequired)
            {
                AS2Context.Context.Panel.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    haxeContext.CurrentFile = "";
                });
            }
            #endregion

            var classes = haxeContext.GetAllProjectClasses();

            #region Reset workaround for bug
            haxeContext.CurrentFile = null;
            #endregion

            foreach (var model in classes.Items)
            {
                if (model.Name.StartsWith(path))
                {
                    var completion = model.Name.Substring(path.Length);
                    var split = completion.Split('.');

                    if (!list.Contains(split[0]))
                        list.Add(split[0]);
                }
            }

            //if (list.Count == 0) return;

            callback(list);
        }

        public void GetCompletePath(string clazz, ListCallback callback)
        {
            var results = new List<string>();

            var hxproj = PluginBase.CurrentProject as HaxeProject;
            var haxeContext = (AS2Context.Context)ASCompletion.Context.ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);

            #region Workaround for bug
            haxeContext.CurrentModel = new ASCompletion.Model.FileModel();
            if (AS2Context.Context.Panel.InvokeRequired)
            {
                AS2Context.Context.Panel.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    haxeContext.CurrentFile = "";
                });
            }
            #endregion

            var classes = haxeContext.GetAllProjectClasses();

            #region Reset workaround for bug
            haxeContext.CurrentFile = null;
            #endregion

            foreach (var model in classes.Items)
            {
                if (model.Name.EndsWith("." + clazz) && !results.Contains(model.Name))
                {
                    results.Add(model.Name);
                }
            }

            //if (results.Count == 0) return;

            callback(results);
        }
    }
}
