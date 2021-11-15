using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// POLYMORPHISM
public interface ISpawnManager
{
    public void Initialize(IEnumerable<Position> spawnPositions);

    public void Spawn();
}
