using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SS3D.Core;
using SS3D.Core.Settings;
using SS3D.Systems.Rounds;
using SS3D.UI.Buttons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Coimbra;
using System.Diagnostics;
using FishNet.Managing;
using FishNet;
using SS3D.Systems.InputHandling;
using SS3D.Systems.Entities.Humanoid;
using System.Text;
using System.IO;

namespace SS3D.Tests
{
    public class ClientGameActions : SpessClientPlayModeTest
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

        }

        [UnitySetUp]
        public override IEnumerator UnitySetUp()
        {
            yield return base.UnitySetUp();
            //yield return TestHelpers.StartAndEnterRound();
            //yield return GetController();
        }

        [UnityTearDown]
        public override IEnumerator UnityTearDown()
        {
            //yield return TestHelpers.FinishAndExitRound();
            yield return base.UnityTearDown();

        }

        [UnityTest]
        public IEnumerator PlayerCanMoveInEachDirectionCorrectly()
        {
            //yield return PlaymodeTestRepository.PlayerCanMoveInEachDirectionCorrectly(controller);

            string batPath = Application.dataPath;
            batPath = batPath.Substring(0, batPath.Length - 6);     // Needed to remove the "Assets" folder.
            batPath += "Builds";


            #if UNITY_STANDALONE_LINUX
            // Automated tests run on Ubuntu, and create the files in StandaloneWindows directory.
            batPath += "/StandaloneWindows";
            #endif

            string[] allFiles = Directory.GetFiles(batPath);
            StringBuilder sb = new StringBuilder();
            sb.Append($"List of all files in {batPath}\n");
            foreach (string file in allFiles)
            {
                sb.Append($"{file}\n");
            }
            string[] allDirectories = Directory.GetDirectories(batPath);
            sb.Append($"List of all directories in {batPath}\n");
            foreach (string file in allDirectories)
            {
                sb.Append($"{file}\n");
            }

            Assert.IsTrue(false, sb.ToString());

            yield return null;

        }
    }
}
