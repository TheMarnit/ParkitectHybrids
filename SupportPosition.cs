using System;
using UnityEngine;

namespace HybridCoasters
{

    public class supportPosition
    {
        public Vector3 leftVerticalSupportPost;
        public Vector3 rightVerticalSupportPost;
        public Vector3 projectedTangentDirection;
        public supportPosition(Vector3 leftPost, Vector3 rightPost, Vector3 tangent)
        {
            leftVerticalSupportPost = leftPost;
            rightVerticalSupportPost = rightPost;
            projectedTangentDirection = tangent;
        }
    }
}