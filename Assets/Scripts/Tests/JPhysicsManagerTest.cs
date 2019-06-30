using Jerre.JPhysics;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class JPhysicsManagerTest
    {
        [Test]
        public void Calculate_reflection_of_incoming_vector()
        {
            TestMethods.AreEqualIsh(new Vector3(-1, 0, 1), PhysicsManager.ReflectIncomingVector(new Vector3(1, 0, 1), Vector3.left));
            TestMethods.AreEqualIsh(new Vector3(2, 0, 0), PhysicsManager.ReflectIncomingVector(new Vector3(0, 0, -2), new Vector3(1, 0, 1).normalized));
        }

        [Test]
        public void Calculate_bounce_of_incoming_vector()
        {
            TestMethods.AreEqualIsh(new Vector3(-2, 0, 1), PhysicsManager.ScaleVectorInNormalDirection(new Vector3(-1, 0, 1), Vector3.left, 2f));
            TestMethods.AreEqualIsh(new Vector3(-1, 0, 2), PhysicsManager.ScaleVectorInNormalDirection(Vector3.forward, new Vector3(-1, 0, 1), 2f));
            TestMethods.AreEqualIsh(new Vector3(1, 0, -2), PhysicsManager.ScaleVectorInNormalDirection(Vector3.back, new Vector3(1, 0, -1), 2f));
            TestMethods.AreEqualIsh(new Vector3(-0.4f, 0, 1.2f), PhysicsManager.ScaleVectorInNormalDirection(Vector3.forward, new Vector3(-2, 0, 1).normalized, 2f));
        }
    }
}
