﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Common;
using SpecFlow.TestProjectGenerator;
using SpecFlow.TestProjectGenerator.Helpers;
using SpecFlow.TestProjectGenerator.NewApi;
using TechTalk.SpecFlow.Infrastructure;


namespace TechTalk.SpecFlow.Specs.Support
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario()]
        public void BeforeScenario()
        {
            _scenarioContext.ScenarioContainer.RegisterTypeAs<OutputConnector, IOutputWriter>();
        }

        [BeforeTestRun]
        public static void BeforTestRun()
        {
            DeletePackageVersionFolders();
        }

        private static void DeletePackageVersionFolders()
        {
            var appConfigDriver = new AppConfigDriver();
            var folders = new Folders(appConfigDriver);

            var currentVersionDriver = new CurrentVersionDriver(folders, new OutputConnector(new SpecFlowOutputHelper()));
            string[] packageNames = { "SpecFlow", "SpecFlow.CustomPlugin", "SpecFlow.MsTest", "SpecFlow.NUnit", "SpecFlow.NUnit.Runners", "SpecFlow.Tools.MsBuild.Generation", "SpecFlow.xUnit" };
            
            foreach (var name in packageNames)
            {
                string hooksPath = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".nuget", "packages", name, currentVersionDriver.GitVersionInfo.NuGetVersion);
                FileSystemHelper.DeleteFolder(hooksPath);
            }
        }
    }
}