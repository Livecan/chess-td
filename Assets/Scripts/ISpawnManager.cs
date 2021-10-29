using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnManager
{
    public void Initialize(IEnumerable<Position> spawnPositions);

    public void Spawn();
}
