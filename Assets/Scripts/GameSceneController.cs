using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSceneController
{
    void sceneNext();
    void sceneReload();
}

public interface IManaged
{
    void Initialize(IGameSceneController gameSceneController);
}

internal class GameSceneController : MonoBehaviour, IGameSceneController
{
    public void sceneNext()
    {
        throw new System.NotImplementedException();
    }

    public void sceneReload()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {

    }


}