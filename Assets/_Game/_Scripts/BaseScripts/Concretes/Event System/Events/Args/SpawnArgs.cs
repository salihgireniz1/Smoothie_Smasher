using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArgs : EventArgs
{
    Vector3 _spawnPosition = Vector3.zero;
    public Vector3 SpawnPosition { get => _spawnPosition; }

    int _spawnCount = 1;
    public int SpawnCount { get => _spawnCount; }


    public SpawnArgs(Vector3 spawnPosition, int spawnCount)
    {
        _spawnPosition = spawnPosition;
        _spawnCount = spawnCount;
    }

    public SpawnArgs(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
        _spawnCount = 1;
    }

    public void Reset()
    {
        _spawnPosition = Vector3.zero;
        _spawnCount = 1;
    }
}