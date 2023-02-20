using Characters;
using System.Collections;
using UnityEngine;

namespace Items
{
    public interface ICollectable 
    {
        void OnCollect(Character character);

    }
}