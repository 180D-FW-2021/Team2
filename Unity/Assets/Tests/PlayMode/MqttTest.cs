using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MqttTest
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MqttTest1()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            //yield return null;
            var gameObject = new GameObject();
            var mqttClass = gameObject.AddComponent<mqtt>();

            yield return new WaitForSeconds(20);
        }
    }
}
